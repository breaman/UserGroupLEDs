using System.Drawing;
using rpi_ws281x;

namespace UserGroupLEDs.Common.Models;

public class ColorFade : IColorPattern
{
    public void Execute(Settings settings, AbortRequest abortRequest)
    {
        var lightSettings = rpi_ws281x.Settings.CreateDefaultSettings();
        var controller = lightSettings.AddController(settings.LEDCount, Pin.Gpio18, StripType.WS2812_STRIP);
        controller.Brightness = settings.Brightness;

        using (var device = new WS281x(lightSettings))
        {
            while (!abortRequest.IsAbortRequested)
            {
                foreach (var color in settings.Colors)
                {
                    Fade(device, color);
                }
                // Fade(device, Color.DarkOrange);
                // Fade(device, Color.Purple);
                // Fade(device, Color.SaddleBrown);
                // Fade(device, Color.ForestGreen);
            }
            device.Reset();
        }
    }

    private static void Fade(WS281x device, Color color)
    {
        var controller = device.GetController();

        controller.SetAll(color);

        device.Render();

        // for (byte i = 0; i <= 150; i++)
        // {
        //     controller.Brightness = i;
        //     device.Render(true);
        //     Thread.Sleep(20);
        // }

        Thread.Sleep(2000);
    }
}