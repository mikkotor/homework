using Backend.Models;

namespace Backend.Services
{
    public interface IDatabaseService
    {
        User AddNewUser(string email, string passwordHash);
    }
}
