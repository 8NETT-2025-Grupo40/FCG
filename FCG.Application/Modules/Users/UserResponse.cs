namespace FCG.Application.Modules.Users;

public class UserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = null!;
    public string Role { get; init; } = null!;
    public DateTime CreateDate { get; init; }
}