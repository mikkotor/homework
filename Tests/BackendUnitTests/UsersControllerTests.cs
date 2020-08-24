using Backend.Controllers;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace BackendUnitTests
{
    public class UsersControllerTests
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IDatabaseService _database;
        private readonly UsersController _usersController;

        public UsersControllerTests()
        {
            _logger = Substitute.For<ILogger<UsersController>>();
            _database = Substitute.For<IDatabaseService>();

            _usersController = new UsersController(_logger, _database);
        }

        [Fact]
        public void GivenUserIsNotAlreadyRegistered_WhenPostCalled_ThenOkObjectResultReturned()
        {
            var newUser = new User
            {
                Email = "some@email.com",
                PasswordHash = "foobar"
            };
            _database.InsertNewUser(newUser.Email, newUser.PasswordHash).Returns(newUser);

            var result = _usersController.Post(newUser);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GivenUserIsAlreadyRegistered_WhenPostCalled_ThenBadRequestObjectResultReturned()
        {
            var newUser = new User
            {
                Email = "some@email.com",
                PasswordHash = "foobar"
            };
            _database.InsertNewUser(newUser.Email, newUser.PasswordHash).Returns(null as User);

            var result = _usersController.Post(newUser);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
