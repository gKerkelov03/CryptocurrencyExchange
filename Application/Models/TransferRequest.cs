

namespace Application.Models;

public class TransferRequest
{
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public Guid CryptocurrencyId { get; set; }
    public double Amount { get; set; }
} 