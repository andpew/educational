namespace Educational.Core.Common.DTO.Auth;

public sealed class UserRegisterDTO
{
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
