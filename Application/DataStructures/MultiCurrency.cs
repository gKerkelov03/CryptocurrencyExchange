using System.Reflection;
using Application.Abstractions;

namespace Application.DataStructures;

public struct MultiCurrency
{
    private readonly Dictionary<Type, object> _amounts;

    public MultiCurrency() => _amounts = new Dictionary<Type, object>();

    private MultiCurrency(Dictionary<Type, object> amounts) => _amounts = amounts;

    public MultiCurrency Add<TCurrency>(SingleCurrency<TCurrency> amount) where TCurrency : ICurrency
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

    public MultiCurrency Subtract<TCurrency>(SingleCurrency<TCurrency> amount) where TCurrency : ICurrency
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

    public SingleCurrency<TCurrency> Get<TCurrency>() where TCurrency : ICurrency
    {
        if (_amounts.TryGetValue(typeof(TCurrency), out var value))
        {
            return (SingleCurrency<TCurrency>)value;
        }

        return new SingleCurrency<TCurrency>(0);
    }

    public SingleCurrency<TTarget> ConvertTo<TTarget>(Dictionary<Type, double> exchangeRates) where TTarget : ICurrency
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

    private static double ConvertCurrency<TFrom, TTo>(object amountObj, double rate)
        where TFrom : ICurrency
        where TTo : ICurrency
        => ((SingleCurrency<TFrom>)amountObj).ConvertTo<TTo>(rate).GetAmount<TTo>();

   
    private static string GetDisplayValue<TCurrency>(object obj) where TCurrency : ICurrency
        => ((SingleCurrency<TCurrency>)obj).GetAmount<TCurrency>().ToString("0.##");

    public static MultiCurrency operator +(MultiCurrency a, MultiCurrency b) => a.CombineWith(b, true);

    public static MultiCurrency operator -(MultiCurrency a, MultiCurrency b) => a.CombineWith(b, false);

    public static MultiCurrency operator +(MultiCurrency multi, object singleCurrency) => multi.Add((dynamic)singleCurrency);

    public static MultiCurrency operator +(object singleCurrency, MultiCurrency multi)
        => multi + singleCurrency;

    public static MultiCurrency operator -(MultiCurrency multi, object singleCurrency) => multi.Subtract((dynamic)singleCurrency);

    public static bool operator ==(MultiCurrency a, MultiCurrency b) => a.Equals(b);

    public static bool operator !=(MultiCurrency a, MultiCurrency b) => !a.Equals(b);

    public override bool Equals(object obj)
    {
        if (obj is not MultiCurrency other || _amounts.Count != other._amounts.Count)
            return false;

        foreach (var kvp in _amounts)
        {
            if (!other._amounts.TryGetValue(kvp.Key, out var otherValue))
                return false;

            if (!kvp.Value.Equals(otherValue))
                return false;
        }

        return true;
    }

    public override string ToString()
       => string.Join(", ", _amounts.Select(kvp =>
       {
           var currencyName = kvp.Key.Name;
           var method = typeof(MultiCurrency).GetMethod(nameof(GetDisplayValue), BindingFlags.NonPublic | BindingFlags.Static)!
               .MakeGenericMethod(kvp.Key);

           return $"{method.Invoke(null, [kvp.Value])} {currencyName}";
       }));

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var kvp in _amounts)
        {
            hash = hash * 31 + kvp.Key.GetHashCode();
            hash = hash * 31 + kvp.Value.GetHashCode();
        }
        return hash;
    }

    private MultiCurrency CombineWith(MultiCurrency other, bool isAddition)
    {
        var result = this;
        foreach (var kvp in other._amounts)
        {
            var currencyType = kvp.Key;
            var method = isAddition ? nameof(Add) : nameof(Subtract);

            var genericMethod = typeof(MultiCurrency).GetMethod(method).MakeGenericMethod(currencyType);
            result = (MultiCurrency)genericMethod.Invoke(result, [kvp.Value]);
        }
        return result;
    }
}

