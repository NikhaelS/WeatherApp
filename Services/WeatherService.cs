using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherApi _weatherApi;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IOptions<WeatherApi> weatherApi, ILogger<WeatherService> logger)
        {
            _weatherApi = weatherApi.Value;
            _logger = logger;
        }

        public async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude)
        {
            var client = new RestClient(_weatherApi.BaseUrl);
            var request = new RestRequest("data/2.5/weather", Method.Get);
            request.AddParameter("lat", latitude);
            request.AddParameter("lon", longitude);
            request.AddParameter("appid", _weatherApi.ApiKey);

            _logger.LogInformation("Sending request to OpenWeatherMap API");
            _logger.LogInformation("Request URL: {0}", client.BuildUri(request));
            _logger.LogInformation("API Key: {0}", _weatherApi.ApiKey);

            var response = await client.ExecuteAsync<WeatherResponse>(request);
            if (response.ErrorException != null)
            {
                _logger.LogError("Error retrieving weather data: {Message}", response.ErrorException.Message);
                throw new ApplicationException("Error retrieving weather data", response.ErrorException);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogError("Unauthorized access - check your API key");
                throw new ApplicationException("Unauthorized access - check your API key");
            }

            if (!response.IsSuccessful)
            {
                _logger.LogError("Request failed with status code {0}: {1}", response.StatusCode, response.Content);
                throw new ApplicationException($"Request failed with status code {response.StatusCode}: {response.Content}");
            }

            _logger.LogInformation("Successfully retrieved weather data");

            return response.Data;

        }
    }

}