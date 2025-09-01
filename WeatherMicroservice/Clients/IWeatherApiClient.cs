using WeatherMicroservice.Models;

namespace WeatherMicroservice.Clients
{
    /// <summary>
    /// Defines a contract for accessing current weather data from an external Weather API.
    /// </summary>
    public interface IWeatherApiClient
    {
        /// <summary>
        /// Queries the WeatherAPI and returns the current weather conditions for the specified location.
        /// </summary>
        /// <param name="location">The name of the city or location to retrieve weather data for.</param>
        /// <returns>
        /// A <see cref="WeatherRecord"/> object containing the current weather conditions for the specified location.
        /// </returns>
        Task<WeatherRecord> GetCurrentWeatherAsync(string location);
    }
}
