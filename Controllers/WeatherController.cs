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

        [HttpGet("convert")]
        public IActionResult ConvertTemperature([FromQuery] double temperature, [FromQuery] string fromUnit, [FromQuery] string toUnit)
        {
            double result;
            switch (fromUnit.ToLower())
            {
                case "celcius":
                    result = toUnit.ToLower() switch
                    {
                        "fahrenheit" => TemperatureConversions.ConvertCelsiusFahrenheit(temperature),
                        "kelvin" => TemperatureConversions.ConvertCelciusKelvin(temperature),
                        _ => throw new ArgumentException("Invalid Unit"),
                    };
                    break;
                case "fahrenheit":
                    result = toUnit.ToLower() switch
                    {

                        "celcius" => TemperatureConversions.ConvertFahrenheitCelcius(temperature),
                        "kelvin" => TemperatureConversions.ConvertCelciusKelvin(TemperatureConversions.ConvertFahrenheitCelcius(temperature)),
                        _ => throw new ArgumentException("Invalid Unit"),
                    };
                    break;
                case "kelvin":
                    result = toUnit.ToLower() switch
                    {
                        "celcius" => TemperatureConversions.ConvertKelvinCelcius(temperature),
                        "fahrenheit" => TemperatureConversions.ConvertCelsiusFahrenheit(TemperatureConversions.ConvertKelvinCelcius(temperature)),
                        _ => throw new ArgumentException("Invalid Unit"),
                    };
                    break;
                default:
                    return BadRequest("Invalid Original Unit");
            }

            return Ok(new { original = temperature, converted = result });
        }
    }

}