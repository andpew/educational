namespace Educational.Core.Common.DTO.Auth;

public sealed class AuthTokensDTO
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
