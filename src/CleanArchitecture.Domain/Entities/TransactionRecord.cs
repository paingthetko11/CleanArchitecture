using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public enum TransactionType { Income = 1, Expense = 2 }

public sealed class TransactionRecord : AuditableEntity, ISoftDelete
{
    private TransactionRecord() { }

    public TransactionRecord(Guid bankId, TransactionType type, decimal amount, string description, DateTimeOffset occurredAt)
    {
        if (bankId == Guid.Empty) throw new ArgumentException("A bank is required.", nameof(bankId));
        if (!Enum.IsDefined(type)) throw new ArgumentOutOfRangeException(nameof(type));
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.", nameof(description));
        Id = Guid.NewGuid(); BankId = bankId; Type = type; Amount = amount;
        Description = description.Trim(); OccurredAt = occurredAt;
    }

    public Guid Id { get; private set; }
    public Guid BankId { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; private set; }
    public bool IsDeleted { get; set; }
}
