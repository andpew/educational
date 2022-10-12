using Educational.Core.Common.Options;
using Educational.Core.DAL.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Educational.Core.BLL.Factories;

public sealed class JwtFactory
{
    private readonly JwtIssuerOptions _issuerOptions;
    private readonly JwtRefreshTokenOptions _refreshTokenOptions;
    private readonly TokenValidationParameters _validationParameters;

    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public JwtFactory(
        JwtIssuerOptions issuerOptions,
        JwtRefreshTokenOptions refreshTokenOptions,
        TokenValidationParameters validationParameters)
    {
        _issuerOptions = issuerOptions;
        _refreshTokenOptions = refreshTokenOptions;
        _validationParameters = validationParameters;
        ThrowIfInvalidOptions();
        _jwtSecurityTokenHandler = new();
    }

    public JwtSecurityToken GenerateAccessToken(int id, string username, string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_issuerOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, username),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken
        (
            issuer: _issuerOptions.Issuer,
            audience: _issuerOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(_issuerOptions.ValidFor),
            signingCredentials: credentials
        );

        return token;
    }

    public RefreshToken GenerateRefreshToken(JwtSecurityToken accessToken, int userId)
    {
        return new()
        {
            Token = Guid.NewGuid().ToString(),
            UserId = userId,
            JwtId = accessToken.Id,
            ExpiresAt = DateTime.UtcNow.Add(_refreshTokenOptions.ValidFor)
        };
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        try
        {
            var principal = _jwtSecurityTokenHandler.ValidateToken(token, _validationParameters, out var validatedToken);

            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return validatedToken is JwtSecurityToken jwtToken
            && jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase);
    }

    private void ThrowIfInvalidOptions()
    {
        if (_issuerOptions is null)
        {
            throw new ArgumentNullException("Jwt Issuer options", nameof(JwtIssuerOptions));
        }
        else if (_issuerOptions.ValidFor <= TimeSpan.FromMinutes(1))
        {
            throw new ArgumentException("Jwt Issuer options: Must be more than one minute", nameof(JwtIssuerOptions.ValidFor));
        }
        else if (_issuerOptions.Issuer == string.Empty)
        {
            throw new ArgumentNullException("Jwt Issuer options", nameof(JwtIssuerOptions.Issuer));
        }
        else if (_issuerOptions.Audience == string.Empty)
        {
            throw new ArgumentNullException("Jwt Issuer options", nameof(JwtIssuerOptions.Audience));
        }
        else if (_issuerOptions.Key == string.Empty)
        {
            throw new ArgumentNullException("Jwt Issuer options", nameof(JwtIssuerOptions.Key));
        }
        else if (_issuerOptions.Key.Length < 16)
        {
            throw new ArgumentException("Jwt Issuer options: Must have more than 15 characters", nameof(JwtIssuerOptions.Key));
        }

        if (_refreshTokenOptions is null)
        {
            throw new ArgumentNullException("Jwt Refresh token options", nameof(JwtRefreshTokenOptions));
        }
        else if (_refreshTokenOptions.ValidFor <= TimeSpan.FromHours(1))
        {
            throw new ArgumentException("Jwt Refresh token options: Must be more than one hour", nameof(JwtRefreshTokenOptions.ValidFor));
        }
    }
}
