using FCG.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Common;

public class EFRepository<T> : IRepository<T>
    where T : BaseEntity
{
    protected ApplicationDbContext Context;
    protected DbSet<T> DbSet;

    public EFRepository(ApplicationDbContext context)
    {
        this.Context = context;
        this.DbSet = this.Context.Set<T>();
    }

    public void Edit(T entidade, CancellationToken cancellationToken)
    {
        entidade.DateUpdated = DateTime.Now;
        this.Context.Set<T>().Update(entidade);
    }

    public async Task AddAsync(T entidade, CancellationToken cancellationToken)
    {
        entidade.DateCreated = DateTime.Now;
        await this.Context.Set<T>().AddAsync(entidade, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        T entity = await this.GetByIdAsync(id, cancellationToken) ?? throw EntityNotFoundException.For<T>();
        this.Context.Set<T>().Remove(entity);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.Context.Set<T>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}