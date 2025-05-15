using System.Net.NetworkInformation;
using Application.Abstractions;
using Application.DataStructures;
using Application.Domain;
using Application.Models;

namespace Application.Services;

public class BalanceService : IBalanceService
{
    private readonly IEfRepository<Balance> _balanceRepository;
    private readonly ICurrencyPriceService _currencyPriceService;

    public BalanceService(IEfRepository<Balance> balanceRepository, ICurrencyPriceService currencyPriceService)
    {
        _balanceRepository = balanceRepository;
        _currencyPriceService = currencyPriceService;
    }

    public async Task<SingleCurrency<Usd>> CalculateTheTotalBalanceInUsd(Guid userId)
    {
        var vsCurrency = "usd";
        var balances = await _balanceRepository.FindAllAsync(b => b.UserId == userId);

        var cryptoIds = balances.Select(b => b?.Cryptocurrency?.CryptocurrencyId).Where(id => id != null).ToArray();
        var prices = await _currencyPriceService.GetCryptoPricesAsync(cryptoIds, vsCurrency);
        var exchangeRates = new Dictionary<Type, double>();
        var total = new MultiCurrency();

        foreach (var balance in balances)
        {
            if (balance!.Cryptocurrency!.CryptocurrencyId == Bitcoin.CryptocurrencyId)
            {
                total += new SingleCurrency<Bitcoin>(balance.Amount);
                exchangeRates.Add(typeof(Bitcoin), prices[Bitcoin.CryptocurrencyId]);
            }
            else if (balance!.Cryptocurrency!.CryptocurrencyId == Ethereum.CryptocurrencyId)
            {
                total += new SingleCurrency<Ethereum>(balance.Amount);
                exchangeRates.Add(typeof(Ethereum), prices[Ethereum.CryptocurrencyId]);
            }
            else if (balance!.Cryptocurrency!.CryptocurrencyId == Solana.CryptocurrencyId)
            { 
                total += new SingleCurrency<Solana>(balance.Amount);
                exchangeRates.Add(typeof(Solana), prices[Solana.CryptocurrencyId]);
            }
        }

        
        var totalInUsd = total.ConvertTo<Usd>(exchangeRates);

        return totalInUsd;
    }

    public async Task<IEnumerable<Balance>> GetBalancesToDisplay(Guid userId) => await _balanceRepository.FindAllAsync(b => b.UserId == userId);

    public async Task TransferAsync(TransferRequest request)
    {
        try
        {
            var fromBalance = await _balanceRepository.FirstOrDefaultAsync(b =>
                b.UserId == request.FromUserId &&
                b.CryptocurrencyId == request.CryptocurrencyId);

            if (fromBalance == null)
                throw new InvalidOperationException("Sender does not have a balance for this cryptocurrency.");

            if (fromBalance.Cryptocurrency == null)
                throw new InvalidOperationException("Cryptocurrency information is missing.");

            var toBalance = await _balanceRepository.FirstOrDefaultAsync(b =>
                b.UserId == request.ToUserId &&
                b.CryptocurrencyId == request.CryptocurrencyId);

            if (fromBalance.Amount < request.Amount)
                throw new InvalidOperationException($"Insufficient balance for transfer. Available: {fromBalance.Amount} {fromBalance.Cryptocurrency.Name}");

            if (toBalance == null)
            {
                toBalance = new Balance
                {
                    UserId = request.ToUserId,
                    CryptocurrencyId = request.CryptocurrencyId,
                    Amount = 0
                };
                await _balanceRepository.AddAsync(toBalance);
            }

            fromBalance.Amount -= request.Amount;
            toBalance.Amount += request.Amount;

            _balanceRepository.Update(fromBalance);
            _balanceRepository.Update(toBalance);

            await _balanceRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Transfer failed: {ex.Message}", ex);
        }
    }
}