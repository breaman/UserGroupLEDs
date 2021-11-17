using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => $"UserGroupLEDS.Web running on {RuntimeInformation.FrameworkDescription} on {RuntimeInformation.OSDescription}!");

app.Run();
