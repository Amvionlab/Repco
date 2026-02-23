namespace LoginBackend.Models.Request;

public class VerifyOtpRequest
{
    public string MobileNumber { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
}
