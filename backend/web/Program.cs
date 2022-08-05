using web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app.MapGet("/tests", () => new List<Test>
{
    new()
    {
        Id = Guid.NewGuid(),
        State = State.Failed,
        TestCode = "this is a random string for the test code"
    },
    new()
    {
        Id = Guid.NewGuid(),
        State = State.Pending,
        TestCode = "this is a random string for the test code"
    },
    new()
    {
        Id = Guid.NewGuid(),
        State = State.Running,
        TestCode = "this is a random string for the test code"
    }
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();