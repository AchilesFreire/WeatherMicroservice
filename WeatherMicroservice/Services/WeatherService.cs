using WeatherMicroservice.Clients;
using WeatherMicroservice.Repositories;


namespace WeatherMicroservice.Services;


public class WeatherService: IWeatherService
{
    private readonly WeatherApiClient _apiClient;
    private readonly WeatherRepository _repository;
    private readonly IEnumerable<string> _defaultLocations ;


    public WeatherService(WeatherApiClient apiClient, WeatherRepository repository, IConfiguration config)
    {
        _apiClient = apiClient;
        _repository = repository;
        _defaultLocations = config.GetSection("DefaultLocations").Get<IEnumerable<string>>() ?? new List<string> { "New York", "London", "Tokyo" };
    }

    public async Task<IEnumerable<Models.WeatherRecord>> CaptureWeatherAsync() 
        => (await CaptureWeatherAsync(_defaultLocations));

    public async Task<Models.WeatherRecord> CaptureWeatherAsync(string location)
    {
        var response = await _apiClient.GetCurrentWeatherAsync(location);

        return response;
    }

    public async Task<IEnumerable<Models.WeatherRecord>> CaptureWeatherAsync ( IEnumerable<string> locations)
    {
        var results = new List<Models.WeatherRecord>();
        foreach ( var location in locations)
        {
            var response = await _apiClient.GetCurrentWeatherAsync(location);

            results.Add(response);

        }   

        return results;
    }
}