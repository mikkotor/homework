namespace Frontend.Models;

public class User
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
    }
}
