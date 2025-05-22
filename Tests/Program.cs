
using Application.DataStructures;
using Application.Models;

namespace DataStructures;

public class Program
{
    private static SingleCurrency<Bitcoin> btc1 = new SingleCurrency<Bitcoin>(1.5);
    private static SingleCurrency<Bitcoin> btc2 = new SingleCurrency<Bitcoin>(2.0);
    private static SingleCurrency<Ethereum> eth1 = new SingleCurrency<Ethereum>(3.0);

    public static void Main()
    {
        TestSingleCurrency();
        TestMultiCurrency();
    }

    public static void TestSingleCurrency()
    {
        // Addition
        var btcTotal = btc1 + btc2;
        Console.WriteLine($"BTC Total: {btcTotal}");

        // Subtraction
        var btcDiff = btc2 - btc1;
        Console.WriteLine($"BTC Difference: {btcDiff}");

        // Equality
        Console.WriteLine($"btc1 == btc2: {btc1 == btc2}");
        Console.WriteLine($"btcTotal == (btc1 + btc2): {btcTotal == (btc1 + btc2)}");

        // Conversion
        double btcToEthRate = 10; // 1 BTC = 10 ETH
        var ethFromBtc = btc1.ConvertTo<Ethereum>(btcToEthRate);
        Console.WriteLine($"Converted BTC to ETH: {ethFromBtc}");

        // Get amount
        double amount = btc1.GetAmount<Bitcoin>();
        Console.WriteLine($"Amount in btc1: {amount}");
    }

    public static void TestMultiCurrency()
    {
        var multi = new MultiCurrency()
           .Add(btc1)
           .Add(eth1);

        Console.WriteLine($"Initial MultiCurrency: {multi}");

        // Add more of same currency
        multi = multi.Add(btc2);
        Console.WriteLine($"After adding more BTC: {multi}");

        // Subtract currency
        multi = multi.Subtract<Bitcoin>(btc1);
        Console.WriteLine($"After subtracting BTC: {multi}");

        // Get specific currency amount
        var getBtc = multi.Get<Bitcoin>();
        Console.WriteLine($"Get BTC from MultiCurrency: {getBtc}");

        // Convert MultiCurrency to another (ETH in this case)
        var exchangeRates = new Dictionary<Type, double>
        {
            { typeof(Bitcoin), 10.0 },   // 1 BTC = 10 ETH
            { typeof(Ethereum), 1.0 }    // 1 ETH = 1 ETH
        };

        var totalInEth = multi.ConvertTo<Ethereum>(exchangeRates);
        Console.WriteLine($"MultiCurrency converted to ETH: {totalInEth}");

        // Convert to new currency not originally present
        var exchangeRatesToSolana = new Dictionary<Type, double>
        {
            { typeof(Bitcoin), 10000.0 },    // 1 BTC = 10,000 DOGE
            { typeof(Ethereum), 500.0 }      // 1 ETH = 500 DOGE
        };

        var totalInDoge = multi.ConvertTo<Solana>(exchangeRatesToSolana);
        Console.WriteLine($"MultiCurrency converted to DOGE: {totalInDoge}");
    }
}