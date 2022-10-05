using Educational.Core.BLL.Services.Abstract;
using Educational.Core.DAL;
using Educational.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Educational.Core.BLL.Services;

public class UserService : IUserService
{
    private readonly DataContext _db;
    public UserService(DataContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _db.Users.ToListAsync();
    }
}
