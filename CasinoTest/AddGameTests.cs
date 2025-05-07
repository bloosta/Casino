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
    public class AddGameTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly ApiClientService _service;

        public AddGameTests()
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
        public async Task AddGameAsync_ValidData_Success()
        {
            // Arrange
            var playedAt = DateTime.Now;
            var type = "Poker";
            var clientIds = new List<int> { 1, 2, 3 };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => 
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString().Contains("api/games")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act & Assert
            await _service.AddGameAsync(playedAt, type, clientIds);
            // Если метод выполнился без исключения, значит тест пройден
        }

        [Fact]
        public async Task AddGameAsync_EmptyClientList_Success()
        {
            // Arrange
            var playedAt = DateTime.Now;
            var type = "Poker";
            var clientIds = new List<int>();

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => 
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString().Contains("api/games")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act & Assert
            await _service.AddGameAsync(playedAt, type, clientIds);
            // Если метод выполнился без исключения, значит тест пройден
        }

        [Fact]
        public async Task AddGameAsync_ServerError_ThrowsException()
        {
            // Arrange
            var playedAt = DateTime.Now;
            var type = "Poker";
            var clientIds = new List<int> { 1, 2, 3 };

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
                _service.AddGameAsync(playedAt, type, clientIds));
            Assert.Contains("500", exception.Message);
        }
    }
} 