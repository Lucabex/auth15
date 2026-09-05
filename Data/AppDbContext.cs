using Microsoft.EntityFrameworkCore;
using auth15.Models;
namespace auth15.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
    {
        
    }
    public DbSet<User> User{get;set;}
}