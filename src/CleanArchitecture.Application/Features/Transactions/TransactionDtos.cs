using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Features.Transactions;

public sealed record TransactionDto(Guid Id, Guid BankId, TransactionType Type, decimal Amount, string Description, DateTimeOffset OccurredAt);
public sealed record CreateTransactionRequest(Guid BankId, TransactionType Type, decimal Amount, string Description, DateTimeOffset OccurredAt);
