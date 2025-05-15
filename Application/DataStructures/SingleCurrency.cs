using Application.Abstractions;

namespace Application.DataStructures;
public readonly struct SingleCurrency<TCurrency> where TCurrency : ICrypto
{
    private readonly double _amount;

    public SingleCurrency(double amount) => _amount = amount;

    public SingleCurrency<TOtherCurrency> ConvertTo<TOtherCurrency>(double exchangeRate)
        where TOtherCurrency : ICrypto
            => new SingleCurrency<TOtherCurrency>(_amount * exchangeRate);

    public double GetAmount<TRequestedCurrency>() where TRequestedCurrency: TCurrency => _amount;

    public static SingleCurrency<TCurrency> operator +(SingleCurrency<TCurrency> a, SingleCurrency<TCurrency> b)
        => new SingleCurrency<TCurrency>(a._amount + b._amount);

    public static SingleCurrency<TCurrency> operator -(SingleCurrency<TCurrency> a, SingleCurrency<TCurrency> b)
        => new SingleCurrency<TCurrency>(a._amount - b._amount);

    public static bool operator ==(SingleCurrency<TCurrency> a, SingleCurrency<TCurrency> b)
        => a._amount == b._amount;

    public static bool operator !=(SingleCurrency<TCurrency> a, SingleCurrency<TCurrency> b)
        => !(a == b);

    public override string ToString() => $"{_amount} {typeof(TCurrency).Name}";

    public override bool Equals(object obj)
        => obj is SingleCurrency<TCurrency> other && this == other;

    public override int GetHashCode() => typeof(TCurrency).GetHashCode() + _amount.GetHashCode();
}