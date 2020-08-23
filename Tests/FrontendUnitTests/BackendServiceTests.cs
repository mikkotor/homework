using System;
using Xunit;
using NSubstitute;
using Frontend.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Frontend.Configurations;
using Microsoft.Extensions.Options;

namespace FrontendUnitTests
{
    public class BackendServiceTests
    {
        private readonly ILogger<BackendService> _logger;
        private readonly IOptions<BackendServerConfig> _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackendService _service;

        public BackendServiceTests()
        {
            _logger = Substitute.For<ILogger<BackendService>>();
            _config = Substitute.For<IOptions<BackendServerConfig>>();
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _service = new BackendService(_logger, _config, _httpClientFactory);
        }

        [Fact]
        public void Test1()
        {
        }
    }
}
