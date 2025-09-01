using WeatherMicroservice.Models;

namespace WeatherMicroservice.Clients
{
    /// <summary>
    /// Provides functionality to query the external WeatherAPI service and retrieve current weather data for a specified location.
    /// </summary>
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherApiClient> _logger;
        private readonly string _apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to send requests to the WeatherAPI.</param>
        /// <param name="config">The configuration object containing WeatherAPI settings.</param>
        /// <param name="logger">The logger instance for logging operations and errors.</param>
        /// <exception cref="ArgumentNullException">Thrown if the API key is not configured.</exception>
        public WeatherApiClient(HttpClient httpClient, IConfiguration config, ILogger<WeatherApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = config["WeatherApi:ApiKey"] ?? throw new ArgumentNullException("WeatherApi:ApiKey not configured");
        }

        /// <summary>
        /// Retrieves the current weather conditions for the specified location from the WeatherAPI.
        /// </summary>
        /// <param name="location">The name of the city or location to query.</param>
        /// <returns>
        /// A <see cref="WeatherRecord"/> object containing the current weather data for the location.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the location is not found in the WeatherAPI.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the response from WeatherAPI is invalid.</exception>
        public async Task<WeatherRecord> GetCurrentWeatherAsync(string location)
        {
            var url = $"v1/current.json?key={_apiKey}&q={Uri.EscapeDataString(location)}&aqi=no";

            _logger.LogInformation("Requesting weather for {Location}...", location);

            var response = await _httpClient.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                _logger.LogWarning("Location {Location} not found in WeatherAPI.", location);
                throw new ArgumentException($"Location '{location}' not found.");
            }   

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<WeatherApiResponse>();
            if (data == null)
                throw new InvalidOperationException("Invalid response from WeatherAPI");

            return new WeatherRecord
            {
                Location = data.Location.Name,
                Condition = data.Current.Condition.Text,
                TemperatureC = data.Current.TempC,
                TemperatureF = data.Current.TempF,
                Humidity = data.Current.Humidity,
                WindKph = data.Current.WindKph,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Internal DTO for deserializing the WeatherAPI JSON response.
        /// </summary>
        private class WeatherApiResponse
        {
            public LocationInfo Location { get; set; } = new();
            public CurrentInfo Current { get; set; } = new();
        }

        /// <summary>
        /// Internal DTO representing location information from the WeatherAPI response.
        /// </summary>
        private class LocationInfo
        {
            public string Name { get; set; } = string.Empty;
        }

        /// <summary>
        /// Internal DTO representing current weather information from the WeatherAPI response.
        /// </summary>
        private class CurrentInfo
        {
            public double TempC { get; set; }
            public double TempF { get; set; }
            public double WindKph { get; set; }
            public double Humidity { get; set; }
            public ConditionInfo Condition { get; set; } = new();
        }

        /// <summary>
        /// Internal DTO representing weather condition details from the WeatherAPI response.
        /// </summary>
        private class ConditionInfo
        {
            public string Text { get; set; } = string.Empty;
        }
    }
}
