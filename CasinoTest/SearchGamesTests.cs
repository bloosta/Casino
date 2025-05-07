using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CasinoFrameworkApp.Services;
using Moq;
using Moq.Protected;
using System.Threading;
using Xunit;

namespace CasinoTest
{
    public class SearchGamesTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly ApiClientService _service;

        public SearchGamesTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://test.com/")
            };
            var factory = new Mock<IHttpClientFactory>();
            factory.Setup(x => x.CreateClient("CasinoApi")).Returns(_httpClient);
            _service = new ApiClientService(factory.Object);
        }

        [Fact]
        public async Task SearchGamesAsync_WithAllFilters_ReturnsFilteredGames()
        {
            // Arrange
            var from = DateTime.Now.AddDays(-1);
            var to = DateTime.Now;
            var expectedGames = new List<GameDto>
            {
                new(1, from, "Poker", new List<int> { 1, 2 }),
                new(2, to, "Poker", new List<int> { 3, 4 })
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => 
                        req.RequestUri.ToString().Contains("typeFilter=Poker") &&
                        req.RequestUri.ToString().Contains("clientId=1")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(expectedGames))
                });

            // Act
            var result = await _service.SearchGamesAsync(from, to, "Poker", 1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Poker", result[0].Type);
            Assert.Equal("Poker", result[1].Type);
        }

        [Fact]
        public async Task SearchGamesAsync_NoFilters_ReturnsAllGames()
        {
            // Arrange
            var expectedGames = new List<GameDto>
            {
                new(1, DateTime.Now, "Poker", new List<int> { 1 }),
                new(2, DateTime.Now, "Blackjack", new List<int> { 2 }),
                new(3, DateTime.Now, "Roulette", new List<int> { 3 })
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => 
                        !req.RequestUri.ToString().Contains("typeFilter") &&
                        !req.RequestUri.ToString().Contains("clientId")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(expectedGames))
                });

            // Act
            var result = await _service.SearchGamesAsync(null, null, null, null);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, g => g.Type == "Poker");
            Assert.Contains(result, g => g.Type == "Blackjack");
            Assert.Contains(result, g => g.Type == "Roulette");
        }

        [Fact]
        public async Task SearchGamesAsync_ServerError_ThrowsException()
        {
            // Arrange
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(() => 
                _service.SearchGamesAsync(DateTime.Now, DateTime.Now, "Poker", 1));
            Assert.Contains("500", exception.Message);
        }
    }
} 