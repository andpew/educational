using AutoMapper;
using Educational.Core.BLL.Services.Abstract;
using Educational.Core.Common.DTO.User;
using Educational.Core.DAL;
using Microsoft.EntityFrameworkCore;

namespace Educational.Core.BLL.Services;

public sealed class UserService : IUserService
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;
    public UserService(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        return _mapper.Map<IEnumerable<UserDTO>>(await _db.Users.ToListAsync());
    }
}
