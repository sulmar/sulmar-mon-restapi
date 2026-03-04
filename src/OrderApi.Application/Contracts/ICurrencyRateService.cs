namespace OrderApi.Application.Contracts;

public interface ICurrencyRateService
{
    Task<decimal> GetRate(string code);
}