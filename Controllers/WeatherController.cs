using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WeatherApp.Services;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        //Controller for current weather forecast.
        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] double latitude, [FromQuery] double longitude)
        {
            if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                return BadRequest("Invalid latitude or longitude values");
            }

            try
            {
                var weather = await _weatherService.GetWeatherAsync(latitude, longitude);
                return Ok(weather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Controller for convert temperature to different units.
        [HttpGet("convert")]
        public IActionResult ConvertTemperature([FromQuery] double temperature, [FromQuery] string fromUnit, [FromQuery] string toUnit)
        {
            double result;
            switch (fromUnit.ToLower())
            {
                case "celsius":
                    result = toUnit.ToLower() switch
                    {
                        "fahrenheit" => TemperatureConversions.ConvertCelsiusFahrenheit(temperature),
                        "kelvin" => TemperatureConversions.ConvertCelsiusKelvin(temperature),
                        _ => throw new ArgumentException("Invalid Unit"),
                    };
                    break;
                case "fahrenheit":
                    result = toUnit.ToLower() switch
                    {

                        "celsius" => TemperatureConversions.ConvertFahrenheitCelsius(temperature),
                        "kelvin" => TemperatureConversions.ConvertCelsiusKelvin(TemperatureConversions.ConvertFahrenheitCelsius(temperature)),
                        _ => throw new ArgumentException("Invalid Unit"),
                    };
                    break;
                case "kelvin":
                    result = toUnit.ToLower() switch
                    {
                        "celsius" => TemperatureConversions.ConvertKelvinCelsius(temperature),
                        "fahrenheit" => TemperatureConversions.ConvertCelsiusFahrenheit(TemperatureConversions.ConvertKelvinCelsius(temperature)),
                        _ => throw new ArgumentException("Invalid Unit"),
                    };
                    break;
                default:
                    return BadRequest("Invalid Original Unit");
            }

            return Ok(new { original = temperature, converted = result });
        }

        //Controller for calculating average temperature.
        [HttpGet("averageTemperature")]
        public async Task<IActionResult> GetAverageTemperature([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                var averageTemperature = await _weatherService.GetWeeklyAevrageTempAsync(latitude, longitude);
                return Ok(new { averageTemperature });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Controller for calculating highest and lowest temperature.
        [HttpGet("highLowTemp")]
        public async Task<ActionResult> GetHighestLowestTemperature([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                var (highestTemp, lowestTemp) = await _weatherService.GetHighestLowestTempAsync(latitude, longitude);
                return Ok(new { HighestTemp = highestTemp, LowestTemp = lowestTemp });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}