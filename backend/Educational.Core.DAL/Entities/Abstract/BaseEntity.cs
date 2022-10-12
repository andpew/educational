namespace Educational.Core.DAL.Entities.Abstract;

public class BaseEntity
{
    public BaseEntity()
    {
        CreatedAt = UpdatedAt = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; set; }
}
