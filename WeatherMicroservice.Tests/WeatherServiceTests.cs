using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using WeatherMicroservice.Clients;
using WeatherMicroservice.Models;
using WeatherMicroservice.Repositories;
using WeatherMicroservice.Services;

namespace WeatherMicroservice.Tests
{
    public class WeatherServiceTests
    {

        private IConfiguration? _configuration;
        private ILogger<WeatherApiClient>? _logger;
        private IWeatherApiClient? _apiClient;

        #region Configuration Methods
        internal void SetUp()
        {
            _configuration = GetConfiguration();
            _logger = GetLogger();
            _apiClient = GetWeatherApiClient(_configuration);
        }
        private ILogger<WeatherApiClient> GetLogger()
        {
            var loggerMock = new Mock<ILogger<WeatherApiClient>>();
            return loggerMock.Object;
        }
        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            // Optionally, you can validate that the required sections exist:
            if (configuration.GetSection("WeatherAPI") == null ||
                configuration.GetSection("Storage") == null ||
                configuration.GetSection("ConnectionStrings") == null)
            {
                throw new InvalidOperationException("Required configuration sections are missing.");
            }

            return configuration;
        }
        private IWeatherApiClient GetWeatherApiClient(IConfiguration configuration)
        {
            var weatherApiSection = configuration.GetSection("WeatherAPI");
            var baseUrl = weatherApiSection.GetValue<string>("BaseUrl");
            var apiKey = weatherApiSection.GetValue<string>("ApiKey");

            // If WeatherApiClient requires other dependencies, add them here.
            // For example, if it needs an HttpClient:
            var httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

            return new WeatherApiClient(httpClient, _configuration, _logger);
        }
        
        #endregion


        [Fact]
        public async Task CaptureWeatherAsync_ThrowsException_ForKnownLocation()
        {
            SetUp();

            // Arrange
            var unknownLocation = "London";

            var result = await _apiClient.GetCurrentWeatherAsync(unknownLocation);
            Assert.NotNull(result);

        }


        [Fact]
        public async Task CaptureWeatherAsync_ThrowsException_ForUnknownLocation()
        {
            SetUp();

            // Arrange
            var unknownLocation = "UnknownCity";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _apiClient.GetCurrentWeatherAsync(unknownLocation)
            );

            Assert.Contains(unknownLocation, exception.Message);

        }
    }
}