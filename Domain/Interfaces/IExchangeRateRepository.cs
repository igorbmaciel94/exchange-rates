
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IExchangeRateRepository
    {
        Task<ExchangeRate> GetRateAsync(string baseCurrency, string quoteCurrency);
        Task AddRateAsync(ExchangeRate rate);
        Task UpdateRateAsync(ExchangeRate rate);
    }
}
