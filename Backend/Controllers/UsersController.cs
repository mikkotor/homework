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
        private readonly DatabaseService _database;

        public UsersController(ILogger<UsersController> logger, DatabaseService database)
        {
            _logger = logger;
            _database = database;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string email)
        {
            return new OkObjectResult(false);
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult AddNewUser([FromBody] User newUser)
        {
            newUser = _database.AddNewUser(newUser.Email, newUser.PasswordHash);
            if (newUser != null)
                return new OkObjectResult($"New user '{newUser.Email}' registered.");
            else
                return new BadRequestObjectResult($"Failed to register new user");
        }
    }
}
