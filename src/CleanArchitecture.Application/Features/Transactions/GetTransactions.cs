using CleanArchitecture.Application.Common.Extensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Features.Transactions;

public sealed record GetTransactionsQuery(QueryParams Query) : IRequest<Result<PagedResult<TransactionDto>>>;

public sealed class GetTransactionsHandler(IApplicationDbContext context) : IRequestHandler<GetTransactionsQuery, Result<PagedResult<TransactionDto>>>
{
    public async Task<Result<PagedResult<TransactionDto>>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Transactions.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Query.Search))
        {
            var search = request.Query.Search.Trim();
            query = query.Where(x => x.Description.Contains(search));
        }
        var projected = query.OrderByDescending(x => x.OccurredAt)
            .Select(x => new TransactionDto(x.Id, x.BankId, x.Type, x.Amount, x.Description, x.OccurredAt));
        return Result<PagedResult<TransactionDto>>.Success(await projected.ToPagedResultAsync(request.Query, cancellationToken));
    }
}
