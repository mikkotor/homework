using Backend.Models;

namespace Backend.Services
{
    public interface IDatabaseService
    {
        User InsertNewUser(string email, string passwordHash);
        User GetUserWithEmail(string email);
    }
}
