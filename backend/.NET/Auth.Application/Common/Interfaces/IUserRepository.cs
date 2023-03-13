
using Auth.Domain.Entities;

namespace Auth.Application.Interface;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    void Add(User user);
    void CreatePaswwordHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt);
    bool VerifyPasswordHash(
        string password,
        byte[] passwordHash,
        byte[] passwordSalt);
}