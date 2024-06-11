using System;
using System.Collections.Generic;

namespace WeatherApp.Models
{
    public class WeatherResponse
    {
       public Main main { get; set; }
       public Wind wind { get; set; }
       public List<WeatherCondition> Weather { get; set; }

        public class Main
        {
            public double Temp { get; set; }
            public int Humidity { get; set; }
        }

        public class Wind
        {
            public double Speed { get; set; }
        }

        public class WeatherCondition
        {
            public string Description { get; set; }
        }
    }

}