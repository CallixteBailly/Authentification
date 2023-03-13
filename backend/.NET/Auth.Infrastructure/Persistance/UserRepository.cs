using System.Security.Cryptography;
using System.Text;
using Auth.Application.Interface;
using Auth.Domain.Entities;
using Auth.Infrastructure.Context;

namespace Auth.Infrastructure.Persistance;
public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _authDbContext;

    public UserRepository(AuthDbContext authDbContext)
    {
        _authDbContext = authDbContext;
    }
    public User? GetUserByEmail(string email)
    {
        return _authDbContext.Users?.SingleOrDefault(u => u.Email == email);
    }
    public void Add(User user)
    {
        _authDbContext.Add(user);
        _authDbContext.SaveChanges();
    }
    public void CreatePaswwordHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
    public bool VerifyPasswordHash(
        string password,
        byte[] passwordHash,
        byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computeHash.SequenceEqual(passwordHash);
    }
}