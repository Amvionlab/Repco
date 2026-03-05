using LoginBackend.Models.Entities;

namespace LoginBackend.Services.Interfaces;

public interface INpaMisService
{
    Task<List<Mis>?> GetNpaTargetReviewAsync(string value);
}