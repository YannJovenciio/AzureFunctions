
using Microsoft.EntityFrameworkCore;
using AzureFunctions.Users;

namespace AzureFunctions.DbContexts{
public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
    : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
}
}