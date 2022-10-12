namespace Educational.Core.Common.DTO.Auth;

public class AuthTokenDTO
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
