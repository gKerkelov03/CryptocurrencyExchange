using Application.DataStructures;
using Application.Domain;
using Application.Models;

namespace Application.Abstractions;

public interface IBalanceService : ITransientLifetime
{
    Task TransferAsync(TransferRequest request);

    Task<SingleCurrency<Usd>> CalculateTheTotalBalanceInUsd(Guid userId);

    Task<IEnumerable<Balance>> GetBalancesToDisplay(Guid userId);
} 