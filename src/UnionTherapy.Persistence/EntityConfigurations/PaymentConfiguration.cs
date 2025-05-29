using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        // Table name
        builder.ToTable("Payments");

        // Primary key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.SessionParticipationId)
            .IsRequired();

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("TRY");

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(PaymentStatus.Pending);

        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.PaymentServiceId)
            .HasMaxLength(100);

        builder.Property(p => p.PaymentServiceReference)
            .HasMaxLength(200);

        builder.Property(p => p.ErrorCode)
            .HasMaxLength(50);

        builder.Property(p => p.ErrorMessage)
            .HasMaxLength(500);

        builder.Property(p => p.RefundReason)
            .HasMaxLength(500);

        builder.Property(p => p.RefundAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(p => p.UserId)
            .HasDatabaseName("IX_Payments_UserId");

        builder.HasIndex(p => p.SessionParticipationId)
            .IsUnique()
            .HasDatabaseName("IX_Payments_SessionParticipationId");

        builder.HasIndex(p => p.Status)
            .HasDatabaseName("IX_Payments_Status");

        builder.HasIndex(p => p.PaymentServiceReference)
            .HasDatabaseName("IX_Payments_PaymentServiceReference");

        // Relationships
        builder.HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.SessionParticipation)
            .WithOne(sp => sp.Payment)
            .HasForeignKey<Payment>(p => p.SessionParticipationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
