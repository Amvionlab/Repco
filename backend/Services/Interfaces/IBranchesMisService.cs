using LoginBackend.Models.Entities;

namespace LoginBackend.Services.Interfaces;

public interface IBranchesMisService
{
    Task<List<Mis>?> GetBranchesTargetReviewAsync(string value);
}