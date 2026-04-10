using Startawy.Application.Common;
using Startawy.Application.DTOs.Package;

namespace Startawy.Application.Interfaces;

public interface IPackageService
{
    Task<ApiResponse<IEnumerable<PackageResponse>>> GetAllAsync();
    Task<ApiResponse<PackageResponse>> GetByIdAsync(string packageId);
    Task<ApiResponse<PackageResponse>> CreateAsync(CreatePackageRequest request);
    Task<ApiResponse<PackageResponse>> UpdateAsync(string packageId, UpdatePackageRequest request);
    Task<ApiResponse<object>> DeleteAsync(string packageId);
}