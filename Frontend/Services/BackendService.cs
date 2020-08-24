using BCrypt;
using Frontend.Configurations;
using Frontend.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

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
                var urlEncoded = _options.UsersController + "?email=" + HttpUtility.UrlEncode(email);
                var response = await client.GetAsync(urlEncoded);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return false;
                }
                else
                {
                    throw new Exception($"Unhandled response from backend service: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong when calling backend service.");
                throw;
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
                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;
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
                throw;
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
