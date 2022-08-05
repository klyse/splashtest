using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using web.Db;
using web.Models;

namespace web.Workers;

public class CypressWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public CypressWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public const string VideosPath = "./videos/";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = _serviceProvider.CreateScope();
            var splashContext = scope.ServiceProvider.GetRequiredService<SplashContext>();

            var run = await splashContext.Runs.FirstOrDefaultAsync(c => c.State == State.Pending, stoppingToken);

            if (run is null)
            {
                await Task.Delay(1000, stoppingToken);
                continue;
            }
            
            run.State = State.Running;
            run.RunDateTime = DateTime.UtcNow;
            await splashContext.SaveChangesAsync(stoppingToken);

            const string basePath = "../cypress";

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
                await pNpmRunDist!.StandardInput.WriteLineAsync("npx cypress run .");
                await pNpmRunDist.WaitForExitAsync(stoppingToken);

                run.State = State.Succeeded;
                await splashContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                run.State = State.Failed;
                await splashContext.SaveChangesAsync(stoppingToken);
            }
            
            const string videoPath = basePath + "/cypress/videos/test.cy.js.mp4";
            File.Move(videoPath, VideosPath + run.Id + ".mp4");
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}