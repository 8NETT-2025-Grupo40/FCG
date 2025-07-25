﻿using FCG.Domain.Users.Enums;

namespace FCG.Application.Users;

public interface IUserAppService
{
    Task<IEnumerable<UserResponse>> GetAll(CancellationToken cancellationToken);

    /// <summary>
    /// Método responsável por criar um usuário.
    /// </summary>
    Task<Guid> CreateUserAsync(CreateUserRequest request, UserRole role, CancellationToken cancellationToken);

    /// <summary>
    /// Método responsável por obter um usuário pelo seu ID.
    /// </summary>
    Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}