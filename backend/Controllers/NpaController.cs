using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LoginBackend.Services.Interfaces;

namespace LoginBackend.Controllers;

[Authorize]
[ApiController]
[Route("api/npa")]
public class NpaController : ControllerBase
{
    private readonly INpaMisService _npaService;

    public NpaController(INpaMisService npaService)
    {
        _npaService = npaService;
    }

    [HttpGet("{value}")]
    public async Task<IActionResult> GetNpaTargetReview(string value)
    {
        var result = await _npaService.GetNpaTargetReviewAsync(value);

        if (result == null)
            return NotFound("NPA - Target Review not found");

        return Ok(result);
    }
}