namespace CleanArchitecture.Application.Common.Models;

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int Take, int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)Take);
}
