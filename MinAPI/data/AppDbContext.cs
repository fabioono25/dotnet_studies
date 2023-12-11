using Microsoft.EntityFrameworkCore;
using MinAPI.Models;

namespace MinAPI.data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Command> Commands => Set<Command>();
    }
}
