using FCG.Domain.Modules.Users;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure
{
    public class DbAppContext: DbContext
    {
        public DbAppContext(DbContextOptions<DbAppContext> options): base(options){}

        
        public DbSet<User> Users { get; set; }

    }
}
