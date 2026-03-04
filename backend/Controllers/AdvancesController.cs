using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LoginBackend.Services.Interfaces;

namespace LoginBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/advances")]
public class AdvancesController : ControllerBase
{
    private readonly IAdvancesMisService _advancesService;

    public AdvancesController(IAdvancesMisService advancesService)
    {
        _advancesService = advancesService;
    }

    [HttpGet("{value}")]
    public async Task<IActionResult> GetAdvancesTargetReview(string value)
    {
        var result = await _advancesService.GetAdvancesTargetReviewAsync(value);

        if (result == null)
            return NotFound("Advances - Target Review not found");

        return Ok(result);
    }
}