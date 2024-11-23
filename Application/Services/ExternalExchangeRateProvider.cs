using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Models;
using ExchangeRates.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ExchangeRates.Infrastructure.Services
{
    public class ExternalExchangeRateProvider : IExternalExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ExternalExchangeRateProvider(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["AlphaVantage:ApiKey"]; // Get API key from configuration
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("External API Key is not configured.");
            }
        }

        public async Task<ExternalRate> FetchExchangeRateAsync(string baseCurrency, string quoteCurrency)
        {
            var url = $"query?function=CURRENCY_EXCHANGE_RATE&from_currency={baseCurrency}&to_currency={quoteCurrency}&apikey={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);

            try
            {
                var exchangeRateData = json.RootElement.GetProperty("Realtime Currency Exchange Rate");
                var bid = decimal.Parse(exchangeRateData.GetProperty("8. Bid Price").GetString() ?? "0");
                var ask = decimal.Parse(exchangeRateData.GetProperty("9. Ask Price").GetString() ?? "0");

                return new ExternalRate(bid, ask);
            }
            catch
            {
                return null;
            }
        }
    }
}
