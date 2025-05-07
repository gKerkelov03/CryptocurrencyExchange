
namespace Application.Errors;

public class ConflictError : Error
{
    public ConflictError(string description) : base(description) { }
}
