using BCrypt;
using Frontend.Configurations;
using Frontend.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class BackendService : IBackendService
    {
        private readonly ILogger<BackendService> _logger;
        private readonly BackendServerConfig _options;
        private readonly IHttpClientFactory _clientFactory;

        public BackendService(ILogger<BackendService> logger,
            IOptions<BackendServerConfig> options,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _options = options.Value;
            _clientFactory = clientFactory;
        }

        public async Task<bool> IsEmailUsed(string email)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(_options.BaseUrl);
                var response = await client.GetAsync(_options.UsersController + "?email=" + email);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong when calling backend service.");
                return false;
            }
        }

        public async Task<(bool success, string message)> RegisterNewAccount(string email, string password)
        {
            try
            {
                var passwordHash = GenerateEncryptedPassword(password);
                var newUser = new User(email, passwordHash);

                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(_options.BaseUrl);
                var requestContent = JsonContent.Create(newUser);

                var response = await client.PostAsync(_options.UsersController, requestContent);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return (true, content);
                }
                else
                {
                    _logger.LogWarning($"Backend service failed to register a new account. {content}");
                    return (false, content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong when calling backend service.");
                return (false, "Something went wrong when calling backend service.");
            }
        }

        private string GenerateEncryptedPassword(string password)
        {
            var salt = BCryptHelper.GenerateSalt();
            var passwordHash = BCryptHelper.HashPassword(password, salt);

            return passwordHash;
        }
    }
}
