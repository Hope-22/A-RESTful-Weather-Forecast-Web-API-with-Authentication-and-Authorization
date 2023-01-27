using Microsoft.EntityFrameworkCore;
using WeatherAPI.Model;

namespace WeatherAPI.Data
{
    public class WeatherAPIDbContext : DbContext
    {
        public WeatherAPIDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<WeatherForecast> WeatherForeCasts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
