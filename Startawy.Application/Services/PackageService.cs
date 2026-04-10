using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Startawy.Application.Common;
using Startawy.Application.DTOs.Package;
using Startawy.Application.Interfaces;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;

namespace Startawy.Application.Services;

public class PackageService : IPackageService
{
    private readonly IPackageRepository _repo;

    public PackageService(IPackageRepository repo)
    {
        _repo = repo;
    }

    public async Task<ApiResponse<IEnumerable<PackageResponse>>> GetAllAsync()
    {
        var packages = await _repo.GetAllAsync();
        var dto = packages.Select(MapToResponse);
        return ApiResponse<IEnumerable<PackageResponse>>.Ok(dto);
    }

    public async Task<ApiResponse<PackageResponse>> GetByIdAsync(string packageId)
    {
        var p = await _repo.GetByIdAsync(packageId);
        if (p is null) return ApiResponse<PackageResponse>.Fail("Package not found.");
        return ApiResponse<PackageResponse>.Ok(MapToResponse(p));
    }

    public async Task<ApiResponse<PackageResponse>> CreateAsync(CreatePackageRequest request)
    {
        var package = new Package
        {
            PackageId = Guid.NewGuid().ToString(),
            Type = request.Type,
            Description = request.Description,
            Price = request.Price,
            Duration = request.DurationDays
        };

        if (request.UnlimitedAi.HasValue || request.UnlimitedAnalysis.HasValue)
        {
            package.Basic = new Basic
            {
                PackageId = package.PackageId,
                UnlimitedAi = request.UnlimitedAi,
                UnlimitedAnalysis = request.UnlimitedAnalysis
            };
        }

        if (request.FollowUpDurationDays.HasValue || request.ConsultantReview.HasValue || request.ConsultantSupport.HasValue)
        {
            package.Premium = new Premium
            {
                PackageId = package.PackageId,
                FollowUpDuration = request.FollowUpDurationDays,
                ConsultantReview = request.ConsultantReview,
                ConsultantSupport = request.ConsultantSupport
            };
        }

        var created = await _repo.CreateAsync(package);
        return ApiResponse<PackageResponse>.Ok(MapToResponse(created), "Package created.");
    }

    public async Task<ApiResponse<PackageResponse>> UpdateAsync(string packageId, UpdatePackageRequest request)
    {
        var existing = await _repo.GetByIdAsync(packageId);
        if (existing is null) return ApiResponse<PackageResponse>.Fail("Package not found.");

        existing.Type = request.Type;
        existing.Description = request.Description;
        existing.Price = request.Price;
        existing.Duration = request.DurationDays;

        if (existing.Basic is null && (request.UnlimitedAi.HasValue || request.UnlimitedAnalysis.HasValue))
            existing.Basic = new Basic { PackageId = existing.PackageId };

        if (existing.Basic is not null)
        {
            existing.Basic.UnlimitedAi = request.UnlimitedAi;
            existing.Basic.UnlimitedAnalysis = request.UnlimitedAnalysis;
        }

        if (existing.Premium is null && (request.FollowUpDurationDays.HasValue || request.ConsultantReview.HasValue || request.ConsultantSupport.HasValue))
            existing.Premium = new Premium { PackageId = existing.PackageId };

        if (existing.Premium is not null)
        {
            existing.Premium.FollowUpDuration = request.FollowUpDurationDays;
            existing.Premium.ConsultantReview = request.ConsultantReview;
            existing.Premium.ConsultantSupport = request.ConsultantSupport;
        }

        var updated = await _repo.UpdateAsync(existing);
        return ApiResponse<PackageResponse>.Ok(MapToResponse(updated), "Package updated.");
    }

    public async Task<ApiResponse<object>> DeleteAsync(string packageId)
    {
        var existing = await _repo.GetByIdAsync(packageId);
        if (existing is null) return ApiResponse<object>.Fail("Package not found.");
        await _repo.DeleteAsync(packageId);
        return ApiResponse<object>.Ok(null!, "Package deleted.");
    }

    private static PackageResponse MapToResponse(Package p)
        => new()
        {
            PackageId = p.PackageId,
            Type = p.Type,
            Description = p.Description,
            Price = p.Price,
            DurationDays = p.Duration,
            UnlimitedAi = p.Basic?.UnlimitedAi,
            UnlimitedAnalysis = p.Basic?.UnlimitedAnalysis,
            FollowUpDurationDays = p.Premium?.FollowUpDuration,
            ConsultantReview = p.Premium?.ConsultantReview,
            ConsultantSupport = p.Premium?.ConsultantSupport
        };
}