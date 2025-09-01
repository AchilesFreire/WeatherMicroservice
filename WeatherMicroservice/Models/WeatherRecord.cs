using System.Text.Json.Serialization;

namespace WeatherMicroservice.Models
{
    public class WeatherRecord
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public double TemperatureC { get; set; }
        public double TemperatureF { get; set; }
        public double Humidity { get; set; }
        public double WindKph { get; set; }
        public DateTime Timestamp { get; set; }

        // Azure Table Storage keys
        public string PartitionKey => Location.Replace(" ", "_").ToLowerInvariant();
        public string RowKey => Timestamp.ToString("yyyyMMddHHmmss");

        public string RainyDayAlert => Condition.ToLower().Contains("rain") ? $"Rain detected in {Location} at {Timestamp:yyyy-MM-dd HH:mm}" : string.Empty;
    }
}