using LoginBackend.Data;
using LoginBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using LoginBackend.Services.Interfaces;

namespace LoginBackend.Services;

public class AdvancesMisService : IAdvancesMisService
{
    private readonly ApplicationDbContext _context;

    public AdvancesMisService(ApplicationDbContext context)
    {
        _context = context;
    }

    private static readonly Dictionary<string, string> AdvancesMap = new()
    {
        { "sanctions_productwise", "Advances - Sanctions- Productwise" },
        { "sanction-disbursement", "Advances - Santion/Disbursment" },
        { "sanctions-roi-wise", "Advances - Sanctions - ROI Wise" },
        { "rhfl_rfml", "Advances - RHFL/RFML" },
        { "sanction-branchwise", "Advances - Sanctions - Branchwise" },
        { "jl-ojl-outstanding", "Advances - JL- OJL Outstanding" },
        { "tenure-wise", "Advances - Tenure Wise" },
        { "roi-wise", "Advances - ROI Wise" },
        { "major-closure", "Advances - Major Closure" },
        { "staff", "Advances - Staff" },
        { "growth", "Advances - Growth" },
        { "major-disbursement", "Advances - Major Disbursement" },
        { "lsw-pipeline-proposal", "Advances - LSW - Pipeline Proposal" },
        { "ho-bdp-sanctions", "Advances - Ho & BDP Sanctions" },
        { "daily-progress-report", "Advances - Daily Progress Report" },
        { "target-achievement", "Advances - Target & Achievement" },
        { "membershipwise-sanction", "Advances - Membershipwise Sanction" },
        { "disbursement", "Advances - Disbursement" },
        { "composition-of-loans", "Advances - Composition of Loans" },
        { "residual-expiry", "Advances - Residual Expiry" },
        { "sanctions-during-the-year", "Advances - Sanctions during the year" },
        { "sanction-amountwise", "Advances - Sanctions - Amountwise" }
    };

    public async Task<List<Mis>?> GetAdvancesTargetReviewAsync(string value)
    {
        if (!AdvancesMap.TryGetValue(value, out var item))
            return null;

        return await _context.Mis
            .Where(m => m.Purpose == item)
            .ToListAsync();
    }
}