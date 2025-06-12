using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class UserAgreementConfiguration : IEntityTypeConfiguration<UserAgreement>
{
    public void Configure(EntityTypeBuilder<UserAgreement> builder)
    {
        // Table name
        builder.ToTable("UserAgreements");

        // Primary key
        builder.HasKey(ua => ua.Id);

        // Properties
        builder.Property(ua => ua.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(ua => ua.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ua => ua.Content)
            .IsRequired()
            .HasMaxLength(10000);

        builder.Property(ua => ua.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(ua => ua.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(UserAgreementStatus.Draft);

        builder.Property(ua => ua.Version)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("1.0");

        builder.Property(ua => ua.EffectiveDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(ua => ua.IsRequired)
            .HasDefaultValue(true);

        builder.Property(ua => ua.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(ua => ua.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(ua => ua.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(ua => ua.Type)
            .HasDatabaseName("IX_UserAgreements_Type");

        builder.HasIndex(ua => ua.Status)
            .HasDatabaseName("IX_UserAgreements_Status");

        builder.HasIndex(ua => ua.EffectiveDate)
            .HasDatabaseName("IX_UserAgreements_EffectiveDate");

        builder.HasIndex(ua => ua.IsRequired)
            .HasDatabaseName("IX_UserAgreements_IsRequired");
    }
} 