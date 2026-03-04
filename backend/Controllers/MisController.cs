using LoginBackend.Data;
using LoginBackend.Models.Request;
using LoginBackend.Models.Response;
using LoginBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// [Authorize]
[ApiController]
[Route("api/mis")]
public class MisController : ControllerBase
{
    private readonly IMisService _misService;

    public MisController(IMisService misService)
    {
        _misService = misService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMisRequest request)
    {
        var result = await _misService.CreateAsync(request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _misService.GetAllAsync();
        return Ok(result);
    }
}