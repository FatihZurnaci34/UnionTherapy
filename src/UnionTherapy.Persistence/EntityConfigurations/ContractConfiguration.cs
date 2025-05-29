using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        // Table name
        builder.ToTable("Contracts");

        // Primary key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(10000);

        builder.Property(c => c.Type)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Version)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("1.0");

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.Property(c => c.EffectiveDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(c => c.Type)
            .HasDatabaseName("IX_Contracts_Type");

        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_Contracts_IsActive");

        builder.HasIndex(c => c.EffectiveDate)
            .HasDatabaseName("IX_Contracts_EffectiveDate");

        // Relationships
        builder.HasMany(c => c.UserContracts)
            .WithOne(uc => uc.Contract)
            .HasForeignKey(uc => uc.ContractId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
