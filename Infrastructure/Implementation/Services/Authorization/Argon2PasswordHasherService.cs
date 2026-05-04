using System.Text;
using System.Security.Cryptography;

using Isopoh.Cryptography.Argon2;

using HealthInvoice.Core.Interfaces.Services.Authorization;

namespace HealthInvoice.Infrastructure.Implementation.Services.Authorization;

public class Argon2PasswordHasherService : IPasswordHasherService
{
    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);

        using var argon2 = new Argon2(
            new Argon2Config()
            {
                Type = Argon2Type.DataIndependentAddressing,
                Version = Argon2Version.Nineteen,
                TimeCost = 3,
                Lanes = 4,
                Threads = 4,
                HashLength = 32,
                Salt = salt,
                MemoryCost = 65536,
                Password = Encoding.UTF8.GetBytes(password) 
            });

        var hash = argon2.Hash();

        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash.Buffer)}";
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split(":");

        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        using var argon2 = new Argon2(
            new Argon2Config()
            {
                Type = Argon2Type.DataIndependentAddressing,
                Version = Argon2Version.Nineteen,
                TimeCost = 3,
                Lanes = 4,
                Threads = 4,
                HashLength = 32,
                Salt = salt,
                MemoryCost = 65536,
                Password = Encoding.UTF8.GetBytes(password)
            });

        var newHash = argon2.Hash();

        return CryptographicOperations.FixedTimeEquals(newHash.Buffer, hash);
    }
}
