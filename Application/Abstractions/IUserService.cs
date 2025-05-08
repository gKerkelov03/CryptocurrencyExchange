using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Domain;
using SmartSalon.Application.ResultObject;

namespace Application.Abstractions;

public interface IUserService : ITransientLifetime
{
    Task<Result<User>> LoginAsync(string username, string password);
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByUsernameAsync(string username);
    Task<bool> VerifyPasswordAsync(string username, string password);
    Task<IEnumerable<User>> GetAllUsersAsync();
} 