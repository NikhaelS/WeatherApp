using System;

namespace WeatherApp.Services
{
    public static class TemperatureConversions
    {
        public static double ConvertCelsiusFahrenheit(double celsius)
        {
            return (celsius * 9 / 5) + 32;
        }

        public static double ConvertFahrenheitCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }

        public static double ConvertCelsiusKelvin(double celsius)
        {
            return celsius + 273.15;
        }

        public static double ConvertKelvinCelsius(double kelvin)
        {
            return kelvin - 273.15;
        }
    }

}