namespace LoginBackend.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public int UserType { get; set; } = 2; // 1 = Admin, 2 = Regular User
    public string? Otp { get; set; }
    public DateTime? OtpValid { get; set; }
    // Session stored in DB — survives server restarts
    public string? SessionId { get; set; }
    public DateTime? SessionExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}
