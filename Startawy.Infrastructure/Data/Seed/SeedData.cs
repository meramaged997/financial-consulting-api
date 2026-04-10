using Microsoft.EntityFrameworkCore;
using Startawy.Domain.Entities;
using Startawy.Infrastructure.Data;

namespace Startawy.Infrastructure.Data.Seed;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        // Packages (required for auth/subscriptions logic)
        if (!await db.Packages.AnyAsync(ct))
        {
            var freeId = Guid.NewGuid().ToString();
            var basicId = Guid.NewGuid().ToString();
            var premiumId = Guid.NewGuid().ToString();

            db.Packages.AddRange(
                new Package
                {
                    PackageId = freeId,
                    Type = "Free",
                    Description = "Free trial plan with limited AI usage and basic access.",
                    Price = 0m,
                    Duration = null,
                    Free = new Free
                    {
                        PackageId = freeId,
                        FreeTrial = 1
                    }
                },
                new Package
                {
                    PackageId = basicId,
                    Type = "Basic",
                    Description = "Basic plan with unlimited AI and enhanced tools.",
                    Price = 299m,
                    Duration = 30,
                    Basic = new Basic
                    {
                        PackageId = basicId,
                        UnlimitedAi = true,
                        UnlimitedAnalysis = true
                    }
                },
                new Package
                {
                    PackageId = premiumId,
                    Type = "Premium",
                    Description = "Premium plan with full access, consultant follow-up, and advanced support.",
                    Price = 599m,
                    Duration = 30,
                    Premium = new Premium
                    {
                        PackageId = premiumId,
                        FollowUpDuration = 30,
                        ConsultantReview = true,
                        ConsultantSupport = true
                    }
                }
            );

            await db.SaveChangesAsync(ct);
        }
    }
}

