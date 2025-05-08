using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Domain;
using Application.Domain.Base;
using Application.Errors;
using SmartSalon.Application.ResultObject;

namespace Application.Services;

public class BalanceService : IBalanceService
{
    private readonly IEfRepository<Balance> _balanceRepository;

    public BalanceService(IEfRepository<Balance> balanceRepository)
    {
        _balanceRepository = balanceRepository;
    }

    public async Task<IEnumerable<Balance>> GetUserBalancesAsync(Guid userId)
    {
        return await _balanceRepository.FindAllAsync(b => b.UserId == userId);
    }

    public async Task TransferAsync(TransferRequest request)
    {
        // Get the balances involved in the transfer
        var fromBalance = await _balanceRepository.FirstOrDefaultAsync(b => 
            b.UserId == request.FromUserId && 
            b.CryptocurrencyId == request.CryptocurrencyId);

        var toBalance = await _balanceRepository.FirstOrDefaultAsync(b => 
            b.UserId == request.ToUserId && 
            b.CryptocurrencyId == request.CryptocurrencyId);

        // Validate the transfer
        if (fromBalance == null)
            throw new InvalidOperationException("Sender does not have a balance for this cryptocurrency.");

        if ((decimal)fromBalance.Amount < request.Amount)
            throw new InvalidOperationException("Insufficient balance for transfer.");

        // Create toBalance if it doesn't exist
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

        // Perform the transfer
        fromBalance.Amount = (double)((decimal)fromBalance.Amount - request.Amount);
        toBalance.Amount = (double)((decimal)toBalance.Amount + request.Amount);

        // Update both balances
        _balanceRepository.Update(fromBalance);
        _balanceRepository.Update(toBalance);
    }
} 