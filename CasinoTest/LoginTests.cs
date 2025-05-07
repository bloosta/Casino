using System;
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
    public class LoginTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly ApiClientService _service;

        public LoginTests()
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
        public async Task LoginAsync_ValidCredentials_ReturnsTrue()
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
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var result = await _service.LoginAsync("test", "password");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ReturnsFalse()
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
                    StatusCode = HttpStatusCode.Unauthorized
                });

            // Act
            var result = await _service.LoginAsync("test", "wrong");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task LoginAsync_ServerError_ReturnsFalse()
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
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act
            var result = await _service.LoginAsync("test", "password");

            // Assert
            Assert.False(result);
        }
    }
} 