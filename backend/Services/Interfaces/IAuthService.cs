using LoginBackend.Models.Request;
using LoginBackend.Models.Response;

namespace LoginBackend.Services.Interfaces;

public interface IAuthService
{
    Task<string?> RequestOtpAsync(RequestOtpRequest request);
    Task<LoginResponse?> VerifyOtpAsync(VerifyOtpRequest request);
    Task<bool> LogoutAsync(string sessionId);
    Task<CreateUserResponse?> CreateUserAsync(SignUpRequest request, string callerMobile);
    Task<List<object>?> GetAllUsersAsync(string callerMobile);
    Task<object?> UpdateUserAsync(int userId, UpdateUserRequest request, string callerMobile);
    Task<bool?> DeleteUserAsync(int userId, string callerMobile);
}
