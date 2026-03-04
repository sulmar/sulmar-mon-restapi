using OrderApi.Application.Contracts;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure.Services;

public class NbpApiCurrencyRateService(HttpClient http) : ICurrencyRateService
{
    const string table = "A";

    public async Task<decimal> GetRate(string code)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();
        var request = $"/api/exchangerates/rates/{table}/{normalizedCode}/";

        var data = await http.GetFromJsonAsync<NbpRateResponse>(request);
        if (data?.Rates == null || data.Rates.Count == 0)
            throw new InvalidOperationException($"Brak kursu dla waluty: {normalizedCode}.");

        return data.Rates[0].Mid;
    }
}

internal sealed class NbpRateResponse
{
    [JsonPropertyName("table")]
    public string Table { get; init; } = "";

    [JsonPropertyName("currency")]
    public string Currency { get; init; } = "";

    [JsonPropertyName("code")]
    public string Code { get; init; } = "";

    [JsonPropertyName("rates")]
    public List<NbpRateItem> Rates { get; init; } = [];
}

internal sealed class NbpRateItem
{
    [JsonPropertyName("no")]
    public string No { get; init; } = "";

    [JsonPropertyName("effectiveDate")]
    public string EffectiveDate { get; init; } = "";

    [JsonPropertyName("mid")]
    public decimal Mid { get; init; }
}
