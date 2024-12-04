using System.Text.Json.Serialization;

namespace Infrastructure.Services;

public class ExchangeRateResponse
{
    [JsonPropertyName("Realtime Currency Exchange Rate")]
    public ExchangeRateData RealtimeCurrencyExchangeRate { get; set; }
}

public class ExchangeRateData
{
    [JsonPropertyName("1. From_Currency Code")]
    public string FromCurrencyCode { get; set; }

    [JsonPropertyName("2. From_Currency Name")]
    public string FromCurrencyName { get; set; }

    [JsonPropertyName("3. To_Currency Code")]
    public string ToCurrencyCode { get; set; }

    [JsonPropertyName("4. To_Currency Name")]
    public string ToCurrencyName { get; set; }

    [JsonPropertyName("5. Exchange Rate")]
    public decimal ExchangeRate { get; set; }

    [JsonPropertyName("6. Last Refreshed")]
    public string LastRefreshed { get; set; }

    [JsonPropertyName("7. Time Zone")]
    public string TimeZone { get; set; }

    [JsonPropertyName("8. Bid Price")]
    public decimal BidPrice { get; set; }

    [JsonPropertyName("9. Ask Price")]
    public decimal AskPrice { get; set; }
}