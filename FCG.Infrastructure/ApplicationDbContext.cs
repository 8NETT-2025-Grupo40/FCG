using FCG.Domain.Modules.Users;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<User> User { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
