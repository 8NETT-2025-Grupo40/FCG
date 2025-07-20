namespace FCG.Application.Users
{
    public class CreateUserRequest
    {
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
