using FCG.Domain.Modules.Users;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure
{
    public class DbAppContext: DbContext
    {
        public DbAppContext(DbContextOptions<DbAppContext> options): base(options){}

        
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
        }

    }
}
