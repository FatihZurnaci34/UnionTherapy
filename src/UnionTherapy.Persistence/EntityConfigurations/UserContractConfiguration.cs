using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class UserContractConfiguration : IEntityTypeConfiguration<UserContract>
{
    public void Configure(EntityTypeBuilder<UserContract> builder)
    {
        // Table name
        builder.ToTable("UserContracts");

        // Primary key
        builder.HasKey(uc => uc.Id);

        // Properties
        builder.Property(uc => uc.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(uc => uc.UserId)
            .IsRequired();

        builder.Property(uc => uc.ContractId)
            .IsRequired();

        builder.Property(uc => uc.IsAccepted)
            .HasDefaultValue(false);

        builder.Property(uc => uc.AcceptanceDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(uc => uc.IpAddress)
            .IsRequired()
            .HasMaxLength(45); // IPv6 support

        builder.Property(uc => uc.UserAgent)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(uc => uc.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(uc => uc.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(uc => new { uc.UserId, uc.ContractId })
            .IsUnique()
            .HasDatabaseName("IX_UserContracts_UserId_ContractId");

        builder.HasIndex(uc => uc.UserId)
            .HasDatabaseName("IX_UserContracts_UserId");

        builder.HasIndex(uc => uc.ContractId)
            .HasDatabaseName("IX_UserContracts_ContractId");

        builder.HasIndex(uc => uc.IsAccepted)
            .HasDatabaseName("IX_UserContracts_IsAccepted");

        // Relationships
        builder.HasOne(uc => uc.User)
            .WithMany(u => u.UserContracts)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uc => uc.Contract)
            .WithMany(c => c.UserContracts)
            .HasForeignKey(uc => uc.ContractId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
