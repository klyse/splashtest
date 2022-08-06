using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;

namespace web;

public class LoadTestHub : Hub
{
    private readonly IServiceProvider _serviceProvider;

    public LoadTestHub(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public class Progress
    {
        public string Message { get; set; }
    }

    private const string BasePath = "../k6";

    public async Task Run(string host)
    {
        var psiNpmRunDist = new ProcessStartInfo
        {
            FileName = "bash",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            WorkingDirectory = BasePath
        };
        using var pNpmRunDist = Process.Start(psiNpmRunDist);
        pNpmRunDist!.BeginOutputReadLine();
        await pNpmRunDist.StandardInput.WriteLineAsync("k6 run test.js; exit $?");
        pNpmRunDist.OutputDataReceived += (_, args) =>
        {
            Clients.All.SendCoreAsync("progress", new object?[]
            {
                new Progress
                {
                    Message = args.Data
                }
            });
        };
        await pNpmRunDist.WaitForExitAsync();
    }
}