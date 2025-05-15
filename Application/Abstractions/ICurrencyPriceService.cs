using Application.Abstractions;

public interface ICurrencyPriceService : ITransientLifetime
{
    Task<Dictionary<string, double>> GetCryptoPricesAsync(string[] cryptoIds, string outputCurrency);
}
