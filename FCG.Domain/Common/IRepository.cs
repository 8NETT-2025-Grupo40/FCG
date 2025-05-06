namespace FCG.Domain.Common;

/// <summary>
/// Interface genérica para repositórios de entidades do tipo T.
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Marca uma nova entidade para ser adicionada
    /// </summary>
    Task AddAsync(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Marca que um entidade será editada.
    /// </summary>
    void Edit(T entidade, CancellationToken cancellationToken);

    /// <summary>
    /// Marca a remoção de uma entidade com base em seu ID
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Obtém uma entidade pelo seu ID
    /// </summary>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}