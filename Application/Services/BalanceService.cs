using Application.Abstractions;
using Application.Domain;
using Application.Models;

namespace Application.Services;

public class BalanceService : IBalanceService
{
    private readonly IEfRepository<Balance> _balanceRepository;

    public BalanceService(IEfRepository<Balance> balanceRepository) => _balanceRepository = balanceRepository;

    public async Task<IEnumerable<Balance>> GetUserBalancesAsync(Guid userId)
    {
        try
        {
            var balances = await _balanceRepository.FindAllAsync(b => b.UserId == userId);
            return balances.Where(b => b.Cryptocurrency != null);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error retrieving user balances: {ex.Message}", ex);
        }
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