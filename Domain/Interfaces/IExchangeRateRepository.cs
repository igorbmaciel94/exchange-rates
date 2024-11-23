
using System.Threading.Tasks;
using ExchangeRates.Domain.Entities;

namespace ExchangeRates.Domain.Interfaces
{
    public interface IExchangeRateRepository
    {
        Task<ExchangeRate> GetRateAsync(string baseCurrency, string quoteCurrency);
        Task AddRateAsync(ExchangeRate rate);
        Task UpdateRateAsync(ExchangeRate rate);
    }
}
