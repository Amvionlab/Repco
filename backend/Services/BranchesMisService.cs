using LoginBackend.Services.Interfaces;
using LoginBackend.Data;
using LoginBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginBackend.Services;

public class BranchesMisService : IBranchesMisService
{
    private readonly ApplicationDbContext _context;

    public BranchesMisService(ApplicationDbContext context)
    {
        _context = context;
    }

    private static readonly Dictionary<string, string> BranchesMap = new()
    {
        { "all-branches", "ALL BRANCH" },
    };
    
    public async Task<List<Mis>?> GetBranchesTargetReviewAsync(string value)
    {
        if (!BranchesMap.TryGetValue(value, out var item))
            return null;

        return await _context.Mis
            .Where(m => m.Purpose == item)
            .ToListAsync();
    }
}