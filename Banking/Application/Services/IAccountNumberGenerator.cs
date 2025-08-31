using Banking.Api.Persistence;

namespace Banking.Api.Application.Services
{
    public interface IAccountNumberGenerator
    {
        /// <summary>Genera un número de cuenta único (solo dígitos) de 12 caracteres.</summary>
        Task<string> GenerateUniqueAsync(AppDbContext db, CancellationToken ct = default);
    }
}
