using LoginBackend.Data;
using LoginBackend.Models.Entities;
using LoginBackend.Models.Request;
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
}