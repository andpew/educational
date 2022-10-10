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
        ThrowIfInvalidOptions();
        _jwtSecurityTokenHandler = new();
    }

    public string GenerateToken(int id, string username, string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var claims = new[]
        {
            new Claim("subId", id.ToString()),
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

    private void ThrowIfInvalidOptions()
    {
        if (_options is null)
        {
            throw new ArgumentNullException("Issuer options", nameof(_options));
        }
        else if (_options.ValidFor <= 0)
        {
            throw new ArgumentException("Issuer options: Must be a non-zero", nameof(JwtIssuerOptions.ValidFor));
        }
        else if (_options.Issuer == string.Empty)
        {
            throw new ArgumentNullException("Issuer options", nameof(_options.Issuer));
        }
        else if (_options.Audience == string.Empty)
        {
            throw new ArgumentNullException("Issuer options", nameof(_options.Audience));
        }
        else if (_options.Key == string.Empty)
        {
            throw new ArgumentNullException("Issuer options", nameof(_options.Key));
        }
        else if (_options.Key.Length < 16)
        {
            throw new ArgumentException("Issuer options: Must have more than 15 characters", nameof(JwtIssuerOptions.Key));
        }
    }
}
