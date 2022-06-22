using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace PollyExample.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastController()
    {
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        var rng = new Random();
        if (rng.Next() % 3 == 0)
            return StatusCode((int)HttpStatusCode.TooManyRequests);
        
        return Ok(Summaries[rng.Next(Summaries.Length)]);
    }
}