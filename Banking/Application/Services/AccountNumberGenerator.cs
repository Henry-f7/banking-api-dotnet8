using System.Security.Cryptography;
using System.Text;
using Banking.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Api.Application.Services
{
    public class AccountNumberGenerator : IAccountNumberGenerator
    {
        private const int Length = 12;

        public async Task<string> GenerateUniqueAsync(AppDbContext db, CancellationToken ct = default)
        {
            for (int i = 0; i < 20; i++)
            {
                var candidate = GenerateDigits(Length);
                var exists = await db.BankAccounts.AnyAsync(a => a.AccountNumber == candidate, ct);
                if (!exists) return candidate;
            }
            throw new InvalidOperationException("Could not generate a unique account number");
        }

        private static string GenerateDigits(int length)
        {
            var sb = new StringBuilder(length);
            Span<byte> b = stackalloc byte[1];
            using var rng = RandomNumberGenerator.Create();
            for (int i = 0; i < length; i++)
            {
                byte d;
                do { rng.GetBytes(b); d = (byte)(b[0] % 10); } while (d > 9);
                sb.Append((char)('0' + d));
            }
            return sb.ToString();
        }
    }
}
