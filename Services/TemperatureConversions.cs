using System;

namespace WeatherApp.Services
{
    public static class TemperatureConversions
    {
        public static double ConvertCelsiusFahrenheit(double celcius)
        {
            return (celcius * 9 / 5) + 32;
        }

        public static double ConvertFahrenheitCelcius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }

        public static double ConvertCelciusKelvin(double celcius)
        {
            return celcius + 273.15;
        }

        public static double ConvertKelvinCelcius(double kelvin)
        {
            return kelvin - 273.15;
        }
    }

}