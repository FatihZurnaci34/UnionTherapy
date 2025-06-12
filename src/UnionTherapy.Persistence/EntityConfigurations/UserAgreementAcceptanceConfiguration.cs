using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class UserAgreementAcceptanceConfiguration : IEntityTypeConfiguration<UserAgreementAcceptance>
{
    public void Configure(EntityTypeBuilder<UserAgreementAcceptance> builder)
    {
        // Table name
        builder.ToTable("UserAgreementAcceptances");

        // Primary key
        builder.HasKey(uaa => uaa.Id);

        // Properties
        builder.Property(uaa => uaa.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(uaa => uaa.UserId)
            .IsRequired();

        builder.Property(uaa => uaa.UserAgreementId)
            .IsRequired();

        builder.Property(uaa => uaa.IsAccepted)
            .HasDefaultValue(false);

        builder.Property(uaa => uaa.AcceptanceDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(uaa => uaa.IpAddress)
            .IsRequired()
            .HasMaxLength(45);

        builder.Property(uaa => uaa.UserAgent)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(uaa => uaa.AgreementVersion)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(uaa => uaa.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(uaa => uaa.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(uaa => new { uaa.UserId, uaa.UserAgreementId, uaa.AgreementVersion })
            .IsUnique()
            .HasDatabaseName("IX_UserAgreementAcceptances_UserId_AgreementId_Version");

        builder.HasIndex(uaa => uaa.UserId)
            .HasDatabaseName("IX_UserAgreementAcceptances_UserId");

        builder.HasIndex(uaa => uaa.UserAgreementId)
            .HasDatabaseName("IX_UserAgreementAcceptances_UserAgreementId");

        builder.HasIndex(uaa => uaa.IsAccepted)
            .HasDatabaseName("IX_UserAgreementAcceptances_IsAccepted");

        // Relationships
        builder.HasOne(uaa => uaa.User)
            .WithMany(u => u.AgreementAcceptances)
            .HasForeignKey(uaa => uaa.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uaa => uaa.UserAgreement)
            .WithMany(ua => ua.UserAcceptances)
            .HasForeignKey(uaa => uaa.UserAgreementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 