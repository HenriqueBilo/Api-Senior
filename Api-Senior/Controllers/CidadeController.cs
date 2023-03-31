using Api_Senior.Models;
using Api_Senior.Services;
using Api_Senior.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Api_Senior.Controllers
{
    [ApiController]
    public class CidadeController : ControllerBase
    {
        private readonly OpenWeatherService _openWeatherService;

        public CidadeController(OpenWeatherService openWeatherService)
        {
            _openWeatherService = openWeatherService;
        }

        [HttpGet("v1/temperatures")]
        public async Task<IActionResult> GetTemperature(
            [FromQuery] string cidades, 
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim,
            [FromServices] OpenWeatherService apiOpenWeatherService)
        {
            try
            {
                if (cidades.Length == 0)
                {
                    return BadRequest("Pelo menos uma cidade deve ser fornecida.");
                }

                if(dataInicio > dataFim)
                {
                    return BadRequest("A data inicial deve ser menor ou igual a data final.");
                }

                var vetCidades = cidades.Split(',');

                var resultadoFinal = await _openWeatherService.GetTemperaturesAsync(vetCidades, dataInicio, dataFim);

                return Ok(new ResultViewModel<List<Cidade>>(resultadoFinal));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<List<Cidade>>("01XE01 - Falha interna no servidor"));
            }
        }

        private DateTimeOffset ConvertToTimeStamp(DateTime data)
        {
            return new DateTimeOffset(data.Year, data.Month, data.Day, 0, 0, 0, TimeSpan.Zero);
        }
    }
}