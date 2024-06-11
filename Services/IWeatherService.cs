using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude);
        Task<double> GetWeeklyAevrageTempAsync(double latitude, double longitude);
        Task<(double HighestTemp, double LowestTemp)> GetHighestLowestTempAsync(double latitude, double longitude);
    }

}