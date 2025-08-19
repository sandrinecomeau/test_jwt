using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public static class JwtTokenGenerator
{
    public static string GenerateToken(
        string key,
        string issuer,
        string audience,
        IEnumerable<Claim> claims,
        int expiryMinutes = 5)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("JWT key is missing", nameof(key));

        var keyBytes = Encoding.UTF8.GetBytes(key.Trim());
        if (keyBytes.Length < 32)
            throw new ArgumentException("JWT key must be at least 32 bytes", nameof(key));

        var signingKey = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow.AddSeconds(-5),
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

