
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Interfaces;
using ExchangeRates.Infrastructure.Data;

namespace ExchangeRates.Infrastructure.Services
{
    public class ExchangeRateRepository(AppDbContext context) : IExchangeRateRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<ExchangeRate?> GetRateAsync(string baseCurrency, string quoteCurrency)
        {
            return await _context.ExchangeRates
                .FirstOrDefaultAsync(r => r.Pair.BaseCurrency == baseCurrency && r.Pair.QuoteCurrency == quoteCurrency);
        }

        public async Task AddRateAsync(ExchangeRate rate)
        {
            await _context.ExchangeRates.AddAsync(rate);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRateAsync(ExchangeRate rate)
        {
            // Attach the entity if it's not already tracked
            var existingEntry = _context.ExchangeRates.Local
                .FirstOrDefault(e => e.Id == rate.Id);
            if (existingEntry == null)
            {
                _context.ExchangeRates.Attach(rate);
            }

            // Mark the entity as modified
            _context.Entry(rate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
