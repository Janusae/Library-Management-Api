using Microsoft.EntityFrameworkCore;
using Domain.Sql.Entity;
using Domain.Sql.Entity.Book;

namespace Infrastructure.Context
{
    public class ProgramDbContext : DbContext
    {
        public ProgramDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Book { get; set; }
    }
}
