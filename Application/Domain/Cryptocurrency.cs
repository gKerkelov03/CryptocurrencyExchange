using System.ComponentModel.DataAnnotations;
using Application.Domain.Base;

namespace Application.Domain;

public class Cryptocurrency : BaseEntity
{
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    [Required]
    public required string CryptocurrencyId { get; set; }
}
