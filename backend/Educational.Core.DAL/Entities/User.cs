using Educational.Core.DAL.Entities.Abstract;

namespace Educational.Core.DAL.Entities;

public sealed class User : BaseEntity
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = new byte[32];
    public byte[] PasswordSalt { get; set; } = new byte[32];
    public DateTime? VerifiedAt { get; set; }
}
