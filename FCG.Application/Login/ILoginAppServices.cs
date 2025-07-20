namespace FCG.Application.Login
{
    public interface ILoginAppServices
    {
        Task<LoginResponse> LoginAppAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}