using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.Services.Banks;

public sealed record BankDto(Guid Id, string Code, string Name, bool IsActive);
public sealed record CreateBankRequest([property: Required, StringLength(20)] string Code, [property: Required, StringLength(150)] string Name);
public sealed record UpdateBankRequest([property: Required, StringLength(20)] string Code, [property: Required, StringLength(150)] string Name, bool IsActive);
