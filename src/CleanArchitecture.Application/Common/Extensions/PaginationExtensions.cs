using CleanArchitecture.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Extensions;

public static class PaginationExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, QueryParams parameters, CancellationToken cancellationToken)
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((parameters.Page - 1) * parameters.Take).Take(parameters.Take).ToListAsync(cancellationToken);
        return new PagedResult<T>(items, parameters.Page, parameters.Take, totalCount);
    }
}
