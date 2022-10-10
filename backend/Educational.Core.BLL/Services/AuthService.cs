using AutoMapper;
using Educational.Core.BLL.Exceptions;
using Educational.Core.BLL.Factories;
using Educational.Core.BLL.Security;
using Educational.Core.BLL.Services.Abstract;
using Educational.Core.BLL.Services.Interfaces;
using Educational.Core.Common.DTO.Auth;
using Educational.Core.Common.DTO.User;
using Educational.Core.DAL;
using Educational.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Educational.Core.BLL.Services;

public sealed class AuthService : BaseService, IAuthService
{
    private readonly JwtFactory _jwtFactory;
    public AuthService(DataContext db, IMapper mapper, JwtFactory jwtFactory) : base(db, mapper)
    {
        _jwtFactory = jwtFactory;
    }

    public async Task<AuthTokenDTO> Authorize(UserLoginDTO userDto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);

        if (user is null)
        {
            throw new NotFoundException("User with given username is not found");
        }

        if (user.VerifiedAt is null)
        {
            throw new NotVerifiedException("User is not verified");
        }

        if (!SecurityHelper.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new InvalidPasswordException("Invalid password");
        }

        return new()
        {
            Token = _jwtFactory.GenerateToken(user.Id, user.Username, user.Email)
        };
    }

    public async Task<UserDTO> Register(UserRegisterDTO userDto)
    {
        SecurityHelper.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        User user = new()
        {
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        user.VerifiedAt = DateTime.UtcNow;

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return _mapper.Map<UserDTO>(user);
    }
}
