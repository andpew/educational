using AutoMapper;
using Educational.Core.BLL.Services.Abstract;
using Educational.Core.BLL.Services.Interfaces;
using Educational.Core.Common.DTO.User;
using Educational.Core.DAL;
using Microsoft.EntityFrameworkCore;

namespace Educational.Core.BLL.Services;

public sealed class UserService : BaseService, IUserService
{
    public UserService(DataContext db, IMapper mapper) : base(db, mapper) { }

    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        return _mapper.Map<IEnumerable<UserDTO>>(await _db.Users.ToListAsync());
    }
}
