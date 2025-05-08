using Application.Domain;
using SmartSalon.Application.ResultObject;

namespace Application.Abstractions;

public interface IUserService : ITransientLifetime
{
    Task<Result<User>> LoginAsync(string username, string password);
} 