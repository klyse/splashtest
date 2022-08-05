using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using web.Cypress;
using web.Db;
using web.Models;

namespace web.Workers;

public class CypressWorker : BackgroundService
{
    private readonly ILogger<CypressWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CypressWorker(ILogger<CypressWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public const string MediaPath = "./media/";

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
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        Directory.CreateDirectory(path);
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
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                var cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, stoppingToken).Token;

                var psiNpmRunDist = new ProcessStartInfo
                {
                    FileName = "bash",
                    RedirectStandardInput = true,
                    WorkingDirectory = basePath
                };
                using var pNpmRunDist = Process.Start(psiNpmRunDist);
                await pNpmRunDist!.StandardInput.WriteLineAsync("npx cypress run . || exit $?");
                await pNpmRunDist.WaitForExitAsync(stoppingToken);

                run.State = pNpmRunDist.ExitCode != 0 ? State.Failed : State.Succeeded;

                await splashContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                run.State = State.Failed;
                await splashContext.SaveChangesAsync(stoppingToken);
            }

            var runDirectory = MediaPath + run.Id + "/";
            ClearDir(runDirectory);
            File.Move(videoPath, runDirectory + "video.mp4");

            foreach (var file in Directory.GetFiles(screenshotsBasePath))
            {
                File.Move(file, runDirectory + "photo.png");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}