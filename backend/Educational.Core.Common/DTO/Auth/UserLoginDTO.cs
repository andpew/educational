namespace Educational.Core.Common.DTO.Auth;

public sealed class UserLoginDTO
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
