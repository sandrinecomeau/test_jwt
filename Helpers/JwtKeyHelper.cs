using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class JwtKeyHelper
{
    public static SymmetricSecurityKey BuildSigningKey(IConfiguration cfg)
    {
        var raw = (cfg["Jwt:Key"] ?? throw new InvalidOperationException("Missing Jwt:Key")).Trim();
        var keyBytes = Encoding.UTF8.GetBytes(raw); 
        if (keyBytes.Length < 16) 
            throw new InvalidOperationException("JWT key too short (<16 bytes).");
        return new SymmetricSecurityKey(keyBytes);  
    }
}
