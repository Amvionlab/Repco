namespace LoginBackend.Models.Response;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? SessionId { get; set; }
    public int UserType { get; set; }
}
