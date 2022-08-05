using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using web.Db;
using web.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("db");

builder.Services
    .Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(c => c.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddCors(c => c.AddDefaultPolicy(p => p
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowCredentials()
        .AllowAnyMethod()))
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContext<SplashContext>(config => { config.UseNpgsql(connectionString); });

var app = builder.Build();

app.MapGet("/tests", async (SplashContext db) => (await db.Tests.ToListAsync()).Select(c => c.Project()));

app.MapPost("/tests", async (TestDao n, SplashContext db) =>
{
    var dbObj = db.Tests.Add(n.Project());
    await db.SaveChangesAsync();
    return dbObj.Entity.Project();
});

app.MapGet("/runs", () => { });

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

using (var serviceScope = app.Services.CreateScope())
{
    var splashContext = serviceScope.ServiceProvider.GetRequiredService<SplashContext>();
    await splashContext.Database.MigrateAsync();
}

app.Run();