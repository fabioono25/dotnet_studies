using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class AppDbContext : DbContext
    {
        // why this code? because we need to pass the options to the base class
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            // example of how to seed data
            // this.Database.EnsureCreated();
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}