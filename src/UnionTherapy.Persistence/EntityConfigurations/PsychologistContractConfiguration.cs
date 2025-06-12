using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class PsychologistContractConfiguration : IEntityTypeConfiguration<PsychologistContract>
{
    public void Configure(EntityTypeBuilder<PsychologistContract> builder)
    {
        // Table name
        builder.ToTable("PsychologistContracts");

        // Primary key
        builder.HasKey(pc => pc.Id);

        // Properties
        builder.Property(pc => pc.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(pc => pc.PsychologistId)
            .IsRequired();

        builder.Property(pc => pc.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(pc => pc.Content)
            .IsRequired()
            .HasMaxLength(10000);

        builder.Property(pc => pc.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(pc => pc.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(PsychologistContractStatus.Draft);

        builder.Property(pc => pc.Version)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("1.0");

        builder.Property(pc => pc.EffectiveDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(pc => pc.CommissionRate)
            .HasColumnType("decimal(5,2)"); // 0.00 - 100.00

        builder.Property(pc => pc.MonthlyFee)
            .HasColumnType("decimal(18,2)");

        builder.Property(pc => pc.SpecialTerms)
            .HasMaxLength(2000);

        builder.Property(pc => pc.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(pc => pc.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(pc => pc.PsychologistId)
            .HasDatabaseName("IX_PsychologistContracts_PsychologistId");

        builder.HasIndex(pc => pc.Type)
            .HasDatabaseName("IX_PsychologistContracts_Type");

        builder.HasIndex(pc => pc.Status)
            .HasDatabaseName("IX_PsychologistContracts_Status");

        builder.HasIndex(pc => pc.EffectiveDate)
            .HasDatabaseName("IX_PsychologistContracts_EffectiveDate");

        // Relationships
        builder.HasOne(pc => pc.Psychologist)
            .WithMany(p => p.Contracts)
            .HasForeignKey(pc => pc.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 