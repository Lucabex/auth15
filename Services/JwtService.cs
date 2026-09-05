using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using auth15.Models;
using Microsoft.IdentityModel.Tokens;

namespace auth15.Services;

 public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;

    public JwtService(IConfiguration configuration){
        _configuration = configuration;
        var secretKey = _configuration["JwtSettings:SecretKey"];
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

    }

    public string GenerateCode(User user)
    {
        var claim = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name,(user.Name ?? "Unknown")),
            new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claim),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpirationInMinutes"])),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(_key,SecurityAlgorithms.HmacSha256)
            
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}