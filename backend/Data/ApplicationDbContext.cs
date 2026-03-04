using Microsoft.EntityFrameworkCore;
using LoginBackend.Models.Entities;

namespace LoginBackend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Mis> Mis { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.MobileNumber).HasColumnName("mobile_number");
            entity.Property(e => e.Otp).HasColumnName("otp");
            entity.Property(e => e.OtpValid).HasColumnName("otp_valid");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UserType).HasColumnName("user_type");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.SessionExpiresAt).HasColumnName("session_expires_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        });

        modelBuilder.Entity<Mis>().ToTable("mis");

        modelBuilder.Entity<Mis>(entity =>
        {
            entity.HasKey(e => e.PK_ID);
            entity.Property(e => e.PK_ID).HasColumnName("PK_ID");
            entity.Property(e => e.Purpose).HasColumnName("Purpose");
            entity.Property(e => e.Description1).HasColumnName("Description1");
            entity.Property(e => e.Description2).HasColumnName("Description2");
            entity.Property(e => e.MobileNo).HasColumnName("MobileNo");
            entity.Property(e => e.BusnsDt).HasColumnName("BusnsDt");
            entity.Property(e => e.OS_AsOnDate).HasColumnName("OS_AsOnDate");
            entity.Property(e => e.DayIncrease).HasColumnName("DayIncrease");
            entity.Property(e => e.MnthIncrease).HasColumnName("MnthIncrease");
            entity.Property(e => e.YearIncrease).HasColumnName("YearIncrease");
            entity.Property(e => e.Percentage).HasColumnName("Percentage");
            entity.Property(e => e.Previous_OS).HasColumnName("Previous_OS");
            entity.Property(e => e.Growth_Over_March).HasColumnName("Growth_Over_March");
            entity.Property(e => e.Monthly_Target_Achieved).HasColumnName("Monthly_Target_Achieved");
            entity.Property(e => e.Yearly_Target_Achieved).HasColumnName("Yearly_Target_Achieved");
            entity.Property(e => e.Growth_Over_March_Branch).HasColumnName("Growth_Over_March_Branch");
            entity.Property(e => e.Current_Month_Target).HasColumnName("Current_Month_Target");
            entity.Property(e => e.Monthly_GAP).HasColumnName("Monthly_GAP");
            entity.Property(e => e.Yearend_Target).HasColumnName("Yearend_Target");
            entity.Property(e => e.Year_End_Gap).HasColumnName("Year_End_Gap");
            entity.Property(e => e.Previous_Year).HasColumnName("Previous_Year");
            entity.Property(e => e.No_Of_Acct).HasColumnName("No_Of_Acct");
            entity.Property(e => e.No_Of_Acct_OS).HasColumnName("No_Of_Acct_OS");
            entity.Property(e => e.No_Of_Acct1).HasColumnName("No_Of_Acct1");
            entity.Property(e => e.No_Of_Acct1_OS1).HasColumnName("No_Of_Acct1_OS1");
            entity.Property(e => e.Total_No_Acct).HasColumnName("Total_No_Acct");
            entity.Property(e => e.Total_No_Acct_OS).HasColumnName("Total_No_Acct_OS");
            entity.Property(e => e.BrID).HasColumnName("BrID");
            entity.Property(e => e.PrdID).HasColumnName("PrdID");
        });
    }
}