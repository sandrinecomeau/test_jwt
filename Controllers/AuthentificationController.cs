using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtDemo.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IAuthService _auth;

    public AuthController(IConfiguration config, IAuthService auth)
    {
        _config = config;
        _auth = auth;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.Username))
            return BadRequest(new { error = "username/password requis" });

        if (!_auth.ValidateUser(dto.Username, out var user))
            return Unauthorized(new { error = "Identifiants invalides" });
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user!.Username),
            new Claim(ClaimTypes.Role, user.Role)
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



