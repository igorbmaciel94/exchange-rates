using System.Threading.Tasks;
using Xunit;
using Moq;
using Application.Models;
using System;
using Application.Feature;
using Domain.Entities;
using Domain.Interfaces;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Tests
{
    public class ExchangeRateServiceTests
    {
        [Fact]
        public async Task AddOrUpdateRateAsync_AddsRate_WhenRateDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IExchangeRateRepository>();
            mockRepo.Setup(repo => repo.GetRateAsync("USD", "EUR")).ReturnsAsync((ExchangeRate)null);

            var mockExternalProvider = new Mock<IExternalExchangeRateProvider>();
            mockExternalProvider.Setup(provider => provider.FetchExchangeRateAsync("USD", "EUR"))
                .ReturnsAsync(new ExternalRate(1.1m, 1.2m));

            var mockLogger = new Mock<ILogger<ExchangeRateCommandHandler>>();

            var service = new ExchangeRateCommandHandler(mockRepo.Object, mockExternalProvider.Object, mockLogger.Object);

            // Act
            var result = await service.AddOrUpdateRateAsync("USD", "EUR", 1.1m, 1.2m);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("USD", result.Pair.BaseCurrency);
            Assert.Equal("EUR", result.Pair.QuoteCurrency);
            Assert.Equal(1.1m, result.Bid);
            Assert.Equal(1.2m, result.Ask);
            mockRepo.Verify(repo => repo.AddRateAsync(It.IsAny<ExchangeRate>()), Times.Once);
            mockLogger.Verify(logger => logger.LogInformation(
                "Added new exchange rate for {BaseCurrency}/{QuoteCurrency}.", "USD", "EUR"), Times.Once);
        }

        [Fact]
        public async Task AddOrUpdateRateAsync_UpdatesRate_WhenRateExists()
        {
            // Arrange
            var pair = new CurrencyPair("USD", "EUR");
            var existingRate = new ExchangeRate(pair, 1.0m, 1.1m);

            var mockRepo = new Mock<IExchangeRateRepository>();
            mockRepo.Setup(repo => repo.GetRateAsync("USD", "EUR")).ReturnsAsync(existingRate);

            var mockExternalProvider = new Mock<IExternalExchangeRateProvider>();
            var mockLogger = new Mock<ILogger<ExchangeRateCommandHandler>>();

            var service = new ExchangeRateCommandHandler(mockRepo.Object, mockExternalProvider.Object, mockLogger.Object);

            // Act
            var result = await service.AddOrUpdateRateAsync("USD", "EUR", 1.2m, 1.3m);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("USD", result.Pair.BaseCurrency);
            Assert.Equal("EUR", result.Pair.QuoteCurrency);
            Assert.Equal(1.2m, result.Bid);
            Assert.Equal(1.3m, result.Ask);
            mockRepo.Verify(repo => repo.UpdateRateAsync(It.IsAny<ExchangeRate>()), Times.Once);
            mockLogger.Verify(logger => logger.LogInformation(
                "Exchange rate found. Updating values."), Times.Once);
        }

        [Fact]
        public async Task AddOrUpdateRateAsync_ThrowsException_WhenExternalFetchFails()
        {
            // Arrange
            var mockRepo = new Mock<IExchangeRateRepository>();
            mockRepo.Setup(repo => repo.GetRateAsync("USD", "EUR")).ReturnsAsync((ExchangeRate)null);

            var mockExternalProvider = new Mock<IExternalExchangeRateProvider>();
            mockExternalProvider.Setup(provider => provider.FetchExchangeRateAsync("USD", "EUR"))
                .ReturnsAsync((ExternalRate)null); // Simulate fetch failure

            var mockLogger = new Mock<ILogger<ExchangeRateCommandHandler>>();

            var service = new ExchangeRateCommandHandler(mockRepo.Object, mockExternalProvider.Object, mockLogger.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                service.AddOrUpdateRateAsync("USD", "EUR", 1.1m, 1.2m));

            Assert.Equal("Failed to fetch exchange rate for USD/EUR.", exception.Message);
            mockRepo.Verify(repo => repo.AddRateAsync(It.IsAny<ExchangeRate>()), Times.Never);
            mockLogger.Verify(logger => logger.LogError(
                It.IsAny<Exception>(), "Error occurred while adding or updating exchange rate for {BaseCurrency}/{QuoteCurrency}.", "USD", "EUR"), Times.Once);
        }
    }
}
