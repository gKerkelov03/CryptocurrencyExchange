
namespace Application.Errors;

public class ValidationError : Error
{
    public string PropertyName { get; init; }

    public ValidationError(string propertyName, string description) : base(description) => PropertyName = propertyName;
}
