using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.Domain;
using Application.Domain.Base;
using Application.Errors;
using SmartSalon.Application.ResultObject;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IEfRepository<User> _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IEfRepository<User> userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<User>> LoginAsync(string username, string password)
    {
        var user = await GetUserByUsernameAsync(username);

        if (user == null)
        {
            return Result<User>.Failure(Error.NotFound);
        }

        if (!await VerifyPasswordAsync(username, password))
        {
            return Result<User>.Failure(Error.Unauthorized);
        }

        return Result<User>.Success(user);
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _userRepository.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> VerifyPasswordAsync(string username, string password)
    {
        var user = await GetUserByUsernameAsync(username);
        if (user == null)
        {
            return false;
        }

        return _passwordHasher.VerifyPassword(password, user.Password);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.FindAllAsync(u => true);
    }
} 