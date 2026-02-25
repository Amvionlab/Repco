using LoginBackend.Data;
using LoginBackend.Models.Request;
using LoginBackend.Models.Response;
using LoginBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ApplicationDbContext _context;

    public AuthController(IAuthService authService, ApplicationDbContext context)
    {
        _authService = authService;
        _context = context;
    }

    [HttpGet("health")]
    public async Task<IActionResult> CheckHealth()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            Console.WriteLine(canConnect);
            return canConnect
                ? Ok(new { Status = "Healthy", Database = "Connected" })
                : StatusCode(500, new { Status = "Unhealthy", Database = "Connection Failed" });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new { Status = "Error", Message = ex.Message });
           
        }
    }

    [HttpPost("request-otp")]
    public async Task<IActionResult> RequestOtp([FromBody] RequestOtpRequest request)
    {
        try
        {
            var message = await _authService.RequestOtpAsync(request);
            return message != null
                ? Ok(new { Message = message })
                : NotFound(new { Message = "User with this mobile number does not exist" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Status = "Error", Message = ex.Message });
        }
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        try
        {
            var result = await _authService.VerifyOtpAsync(request);
            return result != null
                ? Ok(result)
                : Unauthorized(new { Message = "Invalid or expired OTP" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Status = "Error", Message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("verify")]
    public async Task<IActionResult> Verify()
    {
       
        var sessionId = Request.Headers["X-Session-Id"].ToString();

        if (string.IsNullOrEmpty(sessionId))
            return Unauthorized(new { Message = "No session ID provided" });

        // Look up session in DB
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.SessionId == sessionId &&
            u.SessionExpiresAt > DateTimeOffset.Now &&
            u.DeletedAt == null);

        if (user == null)
            return Unauthorized(new { Message = "Session expired or invalid" });

        return Ok(new
        {
            Message = "Token and Session are valid",
            MobileNumber = user.MobileNumber,
            SessionId = sessionId,
            UserType = user.UserType
        });
    }

    [Authorize]
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] SignUpRequest request)
    {
        try
        {
            // Get caller's mobile from JWT sub claim
            var callerMobile = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(callerMobile))
                return Unauthorized(new { Message = "Invalid token" });

            var result = await _authService.CreateUserAsync(request, callerMobile);

            if (result == null)
                return StatusCode(403, new { Message = "Only admins can create users" });

            if (result.UserId == 0)
                return Conflict(new { result.Message });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Status = "Error", Message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var sessionId = Request.Headers["X-Session-Id"].ToString();
        if (string.IsNullOrEmpty(sessionId))
            return BadRequest(new { Message = "No session ID provided" });

        await _authService.LogoutAsync(sessionId);
        return Ok(new { Message = "Logged out successfully" });
    }

    // ── Admin: Get All Users ─────────────────────────────────────────
    [Authorize]
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var callerMobile = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(callerMobile))
                return Unauthorized(new { Message = "Invalid token" });

            var users = await _authService.GetAllUsersAsync(callerMobile);

            if (users == null)
                return StatusCode(403, new { Message = "Only admins can view users" });

            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Status = "Error", Message = ex.Message });
        }
    }

    // ── Admin: Update User ───────────────────────────────────────────
    [Authorize]
    [HttpPut("users/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var callerMobile = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(callerMobile))
                return Unauthorized(new { Message = "Invalid token" });

            var result = await _authService.UpdateUserAsync(id, request, callerMobile);

            if (result == null)
                return StatusCode(403, new { Message = "Only admins can update users" });

            // Check if it's an error response
            var errorProp = result.GetType().GetProperty("Error");
            if (errorProp != null)
            {
                var errorMsg = errorProp.GetValue(result)?.ToString();
                return BadRequest(new { Message = errorMsg });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Status = "Error", Message = ex.Message });
        }
    }

    // ── Admin: Delete User (soft) ────────────────────────────────────
    [Authorize]
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var callerMobile = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(callerMobile))
                return Unauthorized(new { Message = "Invalid token" });

            var result = await _authService.DeleteUserAsync(id, callerMobile);

            if (result == null)
                return StatusCode(403, new { Message = "Only admins can delete users" });

            if (result == false)
                return BadRequest(new { Message = "User not found or cannot delete yourself" });

            return Ok(new { Message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Status = "Error", Message = ex.Message });
        }
    }
}
