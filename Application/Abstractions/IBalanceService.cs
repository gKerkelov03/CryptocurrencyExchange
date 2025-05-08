using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Domain;

namespace Application.Abstractions;

public interface IBalanceService : ITransientLifetime
{
    Task<IEnumerable<Balance>> GetUserBalancesAsync(Guid userId);
    Task TransferAsync(TransferRequest request);
} 