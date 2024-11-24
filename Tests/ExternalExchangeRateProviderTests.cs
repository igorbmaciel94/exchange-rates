using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests
{
    public class ExternalExchangeRateProviderTests
    {
        private static IConfiguration LoadConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<ExternalExchangeRateProviderTests>();
            return configurationBuilder.Build();
        }

        [Fact]
        public async Task FetchExchangeRateAsync_ReturnsRate_WhenApiReturnsValidResponse()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            var responseContent = JsonSerializer.Serialize(new Dictionary<string, object>
            {
                { "Realtime Currency Exchange Rate", new Dictionary<string, string>
                    {
                        { "8. Bid Price", "1.1" },
                        { "9. Ask Price", "1.2" }
                    }
                }
            });

            mockHttpMessageHandler.SetupResponse(HttpStatusCode.OK, responseContent);

            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://www.alphavantage.co/")
            };

            var configuration = LoadConfiguration();
            var mockLogger = new Mock<ILogger<ExternalExchangeRateProvider>>();

            var provider = new ExternalExchangeRateProvider(httpClient, configuration, mockLogger.Object);

            // Act
            var result = await provider.FetchExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1.1m, result.Bid);
            Assert.Equal(1.2m, result.Ask);
            mockLogger.Verify(logger => logger.LogInformation(
                "Successfully fetched exchange rate: Bid={Bid}, Ask={Ask}.", 1.1m, 1.2m), Times.Once);
        }

        [Fact]
        public async Task FetchExchangeRateAsync_ReturnsNull_WhenApiReturnsErrorResponse()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            mockHttpMessageHandler.SetupResponse(HttpStatusCode.BadRequest, string.Empty);

            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://www.alphavantage.co/")
            };

            var configuration = LoadConfiguration();
            var mockLogger = new Mock<ILogger<ExternalExchangeRateProvider>>();

            var provider = new ExternalExchangeRateProvider(httpClient, configuration, mockLogger.Object);

            // Act
            var result = await provider.FetchExchangeRateAsync("USD", "EUR");

            // Assert
            Assert.Null(result);
            mockLogger.Verify(logger => logger.LogWarning(
                "Failed to fetch exchange rate. Status Code: {StatusCode}", HttpStatusCode.BadRequest), Times.Once);
        }
    }

    // Helper class for mocking HttpMessageHandler
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private HttpStatusCode _statusCode;
        private string _responseContent;

        public void SetupResponse(HttpStatusCode statusCode, string responseContent)
        {
            _statusCode = statusCode;
            _responseContent = responseContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = new StringContent(_responseContent)
            };
            return Task.FromResult(response);
        }
    }
}
