using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly WeatherApi _weatherApi;
        private readonly ILogger<WeatherService> _logger;
        private readonly RestClient _client;

        public WeatherService(IOptions<WeatherApi> weatherApi, ILogger<WeatherService> logger)
        {
            _weatherApi = weatherApi.Value;
            _logger = logger;
            _client = new RestClient(_weatherApi.BaseUrl);
        }

        private async Task<T> GetApiResponseAsync<T>(string endpoint, double latitude, double longitude)
        {
            var request = new RestRequest(endpoint, Method.Get);
            request.AddParameter("lat", latitude);
            request.AddParameter("lon", longitude);
            request.AddParameter("appid", _weatherApi.ApiKey);

            _logger.LogInformation("Sending request to OpenWeatherMap API: {RequestUrl}", _client.BuildUri(request));

            var response = await _client.ExecuteAsync(request);

            if (response.ErrorException != null)
            {
                _logger.LogError("Error retrieving weather data: {Message}", response.ErrorException.Message);
                throw new ApplicationException("Error retrieving weather data", response.ErrorException);
            }

            if (!response.IsSuccessful)
            {
                _logger.LogError("Request failed with status code {StatusCode}: {Content}", response.StatusCode, response.Content);
                throw new ApplicationException($"Request failed with status code {response.StatusCode}: {response.Content}");
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude)
        {
            try
            {
                return await GetApiResponseAsync<WeatherResponse>("data/2.5/weather", latitude, longitude);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving weather data");
                throw;
            }
        }

        public async Task<double> GetWeeklyAevrageTempAsync(double latitude, double longitude)
        {
            try
            {
                var forecastResponse = await GetApiResponseAsync<ForecastResponse>("data/2.5/forecast", latitude, longitude);
                var dailyTemperatures = forecastResponse.List.Select(entry => entry.Main.Temp);
                var averageTemperature = dailyTemperatures.Average();

                _logger.LogInformation("Successfully calculated weekly average temperature");
                return averageTemperature;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating weekly average temperature");
                throw;
            }
        }

        public async Task<(double HighestTemp, double LowestTemp)> GetHighestLowestTempAsync(double latitude, double longitude)
        {
            try
            {
                var forecastResponse = await GetApiResponseAsync<ForecastResponse>("data/2.5/forecast", latitude, longitude);
                var temperatures = forecastResponse.List.Select(entry => entry.Main.Temp).ToArray();

                var highestTemp = temperatures.Max();
                var lowestTemp = temperatures.Min();

                _logger.LogInformation("Successfully calculated highest & lowest temperatures.");
                return (highestTemp, lowestTemp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating highest and lowest temperatures");
                throw;
            }
        }
    }
}
