using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtDemo.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "sandrine"),
            new Claim(ClaimTypes.Role, "admin")
        };

        var token = JwtTokenGenerator.GenerateToken(
            _config["Jwt:Key"]!,
            _config["Jwt:Issuer"]!,
            _config["Jwt:Audience"]!,
            claims,
            int.Parse(_config["Jwt:AccessTokenMinutes"]!)
        );

        return Ok(new { token });
    }
}
