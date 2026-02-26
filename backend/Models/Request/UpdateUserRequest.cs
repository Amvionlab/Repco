namespace LoginBackend.Models.Request;

public class UpdateUserRequest
{
    public string? Name { get; set; }
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public int? UserType { get; set; }
    public bool? IsActive { get; set; }
}

