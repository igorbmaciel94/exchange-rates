namespace Presentation.Dto
{
    public class ExchangeRateCreateDto
    {
        public required string BaseCurrency { get; set; }
        public required string QuoteCurrency { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }
}
