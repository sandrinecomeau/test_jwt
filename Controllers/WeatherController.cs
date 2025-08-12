using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [Authorize]
    [HttpGet]
    public IActionResult GetWeather()
    {
        var rng = new Random();
        var start = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        var data = Enumerable.Range(0, 5).Select(i => new
        {
            Date = start.AddDays(i),
            TemperatureC = rng.Next(-20, 35),
            TemperatureF = 32 + (int)(rng.Next(-20, 35) / 0.5556),
            Summary = Summaries[rng.Next(Summaries.Length)]
        });

        return Ok(data);
    }
}
