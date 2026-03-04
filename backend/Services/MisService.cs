using LoginBackend.Data;
using LoginBackend.Models.Request;
using LoginBackend.Models.Response;
using LoginBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using LoginBackend.Services.Interfaces;

namespace LoginBackend.Services;

public class MisService : IMisService
{
    private readonly ApplicationDbContext _context;

    public MisService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Mis> CreateAsync(CreateMisRequest request)
    {
        var mis = new Mis
        {
            Purpose = request.Purpose,
            Description1 = request.Description1,
            Description2 = request.Description2,
            MobileNo = request.MobileNo,
            BusnsDt = request.BusnsDt,
            OS_AsOnDate = request.OS_AsOnDate,
            DayIncrease = request.DayIncrease,
            MnthIncrease = request.MnthIncrease,
            YearIncrease = request.YearIncrease,
            Percentage = request.Percentage,
            Previous_OS = request.Previous_OS,
            Growth_Over_March = request.Growth_Over_March,
            Monthly_Target_Achieved = request.Monthly_Target_Achieved,
            Yearly_Target_Achieved = request.Yearly_Target_Achieved,
            Growth_Over_March_Branch = request.Growth_Over_March_Branch,
            Current_Month_Target = request.Current_Month_Target,
            Monthly_GAP = request.Monthly_GAP,
            Yearend_Target = request.Yearend_Target,
            Year_End_Gap = request.Year_End_Gap,
            Previous_Year = request.Previous_Year,
            No_Of_Acct = request.No_Of_Acct,
            No_Of_Acct_OS = request.No_Of_Acct_OS,
            No_Of_Acct1 = request.No_Of_Acct1,
            No_Of_Acct1_OS1 = request.No_Of_Acct1_OS1,
            Total_No_Acct = request.Total_No_Acct,
            Total_No_Acct_OS = request.Total_No_Acct_OS,
            BrID = request.BrID,
            PrdID = request.PrdID
        };

        _context.Mis.Add(mis);
        await _context.SaveChangesAsync();

        return mis;
    }

    public async Task<List<Mis>> GetAllAsync()
    {
        return await _context.Mis.Take(10).ToListAsync();
    }

    public async Task<List<Mis>> GetDepositTargetReviewAsync(string value)
    {
        string? item = null;
        if (value == "target_review"){
           item = "Deposits - Target Review";
        }
        else if (value == "major_closure"){
            item = "Deposits - Major Closure";
        }
        else if (value == "target_achieved_branches"){
            item = "Deposits - Target Achieved Branches";
        }
        else if (value == "tenure_wise"){
            item = "Deposit_Tenure_Wise";
        }
        else if (value == "productwise_outstanding"){
            item = "Deposits - Productwise Outstanding";
        }
        else if (value == "state_wise"){
            item = "Deposit_State_Wise";
        }
        else if (value == "constitutionwise"){
            item = "Deposit_Constitutionwise";
        }
        else if (value == "membership"){
            item = "Deposit_Membership";
        }
        else if (value == "major_mobilization"){
            item = "Deposit-Major Mobilization";
        }
        else if (value == "roi_slab"){
            item = "Deposit_ROI_Slab";
        }
        else if (value == "retail_bulk"){
            item = "Deposit_Retail_Bulk";
        }
        else if (value == "residual_maturity"){
            item = "Deposit_Residual_Maturity";
        }
        else {
            return null;
        }
        Console.WriteLine("item: " + item);
        Console.WriteLine("value: " + value);
        return await _context.Mis
            .Where(m => m.Purpose == item)
            .ToListAsync();
    }
}