namespace ExchangeRates.Domain.Entities
{
    public class ExchangeRate
    {
        public int Id { get; private set; }
        public CurrencyPair Pair { get; private set; }
        public decimal Bid { get; private set; }
        public decimal Ask { get; private set; }

        // Parameterless constructor for EF Core
        private ExchangeRate() { }

        public ExchangeRate(CurrencyPair pair, decimal bid, decimal ask)
        {
            Pair = pair;
            Bid = bid;
            Ask = ask;
        }

        // Method to update the rate
        public void UpdateRate(decimal bid, decimal ask)
        {
            Bid = bid;
            Ask = ask;
        }
    }
}
