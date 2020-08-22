using System.Threading.Tasks;

namespace Frontend.Services
{
    interface IBackendService
    {
        Task<bool> IsEmailUsed(string email);
        Task<(bool success, string message)> RegisterNewAccount(string email, string password);
    }
}
