
using System.ComponentModel.DataAnnotations;
using Application.Domain.Base;


namespace Application.Domain;

public class Role : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public  virtual ICollection<User>? Users { get; set; }
}