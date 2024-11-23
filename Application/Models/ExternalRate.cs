namespace Application.Models
{
    public class ExternalRate(decimal bid, decimal ask)
    {
        public decimal Bid { get; set; } = bid;
        public decimal Ask { get; set; } = ask;
    }
}
