using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => $"Hello World from {RuntimeInformation.FrameworkDescription} on {RuntimeInformation.OSDescription}!");

app.Run();
