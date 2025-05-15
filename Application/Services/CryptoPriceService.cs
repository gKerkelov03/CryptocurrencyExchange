
using Newtonsoft.Json;

namespace Application.Services;

public class CryptoPriceService : ICryptoPriceService
{
    private readonly HttpClient _httpClient;

    public CryptoPriceService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<Dictionary<string, double>> GetCryptoPricesAsync(string[] coinIds, string vsCurrency)
    {
        string ids = string.Join(",", coinIds);
        string url = $"https://api.coingecko.com/api/v3/simple/price?ids={ids}&vs_currencies={vsCurrency}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to fetch data from CoinGecko");
        }
            
        string json = await response.Content.ReadAsStringAsync();
        var deserialized = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, double>>>(json);
        var prices = deserialized!.ToDictionary(kvp => kvp.Key, kvp => kvp.Value[vsCurrency]);

        return prices;
    }
}
