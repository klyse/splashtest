using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web;
using web.Db;
using web.Models;
using web.Workers;

if (!Directory.Exists(CypressWorker.MediaPath)) Directory.CreateDirectory(CypressWorker.MediaPath);

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
    .AddDbContext<SplashContext>(config => { config.UseNpgsql(connectionString); })
    .AddHostedService<CypressWorker>()
    .AddSignalR();

var app = builder.Build();

app.MapGet("/tests", async (SplashContext db) => 
    (await db.Tests.Include(c => c.Runs)
        .OrderByDescending(c => c.CreatedDateTime)
    .ToListAsync())
    .Select(c => c.Project()));

app.MapPost("/tests", async (TestDao n, SplashContext db) =>
{
    var dbObj = db.Tests.Add(n.Project());
    await db.SaveChangesAsync();
    return dbObj.Entity.Project();
});

app.MapPost("/test/{testId:Guid}/run", async (Guid testId, SplashContext db) =>
{
    var test = await db.Tests.SingleAsync(c => c.Id == testId);
    var run = new Run
    {
        TestId = test.Id,
        State = State.Pending
    };
    test.Runs.Add(run);
    await db.SaveChangesAsync();

    return run.Project();
});

app.MapGet("/run/{runId:Guid}", async (Guid runId, SplashContext db) =>
{
    var run = await db.Runs.SingleAsync(c => c.Id == runId);

    return run.Project();
});

app.MapGet("/run/{runId:Guid}/video", async (Guid runId) =>
{
    var fileName = CypressWorker.MediaPath + runId + "/video.mp4";
    var filestream = File.OpenRead(fileName);

    return Results.File(filestream, "video/mp4",
        fileName, enableRangeProcessing: true);
});

app.MapGet("/run/{runId:Guid}/photo", async (Guid runId) =>
{
    var fileName = CypressWorker.MediaPath + runId + "/photo.png";
    var image = File.OpenRead(fileName);
    return Results.File(image, "image/png");
});

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();
app.MapHub<LoadTestHub>("/load-test");

using (var serviceScope = app.Services.CreateScope())
{
    var splashContext = serviceScope.ServiceProvider.GetRequiredService<SplashContext>();
    await splashContext.Database.MigrateAsync();
}

app.Run();