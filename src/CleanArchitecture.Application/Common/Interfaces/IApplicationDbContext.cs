using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Bank> Banks { get; }
    DbSet<TransactionRecord> Transactions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
