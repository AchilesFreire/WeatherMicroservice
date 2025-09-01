using WeatherMicroservice.Models;

namespace WeatherMicroservice.Services
{
    public interface IWeatherService
    {

        /// <summary>
        /// Obtém as condições climáticas atuais para a lista de locais fornecida.
        /// </summary>
        /// <returns>Lista de registros climáticos de todas as sedes da CI&T.</returns>
        Task<IEnumerable<Models.WeatherRecord>> CaptureWeatherAsync();

        /// <summary>
        /// Obtém as condições climáticas atuais para a lista de locais fornecida.
        /// </summary>
        /// <param name="location"cidade/localidade a consultar.</param>
        /// <returns>Lista de registros climáticos do tipo WeatherRecord.</returns>
        Task<WeatherRecord> CaptureWeatherAsync(string location);


        /// <summary>
        /// Obtém as condições climáticas atuais para a lista de locais fornecida.
        /// </summary>
        /// <param name="locations">Lista de cidades/localidades a consultar.</param>
        /// <returns>Lista de registros climáticos do tipo WeatherRecord.</returns>
        Task<IEnumerable<WeatherRecord>> CaptureWeatherAsync(IEnumerable<string> location);

    }
}
