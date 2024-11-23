
using System;
using System.Threading.Tasks;
using ExchangeRates.Application.Interfaces;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Interfaces;

namespace ExchangeRates.Application.Services
{
    public class ExchangeRateService(IExchangeRateRepository repository, IExternalExchangeRateProvider externalProvider) : IExchangeRateService
    {
        private readonly IExchangeRateRepository _repository = repository;
        private readonly IExternalExchangeRateProvider _externalProvider = externalProvider;

        public async Task<ExchangeRate> GetRateAsync(string baseCurrency, string quoteCurrency)
        {
            return await _repository.GetRateAsync(baseCurrency, quoteCurrency);
        }

        public async Task<ExchangeRate> AddOrUpdateRateAsync(string baseCurrency, string quoteCurrency, decimal bid, decimal ask)
        {
            // Check if the rate already exists in the database
            var rate = await _repository.GetRateAsync(baseCurrency, quoteCurrency);

            if (rate == null)
            {
                // If not found, fetch from the external provider
                var externalRate = await _externalProvider.FetchExchangeRateAsync(baseCurrency, quoteCurrency)
                    ?? throw new Exception($"Failed to fetch exchange rate for {baseCurrency}/{quoteCurrency}.");

                // Create a new rate using the external values
                rate = new ExchangeRate(new CurrencyPair(baseCurrency, quoteCurrency), externalRate.Bid, externalRate.Ask);

                // Persist the new rate to the database
                await _repository.AddRateAsync(rate);
            }
            else
            {
                // If found, update the rate with the provided values
                rate.UpdateRate(bid, ask);

                // Persist the updated rate to the database
                await _repository.UpdateRateAsync(rate);
            }

            return rate;
        }
    }
}
