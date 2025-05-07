
namespace Application.Errors;

public class UnauthorizedError : Error
{
    public UnauthorizedError(string description) : base(description) { }
}
