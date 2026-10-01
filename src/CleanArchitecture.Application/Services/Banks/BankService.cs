using CleanArchitecture.Application.Common.Extensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Services.Banks;

public sealed class BankService(IApplicationDbContext context) : IBankService
{
    public async Task<Result<PagedResult<BankDto>>> GetListAsync(QueryParams query, CancellationToken cancellationToken)
    {
        var banks = context.Banks.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            banks = banks.Where(x => x.Code.Contains(search) || x.Name.Contains(search));
        }
        var projected = banks.OrderBy(x => x.Name).Select(x => new BankDto(x.Id, x.Code, x.Name, x.IsActive));
        return Result<PagedResult<BankDto>>.Success(await projected.ToPagedResultAsync(query, cancellationToken));
    }

    public async Task<Result<BankDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var bank = await context.Banks.AsNoTracking().Where(x => x.Id == id)
            .Select(x => new BankDto(x.Id, x.Code, x.Name, x.IsActive)).SingleOrDefaultAsync(cancellationToken);
        return bank is null ? Result<BankDto>.Failure("Data Not Found") : Result<BankDto>.Success(bank);
    }

    public async Task<Result<BankDto>> CreateAsync(CreateBankRequest request, CancellationToken cancellationToken)
    {
        var code = request.Code.Trim().ToUpperInvariant();
        if (await context.Banks.AnyAsync(x => x.Code == code, cancellationToken)) return Result<BankDto>.Failure("Bank code already exists.");
        var bank = new Bank(code, request.Name);
        context.Banks.Add(bank);
        await context.SaveChangesAsync(cancellationToken);
        return Result<BankDto>.Success(new BankDto(bank.Id, bank.Code, bank.Name, bank.IsActive));
    }

    public async Task<Result<BankDto>> UpdateAsync(Guid id, UpdateBankRequest request, CancellationToken cancellationToken)
    {
        var bank = await context.Banks.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (bank is null) return Result<BankDto>.Failure("Data Not Found");
        var code = request.Code.Trim().ToUpperInvariant();
        if (await context.Banks.AnyAsync(x => x.Id != id && x.Code == code, cancellationToken)) return Result<BankDto>.Failure("Bank code already exists.");
        bank.Update(code, request.Name, request.IsActive);
        await context.SaveChangesAsync(cancellationToken);
        return Result<BankDto>.Success(new BankDto(bank.Id, bank.Code, bank.Name, bank.IsActive));
    }

    public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var bank = await context.Banks.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (bank is null) return Result<bool>.Failure("Data Not Found");
        bank.IsDeleted = true;
        await context.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
