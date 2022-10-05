using Educational.Core.DAL.Entities.Abstract;

namespace Educational.Core.DAL.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; } = null!;
}
