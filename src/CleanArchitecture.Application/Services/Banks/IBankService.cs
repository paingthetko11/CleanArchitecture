using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.Services.Banks;

public interface IBankService
{
    Task<Result<PagedResult<BankDto>>> GetListAsync(QueryParams query, CancellationToken cancellationToken);
    Task<Result<BankDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<BankDto>> CreateAsync(CreateBankRequest request, CancellationToken cancellationToken);
    Task<Result<BankDto>> UpdateAsync(Guid id, UpdateBankRequest request, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
