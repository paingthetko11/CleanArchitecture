using CleanArchitecture.Api.Contracts;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Services.Banks;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("api/banks")]
public sealed class BanksController(IBankService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] QueryParams query, CancellationToken cancellationToken)
    {
        var result = await service.GetListAsync(query, cancellationToken);
        return Ok(ApiResponse<PagedResult<BankDto>>.Ok(result.Data, 200, "Successfully Retrieved"));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return result.Succeeded
            ? Ok(ApiResponse<BankDto>.Ok(result.Data, 200, "Successfully Retrieved"))
            : NotFound(ApiResponse<BankDto>.Fail(404, "Data Not Found"));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBankRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        if (!result.Succeeded) return Conflict(ApiResponse<BankDto>.Fail(409, result.Error ?? "Conflict"));
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, ApiResponse<BankDto>.Ok(result.Data, 201, "Successfully Created"));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateBankRequest request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateAsync(id, request, cancellationToken);
        return result.Succeeded
            ? Ok(ApiResponse<BankDto>.Ok(result.Data, 200, "Successfully Updated"))
            : result.Error == "Data Not Found"
                ? NotFound(ApiResponse<BankDto>.Fail(404, "Data Not Found"))
                : Conflict(ApiResponse<BankDto>.Fail(409, result.Error ?? "Conflict"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return result.Succeeded
            ? Ok(ApiResponse<bool>.Ok(true, 200, "Successfully Deleted"))
            : NotFound(ApiResponse<bool>.Fail(404, "Data Not Found"));
    }
}
