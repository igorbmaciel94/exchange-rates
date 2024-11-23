using System.Threading.Tasks;
using ExchangeRates.Domain.Entities;

namespace ExchangeRates.Application.Interfaces
{
    public interface IExchangeRateService
    {
        Task<ExchangeRate> GetRateAsync(string baseCurrency, string quoteCurrency);

        Task<ExchangeRate> AddOrUpdateRateAsync(string baseCurrency, string quoteCurrency, decimal bid, decimal ask);        
    }
}
