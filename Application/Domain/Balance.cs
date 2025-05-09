﻿using System.ComponentModel.DataAnnotations;
using Application.Domain.Base;

namespace Application.Domain;

public class Balance : BaseEntity
{
    [Required]
    public required double Amount { get; set; }

    public Guid UserId { get; set; }
    public virtual User? User { get; set; }

    public Guid CryptocurrencyId { get; set; }
    public virtual Cryptocurrency? Cryptocurrency { get; set; }
}
