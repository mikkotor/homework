using Frontend.Configurations;
using Frontend.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace FrontendUnitTests;

public class BackendServiceTests
{
    private readonly ILogger<BackendService> _logger;
    private readonly IOptions<BackendServerConfig> _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly MockHttpMessageHandler _mockHttpMessageHandler;
    private readonly IBackendService _service;

    public BackendServiceTests()
    {
        _logger = Substitute.For<ILogger<BackendService>>();
        _config = Substitute.For<IOptions<BackendServerConfig>>();
        _config.Value.Returns(new BackendServerConfig
        {
            BaseUrl = "http://localhost",
            UsersController = "api/users"
        });
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
        _mockHttpMessageHandler = new MockHttpMessageHandler();
        _httpClientFactory.CreateClient().Returns(new HttpClient(_mockHttpMessageHandler));
        _service = new BackendService(_logger, _config, _httpClientFactory);
    }

    [Fact]
    public async Task GivenEmailIsAlreadyUsed_WhenEmailQueried_ThenTrueIsReturned()
    {
        var urlEncoded = HttpUtility.UrlEncode("test@email.com");
        _mockHttpMessageHandler.AddNewMockResponse(urlEncoded, HttpStatusCode.OK);

        var result = await _service.IsEmailUsed("test@email.com");

        Assert.True(result);
    }

    [Fact]
    public async Task GivenEmailIsNotUsed_WhenEmailQueried_ThenFalseIsReturned()
    {
        var urlEncoded = HttpUtility.UrlEncode("test@email.com");
        _mockHttpMessageHandler.AddNewMockResponse(urlEncoded, HttpStatusCode.NoContent);

        var result = await _service.IsEmailUsed("test@email.com");

        Assert.False(result);
    }

    [Fact]
    public async Task GivenBackendServiceIsBroken_WhenEmailQueried_ThenExceptionThrown()
    {
        _mockHttpMessageHandler.AddNewMockResponse("test@email.com", HttpStatusCode.InternalServerError);

        await Assert.ThrowsAnyAsync<Exception>(() => _service.IsEmailUsed("test@email.com"));
    }

    [Fact]
    public async Task GivenEmailIsNotUsed_WhenNewAccountRegistered_ThenSuccessReturned()
    {
        _mockHttpMessageHandler.AddNewMockResponse("api/users", HttpStatusCode.OK, "Account created");

        var result = await _service.RegisterNewAccount("test@email.com", "whatevs");

        Assert.Equal((true, "Account created"), result);
    }

    [Fact]
    public async Task GivenEmailIsUsed_WhenNewAccountRegistered_ThenFailureReturned()
    {
        _mockHttpMessageHandler.AddNewMockResponse("api/users", HttpStatusCode.BadRequest, "Failed to register new user");

        var result = await _service.RegisterNewAccount("test@email.com", "whatevs");

        Assert.Equal((false, "Failed to register new user"), result);
    }

    [Fact]
    public async Task GivenBackendServiceIsBroken_WhenNewAccountRegistered_ThenExceptionThrown()
    {
        await Assert.ThrowsAnyAsync<Exception>(() => _service.RegisterNewAccount("test@email.com", "whatevs"));
    }
}
