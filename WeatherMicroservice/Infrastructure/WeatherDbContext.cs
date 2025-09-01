using Microsoft.EntityFrameworkCore;
using WeatherMicroservice.Models;

namespace WeatherMicroservice.Infrastructure
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherRecord> WeatherRecords { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Location).IsRequired();
                entity.Property(e => e.Condition).IsRequired();
                entity.Property(e => e.TemperatureC);
                entity.Property(e => e.TemperatureF);
                entity.Property(e => e.Humidity);
                entity.Property(e => e.WindKph);
                entity.Property(e => e.Timestamp);
            });

        }
    }
}