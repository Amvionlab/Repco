using LoginBackend.Models.Entities;
using LoginBackend.Models.Request;

namespace LoginBackend.Services.Interfaces;

public interface IMisService
{
    Task<Mis> CreateAsync(CreateMisRequest request);
    Task<List<Mis>> GetAllAsync();
    Task<List<Mis>> GetDepositTargetReviewAsync(string value);
}