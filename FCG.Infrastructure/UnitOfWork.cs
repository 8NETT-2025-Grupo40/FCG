using FCG.Domain.Common;
using FCG.Domain.Modules.Users;

namespace FCG.Infrastructure;

public class UnitOfWork(ApplicationDbContext context, IUserRepository userRepository)
    : IUnitOfWork
{
    public IUserRepository UserRepository { get; } = userRepository;

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}