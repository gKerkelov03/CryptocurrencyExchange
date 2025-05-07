
namespace Application.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string description) : base(description) { }
}
