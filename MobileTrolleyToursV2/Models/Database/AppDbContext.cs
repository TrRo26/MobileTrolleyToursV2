using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MobileTrolleyTours.Models.Users;

namespace MobileTrolleyTours.Models.Database;

//public class AppDbContext : DbContext
//{
//public DbSet<User> Users { get; set; }

//public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//	{

//	}
//}

public class AppDbContext : IdentityDbContext<User>
{
public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
{
}
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);
}
}