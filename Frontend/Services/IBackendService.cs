using System.Threading.Tasks;

namespace Frontend.Services
{
    public interface IBackendService
    {
        /// <summary>
        /// Check backend service if the specified email address has been used already
        /// </summary>
        /// <param name="email">The email address to check</param>
        /// <returns>True if email used, false if not</returns>
        Task<bool> IsEmailUsed(string email);

        /// <summary>
        /// Register a new account
        /// </summary>
        /// <param name="email">The email address of the new account</param>
        /// <param name="password">Bcrypted password of the new account</param>
        /// <returns>Tuple with 'success' as boolean and 'message' with any extra details</returns>
        Task<(bool success, string message)> RegisterNewAccount(string email, string password);
    }
}
