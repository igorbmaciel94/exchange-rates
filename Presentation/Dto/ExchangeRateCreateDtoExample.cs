namespace Presentation.Dto
{
    using Swashbuckle.AspNetCore.Filters;

    public class ExchangeRateCreateDtoExample : IExamplesProvider<ExchangeRateCreateDto>
    {
        public ExchangeRateCreateDto GetExamples()
        {
            return new ExchangeRateCreateDto
            {
                BaseCurrency = "USD",
                QuoteCurrency = "EUR",
                Bid = 1.1m,
                Ask = 1.2m
            };
        }
    }

}
