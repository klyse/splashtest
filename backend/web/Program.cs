using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Db;
using web.Models;

const string videosPath = "./videos/";

if (!Directory.Exists(videosPath))
{
    Directory.CreateDirectory(videosPath);
}

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("db");

builder.Services
    .Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(c => c.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .Configure<JsonOptions>(c => c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddCors(c => c.AddDefaultPolicy(p => p
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowCredentials()
        .AllowAnyMethod()))
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContext<SplashContext>(config => { config.UseNpgsql(connectionString); });

var app = builder.Build();

app.MapGet("/tests", async (SplashContext db) => (await db.Tests.Include(c => c.Runs).ToListAsync()).Select(c => c.Project()));

app.MapPost("/tests", async (TestDao n, SplashContext db) =>
{
    var dbObj = db.Tests.Add(n.Project());
    await db.SaveChangesAsync();
    return dbObj.Entity.Project();
});

app.MapPost("/run/{testId:Guid}", async (Guid testId, SplashContext db, IServiceProvider provider) =>
{
    var test = await db.Tests.SingleAsync(c => c.Id == testId);
    var run = new Run
    {
        TestId = test.Id,
        State = State.Running,
        RunDateTime = DateTime.UtcNow
    };
    test.Runs.Add(run);
    await db.SaveChangesAsync();

    var thread = new Thread(async () =>
    {
        var splashContext = provider.CreateScope().ServiceProvider.GetRequiredService<SplashContext>();
        splashContext.Runs.Attach(run);

        var basePath = "../cypress";
        var psiNpmRunDist = new ProcessStartInfo
        {
            FileName = "bash",
            RedirectStandardInput = true,
            WorkingDirectory = basePath
        };
        using var pNpmRunDist = Process.Start(psiNpmRunDist);
        pNpmRunDist!.StandardInput.WriteLine("npx cypress run . && exit 0");
        pNpmRunDist.WaitForExit();

        var videoPath = basePath + "/cypress/videos/test.cy.js.mp4";
        File.Move(videoPath, videosPath + run.Id + ".mp4");
        run.State = State.Succeeded;
        await splashContext.SaveChangesAsync();
    });
    thread.Start();

    return run.Id;
});

app.MapGet("/run/{runId:Guid}/video", async (Guid runId)  =>
{
    var fileName = videosPath + runId + ".mp4";
    var filestream = File.OpenRead(fileName);

    return Results.File(filestream, contentType: "video/mp4", 
        fileDownloadName: fileName, enableRangeProcessing: true); 

});

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

using (var serviceScope = app.Services.CreateScope())
{
    var splashContext = serviceScope.ServiceProvider.GetRequiredService<SplashContext>();
    await splashContext.Database.MigrateAsync();
}

app.Run();