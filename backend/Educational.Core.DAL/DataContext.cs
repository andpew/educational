﻿using Educational.Core.DAL.Entities;
using Educational.Core.DAL.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Educational.Core.DAL;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }

    public DbSet<User> Users { get; set; } = null!;
}