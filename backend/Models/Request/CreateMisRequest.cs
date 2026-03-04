namespace LoginBackend.Models.Request;

public class CreateMisRequest
{
    public string Purpose { get; set; } = string.Empty;
    public string Description1 { get; set; } = string.Empty;
    public string Description2 { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public DateTime BusnsDt { get; set; }
    public DateTime OS_AsOnDate { get; set; }
    public decimal DayIncrease { get; set; }
    public decimal MnthIncrease { get; set; }
    public decimal YearIncrease { get; set; }
    public decimal Percentage { get; set; }
    public decimal Previous_OS { get; set; }
    public decimal Growth_Over_March { get; set; }
    public decimal Monthly_Target_Achieved { get; set; }
    public decimal Yearly_Target_Achieved { get; set; }
    public decimal Growth_Over_March_Branch { get; set; }
    public decimal Current_Month_Target { get; set; }
    public decimal Monthly_GAP { get; set; }
    public decimal Yearend_Target { get; set; }
    public decimal Year_End_Gap { get; set; }
    public decimal Previous_Year { get; set; }
    public int No_Of_Acct { get; set; }
    public int No_Of_Acct_OS { get; set; }
    public int No_Of_Acct1 { get; set; }
    public int No_Of_Acct1_OS1 { get; set; }
    public int Total_No_Acct { get; set; }
    public int Total_No_Acct_OS { get; set; }
    public int BrID { get; set; }
    public int PrdID { get; set; }
}