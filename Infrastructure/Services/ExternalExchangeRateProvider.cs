using System.Text.Json;
using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class ExternalExchangeRateProvider : IExternalExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;
        private readonly ILogger<ExternalExchangeRateProvider> _logger;

        public ExternalExchangeRateProvider(HttpClient httpClient, IConfiguration configuration, ILogger<ExternalExchangeRateProvider> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["AlphaVantage:ApiKey"];
            _logger = logger;

            if (string.IsNullOrEmpty(_apiKey))
            {
                _logger.LogCritical("External API Key is not configured.");
                throw new InvalidOperationException("External API Key is not configured.");
            }
        }

        public async Task<ExternalRate?> FetchExchangeRateAsync(string baseCurrency, string quoteCurrency)
        {
            _logger.LogInformation("Fetching exchange rate from external provider for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);

            var url = $"query?function=CURRENCY_EXCHANGE_RATE&from_currency={baseCurrency}&to_currency={quoteCurrency}&apikey={_apiKey}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch exchange rate. Status Code: {StatusCode}", response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);

                var exchangeRateData = json.RootElement.GetProperty("Realtime Currency Exchange Rate");
                var bid = decimal.Parse(exchangeRateData.GetProperty("8. Bid Price").GetString() ?? "0");
                var ask = decimal.Parse(exchangeRateData.GetProperty("9. Ask Price").GetString() ?? "0");

                _logger.LogInformation("Successfully fetched exchange rate: Bid={Bid}, Ask={Ask}.", bid, ask);
                return new ExternalRate(bid, ask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching exchange rate for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);
                return null;
            }
        }
    }
}
