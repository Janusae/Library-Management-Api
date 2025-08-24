using Microsoft.EntityFrameworkCore;
using Domain.Sql.Entity;

namespace Infrastructure.Context
{
    public class ProgramDbContext : DbContext
    {
        public ProgramDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id); 
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd(); 
            });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Book { get; set; }
    }
}
