using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Persistence.EntityConfigurations;

public class PsychologistDocumentConfiguration : IEntityTypeConfiguration<PsychologistDocument>
{
    public void Configure(EntityTypeBuilder<PsychologistDocument> builder)
    {
        // Table name
        builder.ToTable("PsychologistDocuments");

        // Primary key
        builder.HasKey(pd => pd.Id);

        // Properties
        builder.Property(pd => pd.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(pd => pd.PsychologistId)
            .IsRequired();

        builder.Property(pd => pd.DocumentName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(pd => pd.DocumentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pd => pd.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(pd => pd.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(pd => pd.FileSize)
            .IsRequired();

        builder.Property(pd => pd.MimeType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pd => pd.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(DocumentStatus.Pending);

        builder.Property(pd => pd.ApprovalNote)
            .HasMaxLength(500);

        builder.Property(pd => pd.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(pd => pd.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        // Indexes
        builder.HasIndex(pd => pd.PsychologistId)
            .HasDatabaseName("IX_PsychologistDocuments_PsychologistId");

        builder.HasIndex(pd => pd.Status)
            .HasDatabaseName("IX_PsychologistDocuments_Status");

        builder.HasIndex(pd => pd.DocumentType)
            .HasDatabaseName("IX_PsychologistDocuments_DocumentType");

        // Relationships
        builder.HasOne(pd => pd.Psychologist)
            .WithMany(p => p.Documents)
            .HasForeignKey(pd => pd.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 
