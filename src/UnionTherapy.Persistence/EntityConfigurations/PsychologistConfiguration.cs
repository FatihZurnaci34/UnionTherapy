using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class PsychologistConfiguration : IEntityTypeConfiguration<Psychologist>
{
    public void Configure(EntityTypeBuilder<Psychologist> builder)
    {
        // Table name
        builder.ToTable("Psychologists");

        // Primary key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.LicenseNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.University)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Department)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.GraduationYear)
            .IsRequired();

        builder.Property(p => p.ExperienceYears)
            .IsRequired();

        builder.Property(p => p.Biography)
            .HasMaxLength(2000);

        builder.Property(p => p.Approach)
            .HasMaxLength(1000);

        builder.Property(p => p.IsApproved)
            .HasDefaultValue(false);

        builder.Property(p => p.ApprovalNote)
            .HasMaxLength(500);

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);

        builder.Property(p => p.HourlyRate)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.AverageRating)
            .HasDefaultValue(0.0)
            .HasColumnType("float");

        builder.Property(p => p.TotalReviews)
            .HasDefaultValue(0);

        builder.Property(p => p.TotalSessions)
            .HasDefaultValue(0);

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(p => p.LicenseNumber)
            .IsUnique()
            .HasDatabaseName("IX_Psychologists_LicenseNumber");

        builder.HasIndex(p => p.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Psychologists_UserId");

        builder.HasIndex(p => p.IsApproved)
            .HasDatabaseName("IX_Psychologists_IsApproved");

        // Relationships
        builder.HasOne(p => p.User)
            .WithOne(u => u.Psychologist)
            .HasForeignKey<Psychologist>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Specializations)
            .WithOne(ps => ps.Psychologist)
            .HasForeignKey(ps => ps.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Documents)
            .WithOne(pd => pd.Psychologist)
            .HasForeignKey(pd => pd.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Sessions)
            .WithOne(s => s.Psychologist)
            .HasForeignKey(s => s.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Reviews)
            .WithOne(pr => pr.Psychologist)
            .HasForeignKey(pr => pr.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
