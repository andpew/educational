using System.IdentityModel.Tokens.Jwt;

namespace Educational.Core.BLL.Extensions;

public static class JwtSecurityTokenExtensions
{
    private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    static JwtSecurityTokenExtensions()
    {
        _jwtSecurityTokenHandler = new();
    }

    public static string Stringify(this JwtSecurityToken token)
    {
        return _jwtSecurityTokenHandler.WriteToken(token);
    }
}
