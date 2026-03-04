using LoginBackend.Models.Entities;

namespace LoginBackend.Services.Interfaces;

public interface IDepositMisService
{
    Task<List<Mis>?> GetDepositTargetReviewAsync(string value);
}