using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtDemo.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    public AuthController(IConfiguration config) => _config = config;

    [HttpPost("login")]
    public IActionResult Login()
    {
        SymmetricSecurityKey signingKey;
        try
        {
            signingKey = JwtKeyHelper.BuildSigningKey(_config);
        }
        catch (Exception ex)
        {
            return Problem($"JWT key error: {ex.Message}");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "sandrine"),
            new Claim(ClaimTypes.Role, "admin")
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { token = tokenStr });
    }
}
