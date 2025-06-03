using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;

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

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(10000);

        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(ContractStatus.Draft);

        builder.Property(c => c.Version)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("1.0");

        builder.Property(c => c.EffectiveDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(c => c.ExpiryDate)
            .IsRequired(false);

        builder.Property(c => c.PsychologistId)
            .IsRequired(false);

        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(c => c.Type)
            .HasDatabaseName("IX_Contracts_Type");

        builder.HasIndex(c => c.Status)
            .HasDatabaseName("IX_Contracts_Status");

        builder.HasIndex(c => c.EffectiveDate)
            .HasDatabaseName("IX_Contracts_EffectiveDate");

        // Relationships
        builder.HasOne(c => c.Psychologist)
            .WithMany()
            .HasForeignKey(c => c.PsychologistId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.UserContracts)
            .WithOne(uc => uc.Contract)
            .HasForeignKey(uc => uc.ContractId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
