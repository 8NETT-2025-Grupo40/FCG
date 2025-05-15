using FCG.Domain.Modules.Users;

namespace FCG.Domain.Common;

/// <summary>
/// Coordenador de repositórios, garantindo que todas as operações realizadas durante uma transação sejam concluídas com sucesso antes de serem persistidas,
/// ou revertidas caso ocorra algum erro.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Método responsável por persistir as alterações realizadas nos repositórios.
    /// </summary>
    Task<int> CommitAsync(CancellationToken cancellationToken);

    IUserRepository UserRepository { get; }
}