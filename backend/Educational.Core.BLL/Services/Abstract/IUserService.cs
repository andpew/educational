using Educational.Core.Common.DTO.User;

namespace Educational.Core.BLL.Services.Abstract;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllUsers();
}
