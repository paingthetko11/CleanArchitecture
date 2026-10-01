using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations;

public sealed class TransactionConfiguration : IEntityTypeConfiguration<TransactionRecord>
{
    public void Configure(EntityTypeBuilder<TransactionRecord> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.Type).HasConversion<int>();
        builder.HasIndex(x => x.OccurredAt);
        builder.HasOne<Bank>().WithMany().HasForeignKey(x => x.BankId).OnDelete(DeleteBehavior.Restrict);
    }
}
