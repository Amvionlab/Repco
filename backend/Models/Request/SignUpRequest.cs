namespace LoginBackend.Models.Request;

public class SignUpRequest
{
    public string Name { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int UserType { get; set; } = 2; // 1 = Admin, 2 = Regular User
}
