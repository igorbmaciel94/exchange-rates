using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IExchangeRateService
    {
        Task<ExchangeRate> GetRateAsync(string baseCurrency, string quoteCurrency);

        Task<ExchangeRate> AddOrUpdateRateAsync(string baseCurrency, string quoteCurrency, decimal bid, decimal ask);
    }
}
