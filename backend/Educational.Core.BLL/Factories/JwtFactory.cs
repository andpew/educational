using Educational.Core.Common.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Educational.Core.BLL.Factories;

public class JwtFactory
{
    private readonly JwtIssuerOptions _options;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public JwtFactory(JwtIssuerOptions options)
    {
        _options = options;
        _jwtSecurityTokenHandler = new();
    }

    public string GenerateToken(int id, string username, string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var claims = new[]
        {
            new Claim("SubId", id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken
        (
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_options.ValidFor),
            signingCredentials: credentials
        );

        return _jwtSecurityTokenHandler.WriteToken(token);
    }
}
