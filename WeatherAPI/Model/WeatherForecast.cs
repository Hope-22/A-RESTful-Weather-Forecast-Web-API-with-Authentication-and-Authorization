using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Model
{
    public class WeatherForecast
    {
        private Guid _id { get; set; }
        [Key]
        public Guid WeatherId { get; set; }

        public DateTime Date { get; set; }

        public string City { get; set; }

        public string Country   { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}