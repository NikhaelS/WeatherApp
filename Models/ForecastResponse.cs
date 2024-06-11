using System;
using System.Collections.Generic;

namespace WeatherApp.Models
{
    public class ForecastResponse
    {
        public List<ForecastItem> List { get; set; }
    }

    public class ForecastItem
    {
        public ForecastMain Main { get; set; }
        public List<WeatherCondition> Weather { get; set; }
        public long Dt { get; set; }
        public string Dt_txt { get; set; }
    }

    //Had to add another class as code complained when trying to reuse Main class from WeatherResponse.cs
    public class ForecastMain
    {
        public double Temp { get; set; }
        public double Feels_like { get; set; }
        public double Temp_min { get; set; }
        public double Temp_max { get; set; }
        public int Pressure { get; set; }
        public int Sea_level { get; set; }
        public int Grnd_level { get; set; }
        public int Humidity { get; set; }
        public double Temp_kf { get; set; }
    }

    //Had to add another class as code complained when trying to reuse Main class from WeatherResponse.cs
    public class WeatherCondition
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
