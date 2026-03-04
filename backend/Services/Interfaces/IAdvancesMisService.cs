using LoginBackend.Models.Entities;

namespace LoginBackend.Services.Interfaces;

public interface IAdvancesMisService
{
    Task<List<Mis>?> GetAdvancesTargetReviewAsync(string value);
}