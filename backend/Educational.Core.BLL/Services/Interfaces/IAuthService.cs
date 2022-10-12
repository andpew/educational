using Educational.Core.Common.DTO.Auth;

namespace Educational.Core.BLL.Services.Interfaces;

public interface IAuthService
{
    Task<AuthTokenDTO> Authorize(UserLoginDTO userDto);
    Task Register(UserRegisterDTO userDto);
    Task<AuthTokenDTO> Refresh(AuthTokenDTO tokenDto);
}
