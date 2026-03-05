using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LoginBackend.Services.Interfaces;

namespace LoginBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/branches")]
public class BranchesController : ControllerBase
{
    private readonly IBranchesMisService _branchesService;

    public BranchesController(IBranchesMisService branchesService)
    {
        _branchesService = branchesService;
    }

    [HttpGet("{value}")]
    public async Task<IActionResult> GetBranchesTargetReview(string value)
    {
        var result = await _branchesService.GetBranchesTargetReviewAsync(value);

        if (result == null)
            return NotFound("Branches - Target Review not found");

        return Ok(result);
    }
}