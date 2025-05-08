namespace Application.Abstractions;

public interface IPasswordHasher : ITransientLifetime
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
} 