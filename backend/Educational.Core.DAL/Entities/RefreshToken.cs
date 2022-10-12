using Educational.Core.DAL.Entities.Abstract;

namespace Educational.Core.DAL.Entities;

public sealed class RefreshToken : BaseEntity
{
    public string Token { get; set; } = null!;
    public string JwtId { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public bool Used { get; set; }
    public bool Invalidated { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
