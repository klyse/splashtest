using Microsoft.AspNetCore.SignalR;

namespace web;

public class LoadTestHub : Hub
{
    public class Progress
    {
        public string Message { get; set; }
    }
    public void Run(string host)
    {
        Clients.All.SendCoreAsync("progress", new[] { new Progress
        {
            Message = "hi " + Guid.NewGuid()
        } });
    }
}