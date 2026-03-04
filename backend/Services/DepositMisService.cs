using LoginBackend.Services.Interfaces;
using LoginBackend.Data;
using LoginBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginBackend.Services;

public class DepositMisService : IDepositMisService
{
    private readonly ApplicationDbContext _context;

    public DepositMisService(ApplicationDbContext context)
    {
        _context = context;
    }

    private static readonly Dictionary<string, string> DepositMap = new()
    {
        { "target_review", "Deposits - Target Review" },
        { "major_closure", "Deposits - Major Closure" },
        { "target_achieved_branches", "Deposits - Target Achieved Branches" },
        { "tenure_wise", "Deposit_Tenure_Wise" },
        { "productwise_outstanding", "Deposits - Productwise Outstanding" },
        { "state_wise", "Deposit_State_Wise" },
        { "constitutionwise", "Deposit_Constitutionwise" },
        { "membership", "Deposit_Membership" },
        { "major_mobilization", "Deposit-Major Mobilization" },
        { "roi_slab", "Deposit_ROI_Slab" },
        { "retail_bulk", "Deposit_Retail_Bulk" },
        { "residual_maturity", "Deposit_Residual_Maturity" }
    };

    public async Task<List<Mis>?> GetDepositTargetReviewAsync(string value)
    {
        if (!DepositMap.TryGetValue(value, out var item))
            return null;

        return await _context.Mis
            .Where(m => m.Purpose == item)
            .ToListAsync();
    }
}