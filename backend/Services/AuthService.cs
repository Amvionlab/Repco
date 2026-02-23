using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginBackend.Data;
using LoginBackend.Models.Request;
using LoginBackend.Models.Response;
using LoginBackend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LoginBackend.Services;

using LoginBackend.Services.Interfaces;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string?> RequestOtpAsync(RequestOtpRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == request.MobileNumber && u.DeletedAt == null);

        if (user == null) return null;

        if (!user.IsActive)
            return null; // Inactive users cannot request OTP

        var otp = new Random().Next(100000, 999999).ToString();
        Console.WriteLine($"[DEBUG] OTP for {request.MobileNumber}: {otp}");

        user.Otp = BCrypt.Net.BCrypt.HashPassword(otp);
        user.OtpValid = DateTimeOffset.Now.AddMinutes(1);
        user.UpdatedAt = DateTimeOffset.Now;

        await _context.SaveChangesAsync();
        return "OTP sent successfully";
    }

    public async Task<LoginResponse?> VerifyOtpAsync(VerifyOtpRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == request.MobileNumber && u.DeletedAt == null);

        if (user == null || user.Otp == null || !user.OtpValid.HasValue)
            return null;

        if (!user.IsActive)
            return null; // Inactive users cannot log in

        // The otp_valid column is "timestamp without time zone" so Npgsql
        // returns Kind=Unspecified containing local time (IST).
        
        // ToUniversalTime() converts Unspecified → Local → UTC correctly.
        var otpExpiry = user.OtpValid.Value;
        Console.WriteLine($"[OTP] Expiry: {otpExpiry}  |  Now: {DateTimeOffset.Now}  |  Expired: {otpExpiry < DateTimeOffset.Now}");

        if (otpExpiry < DateTimeOffset.Now)
        {
            Console.WriteLine("[OTP] REJECTED — expired");
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Otp, user.Otp))
            return null;

        // Clear OTP and generate a new persistent session stored in DB
        var sessionId = Guid.NewGuid().ToString();
        user.Otp = null;
        user.OtpValid = null;
        user.SessionId = sessionId;
        user.SessionExpiresAt = DateTimeOffset.Now.AddDays(30); // 30-day session
        user.UpdatedAt = DateTimeOffset.Now;
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user.MobileNumber);

        return new LoginResponse
        {
            Token = token,
            Message = "Login successful",
            SessionId = sessionId,
            UserType = user.UserType
        };
    }

    public async Task<bool> LogoutAsync(string sessionId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.SessionId == sessionId);

        if (user == null) return false;

        user.SessionId = null;
        user.SessionExpiresAt = null;
        user.UpdatedAt = DateTimeOffset.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<CreateUserResponse?> CreateUserAsync(SignUpRequest request, string callerMobile)
    {
        // Verify the caller is an admin
        var caller = await _context.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == callerMobile && u.DeletedAt == null);

        if (caller == null || caller.UserType != 1)
            return null; // Only admins can create users

        // Check for duplicate mobile number
        var existing = await _context.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == request.MobileNumber && u.DeletedAt == null);

        if (existing != null)
            return new CreateUserResponse
            {
                Message = "User with this mobile number already exists",
                UserId = 0
            };

        var newUser = new User
        {
            Name = request.Name,
            MobileNumber = request.MobileNumber,
            Email = request.Email,
            UserType = request.UserType,
            IsActive = true,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return new CreateUserResponse
        {
            Message = "User created successfully",
            UserId = newUser.Id,
            Name = newUser.Name,
            MobileNumber = newUser.MobileNumber,
            Email = newUser.Email,
            UserType = newUser.UserType
        };
    }

    public async Task<List<object>?> GetAllUsersAsync(string callerMobile)
    {
        var caller = await _context.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == callerMobile && u.DeletedAt == null);

        if (caller == null || caller.UserType != 1)
            return null;

        var users = await _context.Users
            .Where(u => u.DeletedAt == null)
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new
            {
                u.Id,
                u.Name,
                u.MobileNumber,
                u.Email,
                u.UserType,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt
            })
            .ToListAsync();

        return users.Cast<object>().ToList();
    }

    public async Task<object?> UpdateUserAsync(int userId, UpdateUserRequest request, string callerMobile)
    {
        var caller = await _context.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == callerMobile && u.DeletedAt == null);

        if (caller == null || caller.UserType != 1)
            return null;

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.DeletedAt == null);

        if (user == null)
            return new { Error = "User not found" };

        // Check for duplicate mobile if changing it
        if (!string.IsNullOrEmpty(request.MobileNumber) && request.MobileNumber != user.MobileNumber)
        {
            var dup = await _context.Users
                .FirstOrDefaultAsync(u => u.MobileNumber == request.MobileNumber && u.DeletedAt == null && u.Id != userId);
            if (dup != null)
                return new { Error = "Mobile number already in use" };
        }

        if (request.Name != null) user.Name = request.Name;
        if (request.MobileNumber != null) user.MobileNumber = request.MobileNumber;
        if (request.Email != null) user.Email = request.Email;
        if (request.UserType.HasValue) user.UserType = request.UserType.Value;
        if (request.IsActive.HasValue) user.IsActive = request.IsActive.Value;
        user.UpdatedAt = DateTimeOffset.Now;

        await _context.SaveChangesAsync();

        return new
        {
            Message = "User updated successfully",
            user.Id,
            user.Name,
            user.MobileNumber,
            user.Email,
            user.UserType,
            user.IsActive
        };
    }

    public async Task<bool?> DeleteUserAsync(int userId, string callerMobile)
    {
        var caller = await _context.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == callerMobile && u.DeletedAt == null);

        if (caller == null || caller.UserType != 1)
            return null;

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.DeletedAt == null);

        if (user == null) return false;

        // Prevent admin from deleting themselves
        if (user.Id == caller.Id) return false;

        user.DeletedAt = DateTimeOffset.Now;
        user.SessionId = null;
        user.SessionExpiresAt = null;
        user.UpdatedAt = DateTimeOffset.Now;
        await _context.SaveChangesAsync();

        return true;
    }

    private string GenerateJwtToken(string username)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiryMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
