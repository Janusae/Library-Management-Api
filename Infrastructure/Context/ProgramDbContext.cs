using Microsoft.EntityFrameworkCore;
using Domain.Sql.Entity;

namespace Infrastructure.Context
{
    public class ProgramDbContext : DbContext
    {
        public ProgramDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
    }
}
