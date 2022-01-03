using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserGroupLEDs.Common.Models;
using UserGroupLEDs.Api.Models;

var settings = new Settings();
var abort = new AbortRequest();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddHostedService<LongRunningService>();
builder.Services.AddSingleton<BackgroundWorkerQueue>();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapPost("/api/load", () => LoadSettings());
app.MapPost("/api/save", () => SaveSettings());

app.MapGet("/", () => settings);

app.MapGet("/api/colors", () => settings.Colors);

app.MapPost("/api/colors", (List<string> model) =>
{
    settings.Colors.Clear();

    foreach (var currentColor in model)
    {
        settings.Colors.Add(ColorTranslator.FromHtml(currentColor));
        Console.WriteLine(settings.Colors[settings.Colors.Count - 1]);
    }
});

app.MapGet("/api/pattern", () => settings.Pattern);

app.MapPost("/api/pattern", ([FromBody]Pattern model) =>
{
    settings.Pattern = model;
    Console.WriteLine(settings.Pattern);
});

app.MapGet("/api/ledcount", () => settings.LEDCount);

app.MapPost("/api/ledcount", ([FromBody]int model) => {
    settings.LEDCount = model;
    Console.WriteLine(settings.LEDCount);
});

app.MapGet("/api/brightness", () => settings.Brightness);

app.MapPost("/api/brightness", ([FromBody]byte model) => {
    settings.Brightness = model;
    Console.WriteLine(settings.Brightness);
});

app.MapPost("/api/turnOff", () => abort.IsAbortRequested = true);

app.MapPost("/api/turnOn", async (BackgroundWorkerQueue backgroundWorkerQueue) => await TurnOnLights(backgroundWorkerQueue));
LoadSettings();

app.Run();

void LoadSettings()
{
    if (File.Exists("settings.json"))
    {
        var options = new JsonSerializerOptions()
        {
            Converters = {
                new ColorJsonConverter()
            }
        };
        using (var reader = new StreamReader("settings.json"))
        {
            settings = JsonSerializer.Deserialize<Settings>(reader.ReadToEnd(), options);
        }
    }
}

void SaveSettings()
{
    var options = new JsonSerializerOptions()
    {
        Converters = {
            new ColorJsonConverter()
        }
    };
    using (var writer = new StreamWriter("settings.json"))
    {
        writer.WriteLine(JsonSerializer.Serialize(settings, options));
    }
}

async Task TurnOnLights(BackgroundWorkerQueue backgroundWorkerQueue)
{
    abort.IsAbortRequested = false;
    IColorPattern colorPattern = null;

    colorPattern = settings.Pattern switch
    {
        Pattern.Fade => new ColorFade(),
        Pattern.Rainbow => new ColorFade(),
        Pattern.Wipe => new ColorWipe()
    };

    backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
    {
        await Task.Run(() => colorPattern.Execute(settings, abort));
    });
}
