using DataStructures.Cryptos;

namespace DataStructures;

public class Example
{
    public static void ShowExample()
    {
        var btc = new SingleCurrency<Bitcoin>(100);
        var eth = new SingleCurrency<Ethereum>(50);

        var wallet = new MultiCurrency()
            .Add(btc)
            .Add(eth);

        Console.WriteLine(wallet); // 100 USD, 50 EUR

        var rates = new Dictionary<Type, double>
        {
            { typeof(Bitcoin), 1.1 }
        };

        var totalInUsd = wallet.ConvertTo<Bitcoin>(rates);
        Console.WriteLine(totalInUsd);
    }
}