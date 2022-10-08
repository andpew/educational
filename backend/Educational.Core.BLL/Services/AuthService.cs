using AutoMapper;
using Educational.Core.BLL.Exceptions;
using Educational.Core.BLL.Security;
using Educational.Core.BLL.Services.Abstract;
using Educational.Core.Common.DTO.Auth;
using Educational.Core.Common.DTO.User;
using Educational.Core.DAL;
using Educational.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Educational.Core.BLL.Services;

public sealed class AuthService : IAuthService
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;
    public AuthService(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<UserDTO> Authorize(UserLoginDTO userDto)
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

        return _mapper.Map<UserDTO>(user);
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
