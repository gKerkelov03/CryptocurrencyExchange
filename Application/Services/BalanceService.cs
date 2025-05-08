using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Domain;
using Application.Domain.Base;
using Application.Errors;
using Microsoft.EntityFrameworkCore;
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

            // Validate the transfer
            if (fromBalance.Amount < request.Amount)
                throw new InvalidOperationException($"Insufficient balance for transfer. Available: {fromBalance.Amount} {fromBalance.Cryptocurrency.Name}");

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
            fromBalance.Amount -= request.Amount;
            toBalance.Amount += request.Amount;

            // Update both balances
            _balanceRepository.Update(fromBalance);
            _balanceRepository.Update(toBalance);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Transfer failed: {ex.Message}", ex);
        }
    }
} 