using Microsoft.AspNetCore.Mvc;
using UserGroupLEDs.Api.Models;

namespace UserGroupLEDs.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LightsController : ControllerBase
{
    private readonly ILogger<LightsController> _logger;

    public LightsController(ILogger<LightsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public Settings Get()
    {
        return new Settings
        {
            Brightness = 100,
            Colors = null,
            Duration = 10,
            LEDCount = 300,
            Pattern = Pattern.Wipe,
            Steps = 5
        };
    }
}