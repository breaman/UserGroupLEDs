using System.Drawing;
using rpi_ws281x;

namespace UserGroupLEDs.Common.Models;

public class ColorWipe : IColorPattern
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
                    Wipe(device, color);
                }
                // Wipe(device, Color.DarkOrange);
                // Wipe(device, Color.Purple);
                // Wipe(device, Color.SaddleBrown);
                // Wipe(device, Color.ForestGreen);
            }
            device.Reset();
        }
    }

    private static void Wipe(WS281x device, Color color)
    {
        var controller = device.GetController();
        for (int i = 0; i < controller.LEDCount; i++)
        {
            controller.SetLED(i, color);
            device.Render();

            var waitPeriod = (int)Math.Max(500.0 / controller.LEDCount, 5.0);
            Thread.Sleep(waitPeriod);
        }
    }
}