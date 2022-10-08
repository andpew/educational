namespace Educational.Core.Common.DTO.User;

public sealed class UserDTO
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
