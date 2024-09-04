using System;

namespace StreamKit.Server.WebApp;

public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public string? Summary { get; set; }
    public int TemperatureC { get; init; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
