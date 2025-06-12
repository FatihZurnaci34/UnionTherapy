using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        // Table name
        builder.ToTable("Reviews", t => t.HasCheckConstraint("CK_Reviews_Rating", "\"Rating\" >= 1 AND \"Rating\" <= 5"));

        // Primary key
        builder.HasKey(r => r.Id);

        // Properties
        builder.Property(r => r.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(r => r.SessionId)
            .IsRequired();

        builder.Property(r => r.UserId)
            .IsRequired();

        builder.Property(r => r.Rating)
            .IsRequired();

        builder.Property(r => r.Comment)
            .HasMaxLength(1000);

        builder.Property(r => r.IsAnonymous)
            .HasDefaultValue(false);

        builder.Property(r => r.IsApproved)
            .HasDefaultValue(false);

        builder.Property(r => r.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(r => r.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(r => new { r.SessionId, r.UserId })
            .IsUnique()
            .HasDatabaseName("IX_Reviews_SessionId_UserId");

        builder.HasIndex(r => r.SessionId)
            .HasDatabaseName("IX_Reviews_SessionId");

        builder.HasIndex(r => r.UserId)
            .HasDatabaseName("IX_Reviews_UserId");

        builder.HasIndex(r => r.IsApproved)
            .HasDatabaseName("IX_Reviews_IsApproved");

        // Relationships
        builder.HasOne(r => r.Session)
            .WithMany(s => s.Reviews)
            .HasForeignKey(r => r.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
