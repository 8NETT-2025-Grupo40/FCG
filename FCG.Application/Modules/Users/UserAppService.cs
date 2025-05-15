using FCG.Domain.Common;
using FCG.Domain.Modules.Users;

namespace FCG.Application.Modules.Users
{
    public class UserAppService : IUserAppService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAppService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserResponse>> GetAll(CancellationToken cancellationToken)
        {
            return (await this._unitOfWork.UserRepository.GetAllAsync(cancellationToken)).Select(u => new UserResponse()
            {
                Id = u.Id,
                Email = u.Email.ToString(),
                Role = u.Role.ToString(),
                CreateDate = u.CreateDate,
            });
        }

        public async Task<Guid> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            User user = new(request.Name, request.Email, request.Password, UserRole.User, BaseStatus.Active);

            await this._unitOfWork.UserRepository.AddAsync(user, cancellationToken);
            await this._unitOfWork.CommitAsync(cancellationToken);

            return user.Id;
        }

        public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = await this._unitOfWork.UserRepository.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                return null;
            }

            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email.ToString(),
                Role = user.Role.ToString(),
                CreateDate = user.CreateDate
            };
        }
    }
}