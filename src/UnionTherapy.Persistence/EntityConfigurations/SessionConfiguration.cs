using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        // Table name
        builder.ToTable("Sessions");

        // Primary key
        builder.HasKey(s => s.Id);

        // Properties
        builder.Property(s => s.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(s => s.Objective)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(s => s.SpecializationArea)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(s => s.PsychologistId)
            .IsRequired();

        builder.Property(s => s.StartDate)
            .IsRequired();

        builder.Property(s => s.EndDate)
            .IsRequired();

        builder.Property(s => s.MaxParticipants)
            .IsRequired();

        builder.Property(s => s.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(s => s.AnonymousParticipationAvailable)
            .HasDefaultValue(true);

        builder.Property(s => s.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(SessionStatus.Planned);

        builder.Property(s => s.ZoomMeetingId)
            .HasMaxLength(100);

        builder.Property(s => s.ZoomJoinUrl)
            .HasMaxLength(500);

        builder.Property(s => s.ZoomPassword)
            .HasMaxLength(50);

        builder.Property(s => s.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(s => s.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(s => s.PsychologistId)
            .HasDatabaseName("IX_Sessions_PsychologistId");

        builder.HasIndex(s => s.StartDate)
            .HasDatabaseName("IX_Sessions_StartDate");

        builder.HasIndex(s => s.Status)
            .HasDatabaseName("IX_Sessions_Status");

        builder.HasIndex(s => s.SpecializationArea)
            .HasDatabaseName("IX_Sessions_SpecializationArea");

        // Relationships
        builder.HasOne(s => s.Psychologist)
            .WithMany(p => p.Sessions)
            .HasForeignKey(s => s.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Participants)
            .WithOne(sp => sp.Session)
            .HasForeignKey(sp => sp.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Reviews)
            .WithOne(r => r.Session)
            .HasForeignKey(r => r.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.PsychologistReviews)
            .WithOne(pr => pr.Session)
            .HasForeignKey(pr => pr.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
