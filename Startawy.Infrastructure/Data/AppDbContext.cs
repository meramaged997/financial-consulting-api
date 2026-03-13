using Microsoft.EntityFrameworkCore;
using Startawy.Domain.Entities;

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
    public DbSet<BudgetAnalysis> BudgetAnalyses => Set<BudgetAnalysis>();
    public DbSet<Basic> Basics => Set<Basic>();
    public DbSet<Premium> Premiums => Set<Premium>();
    public DbSet<Free> Frees => Set<Free>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.UserId).IsRequired();
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Password).IsRequired().HasMaxLength(256);
            entity.Property(u => u.Phone).HasMaxLength(20);
            entity.Property(u => u.Type).IsRequired().HasMaxLength(50);

            entity.HasOne(u => u.Admin)
                  .WithOne(a => a.User)
                  .HasForeignKey<Admin>(a => a.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.Consultant)
                  .WithOne(c => c.User)
                  .HasForeignKey<Consultant>(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.StartupFounder)
                  .WithOne(sf => sf.User)
                  .HasForeignKey<StartupFounder>(sf => sf.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(p => p.PackageId);
            entity.Property(p => p.PackageId).IsRequired();
            entity.Property(p => p.Type).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Description).HasMaxLength(1000);
        });

        modelBuilder.Entity<Free>(entity =>
        {
            entity.HasKey(e => e.PackageId);
            entity.HasOne(d => d.Package)
                  .WithOne(p => p.Free)
                  .HasForeignKey<Free>(d => d.PackageId);
        });
    }
}
