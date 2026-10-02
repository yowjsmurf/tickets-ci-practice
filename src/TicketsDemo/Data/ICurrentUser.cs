namespace TicketsDemo.Data;

public enum UserRole
{
    Agent,
    Admin
}

public interface ICurrentUser
{
    UserRole Role { get; set; }
}

/// <summary>
/// Demo-only "login": the role is picked from a dropdown on the page, one per circuit.
/// </summary>
public sealed class DemoCurrentUser : ICurrentUser
{
    public UserRole Role { get; set; } = UserRole.Admin;
}
