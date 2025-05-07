using Application.Errors;

namespace SmartSalon.Application.ResultObject;

public interface IResult
{
    public bool IsSuccess { get; }

    public bool IsFailure { get; }

    public IEnumerable<Error>? Errors { get; }
}
