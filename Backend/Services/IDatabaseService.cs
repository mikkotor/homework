using Backend.Models;

namespace Backend.Services
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Add a new user to the database and return a User object
        /// </summary>
        /// <param name="email">Email address of the user</param>
        /// <param name="passwordHash">Bcrypted password</param>
        /// <returns>New User object with database Id</returns>
        User InsertNewUser(string email, string passwordHash);

        /// <summary>
        /// Search for a specific user from database using their email address
        /// </summary>
        /// <param name="email">The email address</param>
        /// <returns>The User object found or null if no match found</returns>
        User GetUserWithEmail(string email);
    }
}
