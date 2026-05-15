using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

class Program
{
    static void Main()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKey123!@#_MakeItLongEnough32Chars!!"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "admin@skybooker.com"),
            new Claim(ClaimTypes.Role, "ADMIN"),
            new Claim("role", "ADMIN")
        };

        var token = new JwtSecurityToken(
            issuer: "SkyBooker",
            audience: "SkyBooker",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        Console.WriteLine(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
