using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Data;
using WeatherAPI.Model;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
         "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherAPIDbContext _context;

        public WeatherForecastController(WeatherAPIDbContext context, ILogger<WeatherForecastController> logger)
        {
            _context = context;
            _logger = logger;
            
        }


        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetAllWeather()
        {
            return Ok(await _context.WeatherForeCasts.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]

        public async Task<IActionResult> GetWeather([FromRoute] Guid id)
        {
            var weather = await _context.WeatherForeCasts.FindAsync(id);

            if (weather == null)
            {
                return NotFound();
            }

            return Ok(weather);
        }



        [HttpPost]
        public async Task<IActionResult>AddWeather(AddWeatherRequest addWeatherRequest)
        {
            var weather = new WeatherForecast()
            {
                WeatherId = Guid.NewGuid(),
                Date = DateTime.Now,
                City = addWeatherRequest.City,
                Country = addWeatherRequest.Country,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };

            await _context.WeatherForeCasts.AddAsync(weather);
            await _context.SaveChangesAsync();

            return Ok(weather);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWeather([FromRoute] Guid id, UpdateWeatherRequest updateWeatherRequest)
        {
           var weather = await _context.WeatherForeCasts.FindAsync(id);

            if (weather != null)
            {
              weather.City = updateWeatherRequest.City;
              weather.Country = updateWeatherRequest.Country;

                await _context .SaveChangesAsync();

                return Ok(weather);
            }

            return NotFound();

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var weather = await _context.WeatherForeCasts.FindAsync(id);

            if (weather != null)
            {
                _context.Remove(weather);
                await _context.SaveChangesAsync();

                return Ok(weather);
            }

            return NotFound();
        }
    }
}