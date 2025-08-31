namespace Banking.Api.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }
        public string? UpdatedBy { get; set; }

        public DateTime? DeletedAtUtc { get; set; }
        public bool IsDeleted => DeletedAtUtc.HasValue;
    }
}
