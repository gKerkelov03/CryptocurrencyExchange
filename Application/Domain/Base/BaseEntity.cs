using System.ComponentModel.DataAnnotations;

namespace Application.Domain.Base;

public abstract class BaseEntity : IBaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}
