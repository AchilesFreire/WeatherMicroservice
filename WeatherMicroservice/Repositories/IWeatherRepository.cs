using WeatherMicroservice.Models;

namespace WeatherMicroservice.Repositories
{
    public interface IWeatherRepository
    {
        Task AddWeatherRecordAsync(WeatherRecord record);
        Task<WeatherRecord?> GetWeatherRecordByIdAsync(int id);
        Task<IEnumerable<WeatherRecord>> GetAllWeatherRecordsAsync();
    }
}