using Application.Abstractions;

public interface ICryptoPriceService : ITransientLifetime
{
    Task<Dictionary<string, double>> GetCryptoPricesAsync(string[] cryptoIds, string outputCurrency);
}
