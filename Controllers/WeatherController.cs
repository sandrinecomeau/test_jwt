using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemo.Controllers;

[ApiController]
[Route("weather")]
public class WeatherController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Neige", "Pluie", "Nuageux", "Partiellement ensoleillé", "Soleil",
        "Verglas", "Orages", "Orages violents", "Brume", "Brouillard"
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
            Temperature = rng.Next(-20, 35),
            Météo = Summaries[rng.Next(Summaries.Length)]
        });

        return Ok(data);
    }
}
