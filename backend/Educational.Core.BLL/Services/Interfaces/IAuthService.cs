using Educational.Core.Common.DTO.Auth;
using Educational.Core.Common.DTO.User;

namespace Educational.Core.BLL.Services.Interfaces;

public interface IAuthService
{
    Task<UserDTO> Authorize(UserLoginDTO userDto);
    Task<UserDTO> Register(UserRegisterDTO userDto);
}
