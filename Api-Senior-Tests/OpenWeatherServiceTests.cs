using Xunit;
using Moq;
using Api_Senior.Services;
using Api_Senior.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Api_Senior.Models;
using RichardSzalay.MockHttp;
using Microsoft.EntityFrameworkCore;
using Moq.Protected;
using Moq.Language;
using Moq.Language.Flow;
using Api_Senior.Interfaces;
using System;
using System.Linq;

namespace Api_Senior_Tests
{
    public class OpenWeatherServiceTests
    {
        private readonly OpenWeatherService _openWeatherService;
        private readonly Mock<ICidadeDataContext> _cidadeDataContextMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;

        public OpenWeatherServiceTests()
        {
            _cidadeDataContextMock = new Mock<ICidadeDataContext>(); //new DbContextOptionsBuilder<CidadeDataContext>().UseInMemoryDatabase("teste-db-cidade").Options
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _openWeatherService = new OpenWeatherService(_cidadeDataContextMock.Object, _httpClient);
        }

        #region Métodos de sucesso
        /// <summary>
        /// Teste responsável por testar o método GetWeatherDataAsync com sucesso 
        /// </summary>
        /// <returns>Uma lista de objetos (Cidades)</returns>
        [Fact]
        public async Task GetWeatherDataAsyncComSucesso()
        {
            //Arrange

            var listaCidadesBuscadas = new List<string> { "São Paulo", "Rio de Janeiro", "Porto Alegre", "Curitiba" };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var content = new StringContent("{\"main\":{\"temp\":22}}");
            responseMessage.Content = content;

            _httpMessageHandlerMock.Protected()
                                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                                    .ReturnsAsync(responseMessage)
                                    .Verifiable();


            // Configurando o comportamento do método AddCidadesAsync
            _cidadeDataContextMock
                .Setup(x => x.AddCidadesAsync(It.IsAny<List<Cidade>>())).Returns(Task.CompletedTask).Verifiable();

            //Act
            var resultadoMetodo = await _openWeatherService.GetWeatherDataAsync(listaCidadesBuscadas);

            //Assert
            Assert.NotNull(resultadoMetodo);
            Assert.IsType<List<Cidade>>(resultadoMetodo);
            Assert.Equal(4, resultadoMetodo.Count);
            Assert.Equal("São Paulo", resultadoMetodo[0].Nome);
            Assert.Equal("Rio de Janeiro", resultadoMetodo[1].Nome);
            Assert.Equal("Porto Alegre", resultadoMetodo[2].Nome);
            Assert.Equal("Curitiba", resultadoMetodo[3].Nome);
            Assert.Equal(22, resultadoMetodo[0].Temperatura);
            Assert.Equal(22, resultadoMetodo[1].Temperatura);
        }

        /// <summary>
        /// Teste de sucesso para o método GetTemperaturesAsync
        /// </summary>
        /// <returns>Retorna os dados das cidades informadas e do range de data informado</returns>
        [Fact]
        public async Task GetTemperaturesAsyncComSucesso()
        {
            // Arrange
            var dataInicial = DateTime.Now;
            var dataFinal = dataInicial.AddDays(1);
            var cidadesBuscadas = new string[] { "São Paulo", "Rio de Janeiro", "Porto Alegre", "Curitiba" };
            var expectedResults = new List<Cidade>
            {
                new Cidade { IdCidade = "São Paulo" + dataInicial.ToString(), Nome = "São Paulo", Temperatura = 22, Data = dataInicial, DataFormatada = dataInicial.ToString() },
                new Cidade { IdCidade = "Rio de Janeiro" + dataInicial.ToString(), Nome = "Rio de Janeiro", Temperatura = 37, Data = dataInicial, DataFormatada = dataInicial.ToString() },
                new Cidade { IdCidade = "Porto Alegre" + dataInicial.ToString(), Nome = "Porto Alegre", Temperatura = 15, Data = dataInicial, DataFormatada = dataInicial.ToString() },
                new Cidade { IdCidade = "Curitiba" + dataInicial.ToString(), Nome = "Curitiba", Temperatura = 18, Data = dataFinal, DataFormatada = dataFinal.ToString() }
            };

            //A ideia aqui era popular o banco de dados em memória, porém tive dificulade para conseguir fazer funcionar

            var mockDbFactory = new Mock<IDbContextFactory<CidadeDataContext>>();
            var options = new DbContextOptionsBuilder<CidadeDataContext>().UseInMemoryDatabase("teste-db-cidade").Options;

            using (var context = new CidadeDataContext(options))
            {
                context.Cidade.AddRange(expectedResults);
                context.SaveChanges();
            }

            mockDbFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(() => new CidadeDataContext(options));

            // Act

            //Aqui que foi o problema, não soube como passar esse banco para o serviço, de forma que conseguisse mockar ele para retornar os resultados que eu queria
            //var openWeatherService = new OpenWeatherService(mockDbFactory.Object, _httpClient);

            var result = await _openWeatherService.GetTemperaturesAsync(cidadesBuscadas, dataInicial, dataFinal);

            // Assert
            Assert.Equal(expectedResults, result);
        }

        #endregion

        #region Métodos de falha
        /// <summary>
        /// Teste responsável por testar o método GetWeatherDataAsync com falha
        /// </summary>
        /// <returns>Uma falha</returns>
        [Fact]
        public async Task GetWeatherDataAsyncComFalha()
        {
            //Arrange

            var listaCidadesBuscadas = new List<string> { "São Paulo", "Rio de Janeiro", "Porto Alegre", "Curitiba" };

            _httpMessageHandlerMock.Protected()
                                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                                    .ThrowsAsync(new Exception("Falha ao enviar a solicitação HTTP."));

            //Assert
            await Assert.ThrowsAsync<Exception>(() => _openWeatherService.GetWeatherDataAsync(listaCidadesBuscadas));
        }
        #endregion

    }
}