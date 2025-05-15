using System.Reflection;
using Application.Abstractions;

namespace Application.DataStructures;

public struct MultiCurrency
{
    private readonly Dictionary<Type, object> _amounts;

    public MultiCurrency() => _amounts = new Dictionary<Type, object>();

    private MultiCurrency(Dictionary<Type, object> amounts) => _amounts = amounts;

    public MultiCurrency Add<TCurrency>(SingleCurrency<TCurrency> amount) where TCurrency : ICrypto
    {
        var copy = new Dictionary<Type, object>(_amounts);

        if (copy.TryGetValue(typeof(TCurrency), out var existing))
        {
            var existingAmount = (SingleCurrency<TCurrency>)existing;
            copy[typeof(TCurrency)] = existingAmount + amount;
        }
        else
        {
            copy[typeof(TCurrency)] = amount;
        }

        return new MultiCurrency(copy);
    }

    public SingleCurrency<TCurrency> Get<TCurrency>() where TCurrency : ICrypto
    {
        if (_amounts.TryGetValue(typeof(TCurrency), out var value))
        {
            return (SingleCurrency<TCurrency>)value;
        }

        return new SingleCurrency<TCurrency>(0);
    }

    public static MultiCurrency operator +(MultiCurrency a, MultiCurrency b)
    {
        var result = a;

        foreach (var kvp in b._amounts)
        {
            var currencyType = kvp.Key;
            var addMethod = typeof(MultiCurrency).GetMethod(nameof(Add)).MakeGenericMethod(currencyType);
            result = (MultiCurrency)addMethod.Invoke(result, [kvp.Value]);
        }

        return result;
    }

    public MultiCurrency Subtract<TCurrency>(SingleCurrency<TCurrency> amount) where TCurrency : ICrypto
    {
        var copy = new Dictionary<Type, object>(_amounts);
        if (copy.TryGetValue(typeof(TCurrency), out var existing))
        {
            var existingAmount = (SingleCurrency<TCurrency>)existing;
            copy[typeof(TCurrency)] = existingAmount - amount;
        }
        else
        {
            copy[typeof(TCurrency)] = new SingleCurrency<TCurrency>(-amount.GetAmount<TCurrency>());
        }

        return new MultiCurrency(copy);
    }

    public SingleCurrency<TTarget> ConvertTo<TTarget>(Dictionary<Type, double> exchangeRates) where TTarget : ICrypto
    {
        double total = 0;

        foreach (var kvp in _amounts)
        {
            var currencyType = kvp.Key;
            var rawAmount = kvp.Value;

            if (currencyType == typeof(TTarget))
            {
                var same = (SingleCurrency<TTarget>)rawAmount;
                total += same.GetAmount<TTarget>();
            }
            else
            {
                if (!exchangeRates.TryGetValue(currencyType, out var rate))
                    throw new InvalidOperationException($"Missing exchange rate for {currencyType.Name}");

                var method = typeof(MultiCurrency).GetMethod(nameof(ConvertCurrency), BindingFlags.NonPublic | BindingFlags.Static)
                    .MakeGenericMethod(currencyType, typeof(TTarget));

                double converted = (double)method.Invoke(null, [rawAmount, rate]);
                total += converted;
            }
        }

        return new SingleCurrency<TTarget>(total);
    }
    private static double ConvertCurrency<TFrom, TTo>(object amountObj, double rate) where TFrom : ICrypto where TTo : ICrypto
        => ((SingleCurrency<TFrom>)amountObj).ConvertTo<TTo>(rate).GetAmount<TTo>();

    public override string ToString()
        => string.Join(", ", _amounts.Select(kvp =>
        {
            var currencyName = kvp.Key.Name;
            var method = typeof(MultiCurrency).GetMethod(nameof(GetDisplayValue), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(kvp.Key);

            return $"{method.Invoke(null, [kvp.Value])} {currencyName}";
        }));

    private static string GetDisplayValue<TCurrency>(object obj) where TCurrency : ICrypto
        => ((SingleCurrency<TCurrency>)obj).GetAmount<TCurrency>().ToString("0.##");
}
