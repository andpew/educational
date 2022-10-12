using Educational.Core.Common.DTO.Auth;

namespace Educational.Core.BLL.Services.Interfaces;

public interface IAuthService
{
    Task<AuthTokensDTO> Authorize(UserLoginDTO userDto);
    Task Register(UserRegisterDTO userDto);
    Task<AuthTokensDTO> Refresh(RefreshTokenDTO tokenDto);
    Task Revoke(RefreshTokenDTO tokenDto);
}
