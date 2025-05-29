using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class PsychologistSpecializationConfiguration : IEntityTypeConfiguration<PsychologistSpecialization>
{
    public void Configure(EntityTypeBuilder<PsychologistSpecialization> builder)
    {
        // Table name
        builder.ToTable("PsychologistSpecializations");

        // Primary key
        builder.HasKey(ps => ps.Id);

        // Properties
        builder.Property(ps => ps.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(ps => ps.PsychologistId)
            .IsRequired();

        builder.Property(ps => ps.SpecializationArea)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(ps => ps.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(ps => ps.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(ps => new { ps.PsychologistId, ps.SpecializationArea })
            .IsUnique()
            .HasDatabaseName("IX_PsychologistSpecializations_PsychologistId_SpecializationArea");

        builder.HasIndex(ps => ps.PsychologistId)
            .HasDatabaseName("IX_PsychologistSpecializations_PsychologistId");

        builder.HasIndex(ps => ps.SpecializationArea)
            .HasDatabaseName("IX_PsychologistSpecializations_SpecializationArea");

        // Relationships
        builder.HasOne(ps => ps.Psychologist)
            .WithMany(p => p.Specializations)
            .HasForeignKey(ps => ps.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
