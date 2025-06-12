using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class PsychologistReviewConfiguration : IEntityTypeConfiguration<PsychologistReview>
{
    public void Configure(EntityTypeBuilder<PsychologistReview> builder)
    {
        // Table name
        builder.ToTable("PsychologistReviews", t => t.HasCheckConstraint("CK_PsychologistReviews_Rating", "\"Rating\" >= 1 AND \"Rating\" <= 5"));

        // Primary key
        builder.HasKey(pr => pr.Id);

        // Properties
        builder.Property(pr => pr.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(pr => pr.PsychologistId)
            .IsRequired();

        builder.Property(pr => pr.UserId)
            .IsRequired();

        builder.Property(pr => pr.SessionId)
            .IsRequired();

        builder.Property(pr => pr.Rating)
            .IsRequired();

        builder.Property(pr => pr.Comment)
            .HasMaxLength(1000);

        builder.Property(pr => pr.IsAnonymous)
            .HasDefaultValue(false);

        builder.Property(pr => pr.IsApproved)
            .HasDefaultValue(false);

        builder.Property(pr => pr.ApprovalNote)
            .HasMaxLength(500);

        builder.Property(pr => pr.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(pr => pr.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(pr => new { pr.PsychologistId, pr.UserId, pr.SessionId })
            .IsUnique()
            .HasDatabaseName("IX_PsychologistReviews_PsychologistId_UserId_SessionId");

        builder.HasIndex(pr => pr.PsychologistId)
            .HasDatabaseName("IX_PsychologistReviews_PsychologistId");

        builder.HasIndex(pr => pr.UserId)
            .HasDatabaseName("IX_PsychologistReviews_UserId");

        builder.HasIndex(pr => pr.SessionId)
            .HasDatabaseName("IX_PsychologistReviews_SessionId");

        builder.HasIndex(pr => pr.IsApproved)
            .HasDatabaseName("IX_PsychologistReviews_IsApproved");

        // Relationships
        builder.HasOne(pr => pr.Psychologist)
            .WithMany(p => p.Reviews)
            .HasForeignKey(pr => pr.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pr => pr.User)
            .WithMany(u => u.PsychologistReviews)
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(pr => pr.Session)
            .WithMany(s => s.PsychologistReviews)
            .HasForeignKey(pr => pr.SessionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
} 
