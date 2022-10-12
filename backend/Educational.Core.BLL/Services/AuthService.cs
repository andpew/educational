using AutoMapper;
using Educational.Core.BLL.Exceptions;
using Educational.Core.BLL.Extensions;
using Educational.Core.BLL.Factories;
using Educational.Core.BLL.Helpers;
using Educational.Core.BLL.Services.Abstract;
using Educational.Core.BLL.Services.Interfaces;
using Educational.Core.Common.DTO.Auth;
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
            throw new InvalidUsernameOrPasswordException();
        }

        if (user.VerifiedAt is null)
        {
            throw new UserNotVerifiedException();
        }

        if (!SecurityHelper.VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new InvalidUsernameOrPasswordException();
        }

        var accessToken = _jwtFactory.GenerateAccessToken(user.Id, user.Username, user.Email);
        var refreshToken = _jwtFactory.GenerateRefreshToken(accessToken, user.Id);

        await _db.RefreshTokens.AddAsync(refreshToken);
        await _db.SaveChangesAsync();

        return new()
        {
            AccessToken = accessToken.Stringify(),
            RefreshToken = refreshToken.Token
        };
    }

    public async Task Register(UserRegisterDTO userDto)
    {
        if (await _db.Users.AnyAsync(user => user.Username == userDto.Username || user.Email == userDto.Email))
        {
            throw new UserAlreadyExistsException();
        }

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
    }


    public async Task<AuthTokenDTO> Refresh(AuthTokenDTO tokenDto)
    {
        var validatedToken = _jwtFactory.GetPrincipalFromToken(tokenDto.AccessToken);

        if (validatedToken is null)
        {
            throw new InvalidTokenException(tokenDto.RefreshToken);
        }

        var userId = validatedToken.GetUserIdFromPrincipal();
        var tokenId = validatedToken.GetTokenIdFromPrincipal();

        if (userId is null || tokenId is null)
        {
            throw new InvalidTokenException(tokenDto.RefreshToken);
        }

        var userEntity = await _db.Users.FindAsync(userId);

        if (userEntity is null)
        {
            throw new InvalidTokenException(tokenDto.RefreshToken);
        }

        if (userEntity.VerifiedAt is null)
        {
            throw new InvalidTokenException(tokenDto.RefreshToken);
        }

        var storedRefreshToken = await _db.RefreshTokens
            .FirstOrDefaultAsync(token =>
                token.JwtId == tokenId
                && token.UserId == userId
                && token.Token == tokenDto.RefreshToken);

        if (storedRefreshToken is null)
        {
            throw new InvalidTokenException(tokenDto.RefreshToken);
        }

        if (storedRefreshToken.Invalidated || storedRefreshToken.Used)
        {
            throw new InvalidTokenException(tokenDto.RefreshToken);
        }

        if (DateTime.UtcNow > storedRefreshToken.ExpiresAt)
        {
            throw new ExpiredRefreshTokenException();
        }

        storedRefreshToken.Used = true;
        storedRefreshToken.UpdatedAt = DateTime.UtcNow;
        _db.RefreshTokens.Update(storedRefreshToken);
        await _db.SaveChangesAsync();

        var accessToken = _jwtFactory.GenerateAccessToken(userEntity.Id, userEntity.Username, userEntity.Email);
        var refreshToken = _jwtFactory.GenerateRefreshToken(accessToken, userEntity.Id);

        await _db.RefreshTokens.AddAsync(refreshToken);
        await _db.SaveChangesAsync();

        return new()
        {
            AccessToken = accessToken.Stringify(),
            RefreshToken = refreshToken.Token
        };
    }
}
