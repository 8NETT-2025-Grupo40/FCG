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
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasColumnType("varchar(256)");
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired().HasColumnType("varchar(100)");
            modelBuilder.Entity<User>().Property(u => u.Role).IsRequired().HasColumnType("int");
            modelBuilder.Entity<User>().Property(u => u.DateCreated).IsRequired().HasColumnType("DateTime").HasDefaultValue("GETDATE()");
            modelBuilder.Entity<User>().Property(u => u.DateUpdated).HasColumnType("DateTime");
            modelBuilder.Entity<User>().Property(u => u.Status).IsRequired().HasColumnType("char(1)");
        }

    }
}
