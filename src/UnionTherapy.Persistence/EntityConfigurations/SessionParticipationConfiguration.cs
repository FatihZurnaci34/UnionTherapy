using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class SessionParticipationConfiguration : IEntityTypeConfiguration<SessionParticipation>
{
    public void Configure(EntityTypeBuilder<SessionParticipation> builder)
    {
        // Table name
        builder.ToTable("SessionParticipations");

        // Primary key
        builder.HasKey(sp => sp.Id);

        // Properties
        builder.Property(sp => sp.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(sp => sp.SessionId)
            .IsRequired();

        builder.Property(sp => sp.UserId)
            .IsRequired();

        builder.Property(sp => sp.IsAnonymous)
            .HasDefaultValue(false);

        builder.Property(sp => sp.AnonymousName)
            .HasMaxLength(100);

        builder.Property(sp => sp.ParticipationDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(sp => sp.HasAttended)
            .HasDefaultValue(false);

        builder.Property(sp => sp.Notes)
            .HasMaxLength(1000);

        builder.Property(sp => sp.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(sp => sp.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(sp => new { sp.SessionId, sp.UserId })
            .IsUnique()
            .HasDatabaseName("IX_SessionParticipations_SessionId_UserId");

        builder.HasIndex(sp => sp.SessionId)
            .HasDatabaseName("IX_SessionParticipations_SessionId");

        builder.HasIndex(sp => sp.UserId)
            .HasDatabaseName("IX_SessionParticipations_UserId");

        // Relationships
        builder.HasOne(sp => sp.Session)
            .WithMany(s => s.Participants)
            .HasForeignKey(sp => sp.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.User)
            .WithMany(u => u.SessionParticipations)
            .HasForeignKey(sp => sp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sp => sp.Payment)
            .WithOne(p => p.SessionParticipation)
            .HasForeignKey<Payment>(p => p.SessionParticipationId)
            .OnDelete(DeleteBehavior.SetNull);
    }
} 
