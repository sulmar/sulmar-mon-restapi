using OrderApi.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure.Services;

public class NbpApiCurrencyRateService(HttpClient http) : ICurrencyRateService
{
    const string table = "A";

    public async Task<decimal> GetRate(string code)
    {
        var request = $"/api/exchangerates/rates/{table}/{code}/";

        var response = await http.GetStringAsync(request);

        return 0;
    }
}
