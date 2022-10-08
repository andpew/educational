using Educational.Core.BLL.Services.Abstract;
using Educational.Core.Common.DTO.Auth;
using Educational.Core.Common.DTO.User;
using Educational.Core.DAL;

namespace Educational.Core.BLL.Services;

public class AuthService : IAuthService
{
    private readonly DataContext _db;
    public AuthService(DataContext db)
    {
        _db = db;
    }

    public async Task<UserDTO?> Authorize(UserLoginDTO userDto)
    {
        return default;
    }

    public async Task<UserDTO?> Register(UserRegisterDTO userDto)
    {
        return default;
    }
}
