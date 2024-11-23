using Microsoft.AspNetCore.Mvc;
using ExchangeRates.Application.Interfaces;
using ExchangeRates.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace ExchangeRates.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController(IExchangeRateService service) : ControllerBase
    {
        private readonly IExchangeRateService _service = service;

        /// <summary>
        /// Get the exchange rate for a currency pair.
        /// </summary>
        /// <param name="baseCurrency">The base currency (e.g., USD).</param>
        /// <param name="quoteCurrency">The quote currency (e.g., EUR).</param>
        /// <returns>The exchange rate for the specified currency pair.</returns>
        [HttpGet("{baseCurrency}/{quoteCurrency}")]
        [SwaggerOperation(
            Summary = "Get exchange rate",
            Description = "Retrieve the exchange rate for a specific currency pair."
        )]
        [SwaggerResponse(200, "Success", typeof(ExchangeRate))]
        [SwaggerResponse(404, "Not Found")]
        public async Task<IActionResult> GetRate(
            [FromRoute][SwaggerParameter(Description = "The base currency (e.g., USD)")] string baseCurrency,
            [FromRoute][SwaggerParameter(Description = "The quote currency (e.g., EUR)")] string quoteCurrency)
        {
            var rate = await _service.GetRateAsync(baseCurrency, quoteCurrency);
            return rate != null ? Ok(rate) : NotFound();
        }

        /// <summary>
        /// Add a new exchange rate.
        /// </summary>
        /// <param name="dto">The exchange rate data.</param>
        /// <returns>The created exchange rate.</returns>    
        [HttpPost]
        [SwaggerOperation(
            Summary = "Add or update exchange rate",
            Description = "Add or update an exchange rate for a currency pair. If the pair does not exist, fetch it from an external API."
        )]
        [SwaggerResponse(201, "Created or Updated", typeof(ExchangeRate))]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerRequestExample(typeof(ExchangeRateCreateDto), typeof(ExchangeRateCreateDtoExample))]
        public async Task<IActionResult> AddOrUpdateRate([FromBody] ExchangeRateCreateDto dto)
        {
            var rate = await _service.AddOrUpdateRateAsync(dto.BaseCurrency, dto.QuoteCurrency, dto.Bid, dto.Ask);

            return CreatedAtAction(nameof(GetRate), new { rate.Pair.BaseCurrency, rate.Pair.QuoteCurrency }, rate);
        }

    }
}
