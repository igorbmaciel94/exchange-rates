using Microsoft.EntityFrameworkCore;
using ExchangeRates.Domain.Entities;

namespace ExchangeRates.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRate>().OwnsOne(p => p.Pair);
            base.OnModelCreating(modelBuilder);
        }
    }
}
