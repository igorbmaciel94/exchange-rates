namespace ExchangeRates.Domain.Entities
{
    public class CurrencyPair
    {
        public string BaseCurrency { get; private set; }
        public string QuoteCurrency { get; private set; }

        // Parameterless constructor for EF Core
        private CurrencyPair() { }

        // Parameterized constructor for application usage
        public CurrencyPair(string baseCurrency, string quoteCurrency)
        {
            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
        }
    }
}
