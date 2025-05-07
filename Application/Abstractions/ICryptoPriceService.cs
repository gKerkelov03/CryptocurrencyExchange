


using Application.Abstractions;

public interface ICryptoPriceService : ITransientLifetime
{
    Task<Dictionary<string, Dictionary<string, decimal>>> GetCryptoPricesAsync(string[] cryptoIds, string outputCurrency);
}
