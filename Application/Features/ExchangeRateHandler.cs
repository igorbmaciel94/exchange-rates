using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Features
{
    public class ExchangeRateHandler(IExchangeRateRepository repository, IExternalExchangeRateProvider externalProvider, ILogger<ExchangeRateHandler> logger) : IExchangeRateService
    {
        public async Task<ExchangeRate> GetRateAsync(string baseCurrency, string quoteCurrency)
        {
            logger.LogInformation("Fetching exchange rate for {BaseCurrency}/{QuoteCurrency} from the repository.", baseCurrency, quoteCurrency);
            var rate = await repository.GetRateAsync(baseCurrency, quoteCurrency);

            if (rate != null) return rate;
            
            logger.LogInformation("Exchange rate not found. Fetching from external provider.");
            var externalRate = await externalProvider.FetchExchangeRateAsync(baseCurrency, quoteCurrency);

            if (externalRate == null)
            {
                logger.LogError("Failed to fetch exchange rate for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);
                return null;
            }
                
            rate = new ExchangeRate(new CurrencyPair(baseCurrency, quoteCurrency), externalRate.Bid, externalRate.Ask);
            await repository.AddRateAsync(rate);
            logger.LogInformation("Added new exchange rate for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);

            return rate;
        }

        public async Task<ExchangeRate> AddOrUpdateRateAsync(string baseCurrency, string quoteCurrency, decimal bid, decimal ask)
        {
            logger.LogInformation("Adding or updating exchange rate for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);

            var rate = await repository.GetRateAsync(baseCurrency, quoteCurrency);
            if (rate == null)
            {
                rate = new ExchangeRate(new CurrencyPair(baseCurrency, quoteCurrency), bid, ask);
                await repository.AddRateAsync(rate);
                logger.LogInformation("Added new exchange rate for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);
            }
            else
            {
                logger.LogInformation("Exchange rate found. Updating values.");
                rate.UpdateRate(bid, ask);
                await repository.UpdateRateAsync(rate);
                logger.LogInformation("Updated exchange rate for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);
            }

            return rate;
        }
    }
}
