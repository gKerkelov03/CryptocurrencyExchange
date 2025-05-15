using Application.Abstractions;
using Application.DataStructures;
using Application.Domain;
using Application.Models;

namespace Application.Services;

public class BalanceService : IBalanceService
{
    private readonly IEfRepository<Balance> _balanceRepository;
    private readonly ICryptoPriceService _cryptoPriceService;

    public BalanceService(IEfRepository<Balance> balanceRepository, ICryptoPriceService cryptoPriceService)
    {
        _balanceRepository = balanceRepository;
        _cryptoPriceService = cryptoPriceService;
    }

    public async Task<SingleCurrency<Usd>> CalculateTheTotalBalanceInUsd(Guid userId)
    {
        var vsCurrency = "usd";
        var balances = await _balanceRepository.FindAllAsync(b => b.UserId == userId);

        var cryptoIds = balances.Select(b => b?.Cryptocurrency?.CryptocurrencyId).Where(id => id != null).ToArray();
        var prices = await _cryptoPriceService.GetCryptoPricesAsync(cryptoIds, vsCurrency);

        double total = 0;

        foreach (var balance in balances)
        {
            total += balance.Amount * prices[balance!.Cryptocurrency!.CryptocurrencyId];
        }

        var totalInUsd = new SingleCurrency<Usd>(total);

        return totalInUsd;
    }

    public async Task<Dictionary<string, string>> GetBalancesToDisplay(Guid userId)
    {
        var balances = await _balanceRepository.FindAllAsync(b => b.UserId == userId);

        return balances.ToDictionary(b => b!.Cryptocurrency!.Name, b => b.Amount.ToString());
    }

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