using FCG.Domain.Users.Entities;
using FCG.Domain.Users.Repositories;
using FCG.Domain.Users.ValueObjects;
using FCG.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;


namespace FCG.Infrastructure.Repositories
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _appContext;

        public UserRepository(ApplicationDbContext appContext) : base(appContext)
        {
            _appContext = appContext;
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbSet
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await DbSet
                .FirstOrDefaultAsync(u => u.Email.Address == email, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken)
        {
            return await DbSet
                .AnyAsync(u => u.Email.Address == email.Address, cancellationToken);
        }
    }
}