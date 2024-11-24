using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature
{
    public class ExchangeRateCommandHandler(IExchangeRateRepository repository, IExternalExchangeRateProvider externalProvider, ILogger<ExchangeRateCommandHandler> logger) : IExchangeRateService
    {
        private readonly IExchangeRateRepository _repository = repository;
        private readonly IExternalExchangeRateProvider _externalProvider = externalProvider;
        private readonly ILogger<ExchangeRateCommandHandler> _logger = logger;

        public async Task<ExchangeRate> GetRateAsync(string baseCurrency, string quoteCurrency)
        {
            _logger.LogInformation("Fetching exchange rate for {BaseCurrency}/{QuoteCurrency} from the repository.", baseCurrency, quoteCurrency);
            return await _repository.GetRateAsync(baseCurrency, quoteCurrency);
        }

        public async Task<ExchangeRate> AddOrUpdateRateAsync(string baseCurrency, string quoteCurrency, decimal bid, decimal ask)
        {
            _logger.LogInformation("Adding or updating exchange rate for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);

            var rate = await _repository.GetRateAsync(baseCurrency, quoteCurrency);
            if (rate == null)
            {
                _logger.LogInformation("Exchange rate not found. Fetching from external provider.");
                var externalRate = await _externalProvider.FetchExchangeRateAsync(baseCurrency, quoteCurrency)
                    ?? throw new Exception($"Failed to fetch exchange rate for {baseCurrency}/{quoteCurrency}.");

                rate = new ExchangeRate(new CurrencyPair(baseCurrency, quoteCurrency), externalRate.Bid, externalRate.Ask);
                await _repository.AddRateAsync(rate);
                _logger.LogInformation("Added new exchange rate for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);
            }
            else
            {
                _logger.LogInformation("Exchange rate found. Updating values.");
                rate.UpdateRate(bid, ask);
                await _repository.UpdateRateAsync(rate);
                _logger.LogInformation("Updated exchange rate for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);
            }

            return rate;
        }
    }
}
