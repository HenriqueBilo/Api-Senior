using Api_Senior.Data;
using Api_Senior.Interfaces;
using Api_Senior.Models;
using Api_Senior.ViewModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Api_Senior.Services
{
    public class OpenWeatherService
    {
        private readonly ICidadeDataContext _dbContext;
        private readonly HttpClient _httpClient;

        private const string API_KEY = "fcfec5a86fd9f114db134fa465d3e4c6";
        private const string API_URL_WEATHER = "https://api.openweathermap.org/data/2.5/weather";

        public OpenWeatherService(ICidadeDataContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        public async virtual Task<List<Cidade>> GetTemperaturesAsync(string[] cidades, DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                var retorno = await _dbContext.Cidade.Where(t => cidades.Contains(t.Nome) && t.Data >= dataInicial && t.Data <= dataFinal).ToListAsync();
                return retorno;
            }
            catch (Exception ex)
            {
                return await Task.FromException<List<Cidade>>(ex);
            }
        }

        public async Task<List<Cidade>> GetWeatherDataAsync(List<string> listaCidadesBuscadas)
        {
            try
            {
                List<Cidade> resultados = new List<Cidade>();

                foreach (string nomeCidade in listaCidadesBuscadas)
                {
                    string urlDeChamada = $"{API_URL_WEATHER}?q={nomeCidade}&appid={API_KEY}&units=metric";

                    HttpResponseMessage? response = await _httpClient.GetAsync(urlDeChamada);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        object? resultado = JsonConvert.DeserializeObject<OpenWeatherResponse>(json);

                        DateTime dataAtual = DateTime.Now;

                        resultados.Add(new Cidade
                        {
                            IdCidade = nomeCidade + dataAtual.ToString(),
                            Nome = nomeCidade,
                            Temperatura = (double)((OpenWeatherResponse)resultado).Main.Temperature,
                            Data = dataAtual,
                            DataFormatada = dataAtual.ToString("dd/MM/yyyy HH:mm:ss")
                        });
                    }
                }

                await _dbContext.AddCidadesAsync(resultados);
                return resultados;
            }
            catch (Exception ex)
            {
                return await Task.FromException<List<Cidade>>(ex);
            }
        }
    }
}
