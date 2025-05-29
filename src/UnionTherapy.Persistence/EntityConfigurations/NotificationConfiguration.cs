using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // Table name
        builder.ToTable("Notifications");

        // Primary key
        builder.HasKey(n => n.Id);

        // Properties
        builder.Property(n => n.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(n => n.UserId)
            .IsRequired();

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(n => n.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(n => n.IsRead)
            .HasDefaultValue(false);

        builder.Property(n => n.IsSent)
            .HasDefaultValue(false);

        builder.Property(n => n.ErrorMessage)
            .HasMaxLength(500);

        builder.Property(n => n.RetryCount)
            .HasDefaultValue(0);

        builder.Property(n => n.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(n => n.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(n => n.UserId)
            .HasDatabaseName("IX_Notifications_UserId");

        builder.HasIndex(n => n.IsRead)
            .HasDatabaseName("IX_Notifications_IsRead");

        builder.HasIndex(n => n.IsSent)
            .HasDatabaseName("IX_Notifications_IsSent");

        builder.HasIndex(n => n.Type)
            .HasDatabaseName("IX_Notifications_Type");

        builder.HasIndex(n => n.CreatedAt)
            .HasDatabaseName("IX_Notifications_CreatedAt");

        // Relationships
        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
