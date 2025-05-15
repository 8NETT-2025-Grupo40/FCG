namespace FCG.Application.Modules.Login
{
    public interface ILoginAppServices
    {
        Task<LoginResponse> LoginAppAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}