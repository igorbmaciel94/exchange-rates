using Application.Models;
using System.Threading.Tasks;

namespace ExchangeRates.Application.Interfaces
{
    public interface IExternalExchangeRateProvider
    {
        Task<ExternalRate> FetchExchangeRateAsync(string baseCurrency, string quoteCurrency);
    }
}
