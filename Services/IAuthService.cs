
public interface IAuthService
{
    bool ValidateUser(string username, out UserRecord? user);
}
