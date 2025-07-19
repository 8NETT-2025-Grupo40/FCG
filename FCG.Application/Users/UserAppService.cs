using FCG.Domain.Common;
using FCG.Domain.Users.Entities;
using FCG.Domain.Users.Enums;

namespace FCG.Application.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAppService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserResponse>> GetAll(CancellationToken cancellationToken)
        {
            return (await _unitOfWork.UserRepository.GetAllAsync(cancellationToken)).Select(u => new UserResponse()
            {
                Id = u.Id,
                Email = u.Email.ToString(),
                Role = u.Role.ToString(),
                CreateDate = u.DateCreated,
            });
        }

        public async Task<Guid> CreateUserAsync(CreateUserRequest request, UserRole role, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.UserRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            {
                throw new DomainException("E-mail already in use.");
            }

            User user = new(request.Name, request.Email, request.Password, role, BaseStatus.Active);

            await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return user.Id;
        }

        public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = await _unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                return null;
            }

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email.ToString(),
                Role = user.Role.ToString(),
                CreateDate = user.DateCreated
            };
        }
    }
}