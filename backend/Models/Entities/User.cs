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
    public DateTimeOffset? OtpValid { get; set; }
    // Session stored in DB — survives server restarts
    public string? SessionId { get; set; }
    public DateTimeOffset? SessionExpiresAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }
}
