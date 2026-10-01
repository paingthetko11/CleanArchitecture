using CleanArchitecture.Api.Contracts;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Features.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("api/transactions")]
public sealed class TransactionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] QueryParams query, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetTransactionsQuery(query), cancellationToken);
        return Ok(ApiResponse<PagedResult<TransactionDto>>.Ok(result.Data, 200, "Successfully Retrieved"));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateTransactionCommand(request), cancellationToken);
        if (!result.Succeeded) return NotFound(ApiResponse<TransactionDto>.Fail(404, "Data Not Found"));
        return CreatedAtAction(nameof(GetList), ApiResponse<TransactionDto>.Ok(result.Data, 201, "Successfully Created"));
    }
}
