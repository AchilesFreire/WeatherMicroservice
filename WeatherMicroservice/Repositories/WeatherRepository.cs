using Microsoft.EntityFrameworkCore;
using WeatherMicroservice.Infrastructure;
using WeatherMicroservice.Models;

namespace WeatherMicroservice.Repositories
{
    public class WeatherRepository: IWeatherRepository
    {
        private readonly WeatherDbContext _context;

        public WeatherRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task AddWeatherRecordAsync(WeatherRecord record)
        {
            _context.WeatherRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task<WeatherRecord?> GetWeatherRecordByIdAsync(int id)
        {
            return await _context.WeatherRecords.FindAsync(id);
        }

        public async Task<IEnumerable<WeatherRecord>> GetAllWeatherRecordsAsync()
        {
            return await _context.WeatherRecords.ToListAsync();
        }

    }
}
