using Api_Senior.Models;
using Api_Senior.Services;
using Hangfire;

namespace Api_Senior.Jobs
{
    public class OpenWeatherScheduler
    {
        private readonly ILogger<OpenWeatherScheduler> _logger;
        private readonly OpenWeatherService _weatherService;
        private readonly List<string> _cities;

        public OpenWeatherScheduler(OpenWeatherService weatherService, ILogger<OpenWeatherScheduler> logger)
        {
            _weatherService = weatherService;
            _cities = Configuration.Cidades;
            _logger = logger;
        }

        public async Task FetchTemperatures()
        {
            _logger.LogInformation("Iniciando busca de temperaturas...");
            List<Cidade> dadosCidadesBuscadas = await _weatherService.GetWeatherDataAsync(_cities);
            _logger.LogInformation("Finalizando busca de temperaturas...");
        }
    }
}
