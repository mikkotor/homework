using Frontend.Configurations;
using Frontend.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class WeatherForecastService
    {
        private readonly ILogger<WeatherForecastService> _logger;
        private readonly BackendServerConfig _options;
        private readonly IHttpClientFactory _clientFactory;

        public WeatherForecastService(ILogger<WeatherForecastService> logger,
            IOptions<BackendServerConfig> options,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _options = options.Value;
            _clientFactory = clientFactory;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_options.BaseUrl);
            var response = await client.GetAsync("/weatherForecast", HttpCompletionOption.ResponseContentRead);
            var content = await response.Content.ReadAsByteArrayAsync();
            var forecast = JsonSerializer.Deserialize<WeatherForecast[]>(content);
            return forecast;
        }
    }
}
