using Microsoft.EntityFrameworkCore;
using Startawy.Domain.Entities;
using startawy.Core.Entities;

namespace Startawy.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Consultant> Consultants => Set<Consultant>();
    public DbSet<StartupFounder> StartupFounders => Set<StartupFounder>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<ChatBot> ChatBots => Set<ChatBot>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<StartawyReport> StartawyReports => Set<StartawyReport>();
    public DbSet<Basic> Basics => Set<Basic>();
    public DbSet<Premium> Premiums => Set<Premium>();
    public DbSet<Free> Frees => Set<Free>();

    public DbSet<BudgetAnalysis> BudgetAnalyses => Set<BudgetAnalysis>();
    public DbSet<BudgetLineItem> BudgetLineItems => Set<BudgetLineItem>();
    public DbSet<ConsultationRequest> ConsultationRequests => Set<ConsultationRequest>();
    public DbSet<CashFlowForecast> CashFlowForecasts => Set<CashFlowForecast>();
    public DbSet<MonthlyForecast> MonthlyForecasts => Set<MonthlyForecast>();
    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<FinancialStatement> FinancialStatements => Set<FinancialStatement>();
    public DbSet<MarketResearch> MarketResearches => Set<MarketResearch>();
    public DbSet<Competitor> Competitors => Set<Competitor>();
    public DbSet<MarketTrend> MarketTrends => Set<MarketTrend>();
    public DbSet<MarketingCampaign> MarketingCampaigns => Set<MarketingCampaign>();
    public DbSet<DashboardSnapshot> DashboardSnapshots => Set<DashboardSnapshot>();
    public DbSet<ConsultantAvailabilitySlot> ConsultantAvailabilitySlots => Set<ConsultantAvailabilitySlot>();
    public DbSet<ConsultationSession> ConsultationSessions => Set<ConsultationSession>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<FollowUpPlan> FollowUpPlans => Set<FollowUpPlan>();
    public DbSet<FollowUpStep> FollowUpSteps => Set<FollowUpStep>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasOne(u => u.Admin).WithOne(a => a.User)
                  .HasForeignKey<Admin>(a => a.UserId);
            entity.HasOne(u => u.Consultant).WithOne(c => c.User)
                  .HasForeignKey<Consultant>(c => c.UserId);
            entity.HasOne(u => u.StartupFounder).WithOne(sf => sf.User)
                  .HasForeignKey<StartupFounder>(sf => sf.UserId);
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("admin");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AdminLevel).HasColumnName("admin_level");
            entity.Property(e => e.AccessScope).HasColumnName("access_scope");
        });

        modelBuilder.Entity<Consultant>(entity =>
        {
            entity.ToTable("consultant");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.YearsOfExp).HasColumnName("years_of_exp");
            entity.Property(e => e.Certificate).HasColumnName("certificate");
            entity.Property(e => e.Specialization).HasColumnName("specialization");
            entity.Property(e => e.Availability).HasColumnName("availability");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.SessionRate).HasColumnName("session_rate").HasPrecision(18, 2);
        });

        modelBuilder.Entity<StartupFounder>(entity =>
        {
            entity.ToTable("startup_founder");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.BusinessName).HasColumnName("business_name");
            entity.Property(e => e.BusinessSector).HasColumnName("business_sector");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FoundingDate).HasColumnName("founding_date");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.ToTable("session");
            entity.HasKey(e => e.SessionId);
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.FounderId).HasColumnName("founder_id");
            entity.Property(e => e.ConsultantId).HasColumnName("consultant_id");
            entity.HasOne(s => s.Founder).WithMany(f => f.Sessions).HasForeignKey(s => s.FounderId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(s => s.Consultant).WithMany(c => c.Sessions).HasForeignKey(s => s.ConsultantId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("package");
            entity.HasKey(e => e.PackageId);
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Price).HasColumnName("price").HasPrecision(18, 2);
            entity.Property(e => e.Duration).HasColumnName("duration");
        });

        modelBuilder.Entity<Basic>(entity =>
        {
            entity.ToTable("basic");
            entity.HasKey(e => e.PackageId);
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.UnlimitedAi).HasColumnName("unlimited_ai");
            entity.Property(e => e.UnlimitedAnalysis).HasColumnName("unlimited_analysis");
        });

        modelBuilder.Entity<Premium>(entity =>
        {
            entity.ToTable("premium");
            entity.HasKey(e => e.PackageId);
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.ConsultantReview).HasColumnName("consultant_review");
            entity.Property(e => e.ConsultantSupport).HasColumnName("consultant_support");
            entity.Property(e => e.FollowUpDuration).HasColumnName("follow_up_duration");
        });

        modelBuilder.Entity<Free>(entity =>
        {
            entity.ToTable("free");
            entity.HasKey(e => e.PackageId);
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.FreeTrial).HasColumnName("free_trial");
            entity.HasOne(f => f.Package).WithOne(p => p.Free)
                  .HasForeignKey<Free>(f => f.PackageId);
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("subscription");
            entity.HasKey(e => e.SubsId);
            entity.Property(e => e.SubsId).HasColumnName("subs_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TrialType).HasColumnName("trial_type");
            entity.HasOne(s => s.User).WithMany(u => u.Subscriptions).HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(s => s.Package).WithMany().HasForeignKey(s => s.PackageId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("transaction");
            entity.HasKey(e => e.TransId);
            entity.Property(e => e.TransId).HasColumnName("trans_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.SubsId).HasColumnName("subs_id");
            entity.Property(e => e.Amount).HasColumnName("amount").HasPrecision(18, 2);
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.TransDate).HasColumnName("trans_date");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20);
            entity.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key").HasMaxLength(100);
            entity.Property(e => e.ExternalReference).HasColumnName("external_reference").HasMaxLength(200);
            entity.Property(e => e.ReferenceType).HasColumnName("reference_type").HasMaxLength(50);
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id").HasMaxLength(100);

            entity.HasIndex(e => new { e.UserId, e.IdempotencyKey })
                .HasDatabaseName("IX_transaction_user_idempotency")
                .IsUnique()
                .HasFilter("[idempotency_key] IS NOT NULL");
        });

        modelBuilder.Entity<ChatBot>(entity =>
        {
            entity.ToTable("chat_bot");
            entity.HasKey(e => e.ChatId);
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserMessage).HasColumnName("user_message");
            entity.Property(e => e.SysResponse).HasColumnName("sys_response");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.ChatLimit).HasColumnName("chat_limit");
        });

        modelBuilder.Entity<StartawyReport>(entity =>
        {
            entity.ToTable("startawy_reports");
            entity.HasKey(e => e.ReportId);
            entity.Property(e => e.ReportId).HasColumnName("report_id");
            entity.Property(e => e.FounderId).HasColumnName("founder_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Industry).HasColumnName("industry");
            entity.Property(e => e.Link).HasColumnName("link");
            entity.Property(e => e.UploadDate).HasColumnName("upload_date");
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<BudgetAnalysis>().HasOne<Package>().WithMany(p => p.BudgetAnalyses).HasForeignKey(b => b.PackageId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
    }
}