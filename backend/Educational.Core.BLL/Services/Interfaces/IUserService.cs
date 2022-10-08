using Educational.Core.Common.DTO.User;

namespace Educational.Core.BLL.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllUsers();
}
