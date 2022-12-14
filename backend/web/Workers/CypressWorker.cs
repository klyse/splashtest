using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using web.Cypress;
using web.Db;
using web.Models;

namespace web.Workers;

public class CypressWorker : BackgroundService
{
    public const string MediaPath = "./media/";
    private readonly ILogger<CypressWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CypressWorker(ILogger<CypressWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await CycleAsync(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CypressWorker error");
        }
    }

    private void ClearDir(string path)
    {
        if (Directory.Exists(path)) Directory.Delete(path, true);

        Directory.CreateDirectory(path);
    }

    private void SafeGuard(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error");
        }
    }

    private async Task CycleAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = _serviceProvider.CreateScope();
            var splashContext = scope.ServiceProvider.GetRequiredService<SplashContext>();

            var run = await splashContext.Runs
                .Include(c => c.Test)
                .FirstOrDefaultAsync(c => c.State == State.Pending, stoppingToken);

            if (run is null)
            {
                await Task.Delay(1000, stoppingToken);
                continue;
            }

            run.State = State.Running;
            run.RunDateTime = DateTime.UtcNow;
            await splashContext.SaveChangesAsync(stoppingToken);

            const string basePath = "../cypress";
            const string videoPath = basePath + "/cypress/videos/test.cy.js.mp4";
            const string screenshotsBasePath = basePath + "/cypress/screenshots/test.cy.js";

            ClearDir(basePath + "/cypress/videos");
            ClearDir(basePath + "/cypress/screenshots");
            ClearDir(basePath + "/cypress/e2e");

            var fileContent = new FileGenerator().GetFileContent(run.Test.TestSteps!);
            await File.WriteAllTextAsync(basePath + "/cypress/e2e/" + "test.cy.js", fileContent, stoppingToken);

            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                var cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, stoppingToken).Token;

                var psiNpmRunDist = new ProcessStartInfo
                {
                    FileName = "bash",
                    RedirectStandardInput = true,
                    WorkingDirectory = basePath
                };
                using var pNpmRunDist = Process.Start(psiNpmRunDist);
                await pNpmRunDist!.StandardInput.WriteLineAsync("npx cypress run . --headless; exit $?");
                await pNpmRunDist.WaitForExitAsync(cancellationToken);

                run.State = pNpmRunDist.ExitCode != 0 ? State.Failed : State.Succeeded;

                await splashContext.SaveChangesAsync(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Exceeded max run time for TestId {TestId}", run.Test.Id);
                run.State = State.Failed;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Execution error for TestId {TestId}", run.Test.Id);
                run.State = State.Failed;
            }

            var runDirectory = MediaPath + run.Id + "/";
            ClearDir(runDirectory);
            SafeGuard(() => { File.Move(videoPath, runDirectory + "video.mp4"); });

            SafeGuard(() =>
            {
                if (!Directory.Exists(screenshotsBasePath))
                    return;

                foreach (var file in Directory.GetFiles(screenshotsBasePath))
                {
                    File.Move(file, runDirectory + "photo.png");
                    break;
                }
            });

            await splashContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}