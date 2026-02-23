namespace LoginBackend.Models.Response;

public class CreateUserResponse
{
    public string Message { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int UserType { get; set; }
}
