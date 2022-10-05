using Educational.Core.DAL.Entities;

namespace Educational.Core.BLL.Services.Abstract;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsers();
}
