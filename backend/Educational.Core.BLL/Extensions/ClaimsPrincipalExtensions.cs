using Educational.Core.BLL.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Educational.Core.BLL.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserIdFromPrincipal(this ClaimsPrincipal principal)
    {
        if (int.TryParse(principal.FindFirstValue(JwtRegisteredClaimNames.Sub), out var result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }
    public static DateTime? GetExpirationDateFromPrincipal(this ClaimsPrincipal principal)
    {
        if (long.TryParse(principal.FindFirstValue(JwtRegisteredClaimNames.Exp), out var result))
        {
            return DateTimeHelper.FromUnixEpochDate(result);
        }
        else
        {
            return null;
        }
    }
    public static string? GetTokenIdFromPrincipal(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
    }
}
