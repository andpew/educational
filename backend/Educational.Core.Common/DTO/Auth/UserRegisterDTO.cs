namespace Educational.Core.Common.DTO.Auth;

public class UserRegisterDTO
{
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
