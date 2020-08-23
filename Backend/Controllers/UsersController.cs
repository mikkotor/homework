using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IDatabaseService _database;

        public UsersController(ILogger<UsersController> logger, IDatabaseService database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public IActionResult Get(string email)
        {
            var user = _database.GetUserWithEmail(email);
            if (user != null)
                return new OkObjectResult(user);
            else
                return NoContent();
        }

        [HttpPost]
        public IActionResult Post([FromBody] User newUser)
        {
            newUser = _database.InsertNewUser(newUser.Email, newUser.PasswordHash);
            if (newUser != null)
                return new OkObjectResult($"New user '{newUser.Email}' registered.");
            else
                return new BadRequestObjectResult("Failed to register new user");
        }
    }
}
