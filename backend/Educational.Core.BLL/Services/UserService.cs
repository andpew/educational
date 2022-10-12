using AutoMapper;
using Educational.Core.BLL.Exceptions;
using Educational.Core.BLL.Extensions;
using Educational.Core.BLL.Services.Abstract;
using Educational.Core.BLL.Services.Interfaces;
using Educational.Core.Common.DTO.User;
using Educational.Core.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Educational.Core.BLL.Services;

public sealed class UserService : BaseService, IUserService
{
    private readonly IHttpContextAccessor _contextAccessor;
    public UserService(DataContext db, IMapper mapper, IHttpContextAccessor contextAccessor) : base(db, mapper)
    {
        _contextAccessor = contextAccessor;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        return _mapper.Map<IEnumerable<UserDTO>>(await _db.Users.ToListAsync());
    }

    public async Task<UserDTO> GetUserFromToken()
    {
        var validatedToken = _contextAccessor.HttpContext?.User;

        if (validatedToken is null)
        {
            throw new InvalidTokenException("access");
        }

        var userId = validatedToken.GetUserIdFromPrincipal();

        if (userId is null)
        {
            throw new InvalidTokenException("access");
        }

        var userEntity = await _db.Users.FindAsync(userId);

        if (userEntity is null)
        {
            throw new NotFoundException("User is not found.");
        }

        return _mapper.Map<UserDTO>(userEntity);
    }
}
