
using Newtonsoft.Json;

namespace Application.Services;

public class CryptoPriceService : ICryptoPriceService
{
    private readonly HttpClient _httpClient;

    public CryptoPriceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<string, Dictionary<string, decimal>>> GetCryptoPricesAsync(string[] coinIds, string vsCurrency)
    {
        string ids = string.Join(",", coinIds);
        string url = $"https://api.coingecko.com/api/v3/simple/price?ids={ids}&vs_currencies={vsCurrency}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to fetch data from CoinGecko");
        }
            
        string json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, decimal>>>(json);
        return result;
    }
}
