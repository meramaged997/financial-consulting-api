using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;
using Startawy.Infrastructure.Data;

namespace Startawy.Infrastructure.Repositories;

public class PackageRepository : IPackageRepository
{
    private readonly AppDbContext _context;

    public PackageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Package>> GetAllAsync()
        => await _context.Packages
            .AsNoTracking()
            .Include(p => p.Basic)
            .Include(p => p.Premium)
            .ToListAsync();

    public async Task<Package?> GetByIdAsync(string packageId)
        => await _context.Packages
            .Include(p => p.Basic)
            .Include(p => p.Premium)
            .FirstOrDefaultAsync(p => p.PackageId == packageId);

    public async Task<Package?> GetByTypeAsync(string type)
        => await _context.Packages
            .Include(p => p.Basic)
            .Include(p => p.Premium)
            .FirstOrDefaultAsync(p => p.Type == type);

    public async Task<Package> CreateAsync(Package package)
    {
        _context.Packages.Add(package);
        await _context.SaveChangesAsync();
        return package;
    }

    public async Task<Package> UpdateAsync(Package package)
    {
        _context.Packages.Update(package);
        await _context.SaveChangesAsync();
        return package;
    }

    public async Task DeleteAsync(string packageId)
    {
        var existing = await _context.Packages.FindAsync(packageId);
        if (existing is null) return;
        _context.Packages.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
