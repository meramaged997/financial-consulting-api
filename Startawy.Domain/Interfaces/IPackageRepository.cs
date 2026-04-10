using System.Collections.Generic;
using System.Threading.Tasks;
using Startawy.Domain.Entities;

namespace Startawy.Domain.Interfaces;

public interface IPackageRepository
{
    Task<IEnumerable<Package>> GetAllAsync();
    Task<Package?> GetByIdAsync(string packageId);
    Task<Package?> GetByTypeAsync(string type);
    Task<Package> CreateAsync(Package package);
    Task<Package> UpdateAsync(Package package);
    Task DeleteAsync(string packageId);
}