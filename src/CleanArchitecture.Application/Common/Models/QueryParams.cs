namespace CleanArchitecture.Application.Common.Models;

public sealed class QueryParams
{
    private const int MaximumTake = 100;
    private int _page = 1;
    private int _take = 20;

    public int Page { get => _page; set => _page = Math.Max(1, value); }
    public int Take { get => _take; set => _take = Math.Clamp(value, 1, MaximumTake); }
    public string? Search { get; set; }
}
