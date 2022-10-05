using Educational.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Educational.Core.DAL;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
}