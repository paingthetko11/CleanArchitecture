using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Transactions;

public sealed record CreateTransactionCommand(CreateTransactionRequest Request) : IRequest<Result<TransactionDto>>;

public sealed class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Request.BankId).NotEmpty();
        RuleFor(x => x.Request.Amount).GreaterThan(0);
        RuleFor(x => x.Request.Description).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Request.Type).IsInEnum();
    }
}

public sealed class CreateTransactionHandler(IApplicationDbContext context) : IRequestHandler<CreateTransactionCommand, Result<TransactionDto>>
{
    public async Task<Result<TransactionDto>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        if (!await context.Banks.AnyAsync(x => x.Id == request.BankId, cancellationToken)) return Result<TransactionDto>.Failure("Data Not Found");
        var transaction = new TransactionRecord(request.BankId, request.Type, request.Amount, request.Description, request.OccurredAt);
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync(cancellationToken);
        return Result<TransactionDto>.Success(new TransactionDto(transaction.Id, transaction.BankId, transaction.Type, transaction.Amount, transaction.Description, transaction.OccurredAt));
    }
}
