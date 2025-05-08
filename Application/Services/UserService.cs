using Application.Abstractions;
using Application.Domain;
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
        var user = await _userRepository.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return Error.NotFound;
        }

        if (!_passwordHasher.VerifyPassword(password, user.Password))
        {
            return Error.Unauthorized;
        }

        return user;
    }
} 