namespace WeatherAPI.Model
{
    public class AddWeatherRequest
    {
        public DateTime Date { get; set; }
        
        public string City { get; set; }

        public string Country { get; set; }

        public int TemperatureC { get;}

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; }
    }
}
