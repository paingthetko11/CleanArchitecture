using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public sealed class Bank : AuditableEntity, ISoftDelete
{
    private Bank() { }

    public Bank(string code, string name)
    {
        Id = Guid.NewGuid();
        SetDetails(code, name);
        IsActive = true;
    }

    public Guid Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; set; }

    public void Update(string code, string name, bool isActive) { SetDetails(code, name); IsActive = isActive; }

    private void SetDetails(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Bank code is required.", nameof(code));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Bank name is required.", nameof(name));
        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
    }
}
