using LoginBackend.Services.Interfaces;
using LoginBackend.Data;
using LoginBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginBackend.Services;

public class NpaMisService : INpaMisService
{
    private readonly ApplicationDbContext _context;

    public NpaMisService(ApplicationDbContext context)
    {
        _context = context;
    }

    private static readonly Dictionary<string, string> NpaMap = new()
    {
        { "branchwise", "NPA Branchwise" },
        { "outstanding", "NPA Outstanding" },
    };
    
    public async Task<List<Mis>?> GetNpaTargetReviewAsync(string value)
    {
        if (!NpaMap.TryGetValue(value, out var item))
            return null;

        return await _context.Mis
            .Where(m => m.Purpose == item)
            .ToListAsync();
    }
}