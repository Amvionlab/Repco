using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LoginBackend.Services.Interfaces;

namespace LoginBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/deposit")]
public class DepositController : ControllerBase
{
    private readonly IMisService _misService;

    public DepositController(IMisService misService)
    {
        _misService = misService;
    }

    [HttpGet("{value}")]
    public async Task<IActionResult> GetDepositTargetReview(string value)
    {
        var result = await _misService.GetDepositTargetReviewAsync(value);

        if (result == null)
            return NotFound("Deposits - Target Review not found");

        return Ok(result);
    }
}