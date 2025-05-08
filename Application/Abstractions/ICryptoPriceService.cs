using Application.Abstractions;

public interface ICryptoPriceService : ITransientLifetime
{
    Task<Dictionary<string, Dictionary<string, double>>> GetCryptoPricesAsync(string[] cryptoIds, string outputCurrency);
}
