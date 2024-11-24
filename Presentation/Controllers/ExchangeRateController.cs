using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Presentation.Dto;
using Swashbuckle.AspNetCore.Filters;
using Domain.Entities;
using Application.Interfaces;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController(IExchangeRateService service, ILogger<ExchangeRateController> logger) : ControllerBase
    {
        private readonly IExchangeRateService _service = service;
        private readonly ILogger<ExchangeRateController> _logger = logger;

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
            _logger.LogInformation("Fetching exchange rate for {BaseCurrency}/{QuoteCurrency}", baseCurrency, quoteCurrency);
            var rate = await _service.GetRateAsync(baseCurrency, quoteCurrency);

            if (rate != null)
            {
                _logger.LogInformation("Exchange rate found for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);
                return Ok(rate);
            }

            _logger.LogWarning("Exchange rate not found for {BaseCurrency}/{QuoteCurrency}.", baseCurrency, quoteCurrency);
            return NotFound();
        }

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
            _logger.LogInformation("Adding or updating exchange rate for {BaseCurrency}/{QuoteCurrency}.", dto.BaseCurrency, dto.QuoteCurrency);

            try
            {
                var rate = await _service.AddOrUpdateRateAsync(dto.BaseCurrency, dto.QuoteCurrency, dto.Bid, dto.Ask);
                _logger.LogInformation("Successfully added or updated exchange rate for {BaseCurrency}/{QuoteCurrency}.", dto.BaseCurrency, dto.QuoteCurrency);

                return CreatedAtAction(nameof(GetRate), new { rate.Pair.BaseCurrency, rate.Pair.QuoteCurrency }, rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding or updating exchange rate for {BaseCurrency}/{QuoteCurrency}.", dto.BaseCurrency, dto.QuoteCurrency);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
