using Microsoft.EntityFrameworkCore;
using UnionTherapy.Domain.Entities;

namespace UnionTherapy.Persistence.Context;

public class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
    {
    }

    // DbSet'ler
    public DbSet<User> Users { get; set; }
    public DbSet<Psychologist> Psychologists { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SessionParticipation> SessionParticipations { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<PsychologistReview> PsychologistReviews { get; set; }
    public DbSet<PsychologistSpecialization> PsychologistSpecializations { get; set; }
    public DbSet<PsychologistDocument> PsychologistDocuments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserAgreement> UserAgreements { get; set; }
    public DbSet<UserAgreementAcceptance> UserAgreementAcceptances { get; set; }
    public DbSet<PsychologistContract> PsychologistContracts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
         
        // PostgreSQL schema ayarı
        modelBuilder.HasDefaultSchema("dev_schema");
          
        // Entity konfigürasyonlarını assembly'den uygula
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Migration history table'ı doğru schema'da oluştur ve timeout ayarla
        if (optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(options =>
            {
                options.MigrationsHistoryTable("__EFMigrationsHistory", "dev_schema");
                options.CommandTimeout(30); // 30 saniye timeout
            });
        }
    }
} 