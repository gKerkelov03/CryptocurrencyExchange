using Application.DataStructures;
using Application.Models;

namespace Application.Abstractions;

public interface IBalanceService : ITransientLifetime
{
    Task TransferAsync(TransferRequest request);

    Task<SingleCurrency<Usd>> CalculateTheTotalBalanceInUsd(Guid userId);

    Task<Dictionary<string, double>> GetBalancesToDisplay(Guid userId);
} 