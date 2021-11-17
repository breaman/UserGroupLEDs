using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => $"UserGroupLEDS.Alexa running on {RuntimeInformation.FrameworkDescription} on {RuntimeInformation.OSDescription}!");

app.Run();
