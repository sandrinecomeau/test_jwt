using Microsoft.Extensions.Configuration;

public class AuthService : IAuthService
{
    private readonly List<UserRecord> _users;

    public AuthService(IConfiguration config)
    {
        _users = config.GetSection("Auth:Users").Get<List<UserRecord>>() ?? new();
    }

    public bool ValidateUser(string username, out UserRecord? user)
    {
        user = _users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
        return user is not null;
    }
}