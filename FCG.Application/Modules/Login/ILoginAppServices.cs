namespace FCG.Application.Modules.Login
{
    public interface ILoginAppServices
    {
        Task<LoginAppResultDTO> LoginAppAsync(LoginRequestDto requestDto, CancellationToken cancellationToken);
    }
}