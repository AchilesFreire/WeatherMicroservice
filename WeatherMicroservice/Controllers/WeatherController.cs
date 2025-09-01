using Microsoft.AspNetCore.Mvc;
using System.Collections;
using WeatherMicroservice.Models;
using WeatherMicroservice.Services;

namespace WeatherMicroservice.Controllers
{
    /// <summary>
    /// Controller responsible for providing endpoints to retrieve current weather information.
    /// Supports queries for predefined locations, a specific location, or a list of locations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherService _weatherService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherController"/> class.
        /// </summary>
        /// <param name="service">The weather service used to obtain weather data.</param>
        /// <param name="logger">The logger instance for logging operations and errors.</param>
        public WeatherController(
            IWeatherService service,
            ILogger<WeatherController> logger)
        {
            _weatherService = service;
            _logger = logger;
        }

        /// <summary>
        /// Gets the current weather for all predefined locations.
        /// </summary>
        /// <returns>
        /// A list of <see cref="WeatherRecord"/> objects containing weather results for each predefined location.
        /// </returns>
        [HttpGet("GetWeather", Name = "GetWeather")]
        public async Task<IEnumerable<WeatherRecord>> GetWeather()
        {
            return await _weatherService.CaptureWeatherAsync();
        }

        /// <summary>
        /// Gets the current weather for a specific location.
        /// </summary>
        /// <param name="location">The name of the city or location to query.</param>
        /// <returns>
        /// A <see cref="WeatherRecord"/> object containing the weather result for the specified location.
        /// </returns>
        [HttpGet("GetWeather/{location}", Name = "GetWeatherLocation")]
        public async Task<WeatherRecord> GetWeatherLocation(string location)
        {
            return await _weatherService.CaptureWeatherAsync(location);
        }

        /// <summary>
        /// Gets the current weather for a list of specified locations.
        /// </summary>
        /// <param name="locations">A list of city or location names to query.</param>
        /// <returns>
        /// A list of <see cref="WeatherRecord"/> objects containing weather results for each specified location.
        /// </returns>
        [HttpGet("GetWeatherLocations", Name = "GetWeatherLocations")]
        public async Task<IEnumerable<WeatherRecord>> GetWeatherLocations([FromBody] IEnumerable<string> locations)
        {
            return await _weatherService.CaptureWeatherAsync(locations);
        }
    }
}
