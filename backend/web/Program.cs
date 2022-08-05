var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/tests", () => "Hello World!");

app.Run();