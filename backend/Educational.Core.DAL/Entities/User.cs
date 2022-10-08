using Educational.Core.DAL.Entities.Abstract;

namespace Educational.Core.DAL.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = new byte[32];
    public byte[] PasswordSalt { get; set; } = new byte[32];
    public DateTime? VerifiedAt { get; set; }
}
