using Microsoft.AspNetCore.Mvc;
using MagicSignal.Modules.Accounts.Application.DTOs.Auth;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;

namespace MagicSignal.Modules.Accounts.API.Controllers;
[ApiController]
[Route("api/accounts/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(request);

        if (!result.IsSuccess)
            return Unauthorized(result.Message);

        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }
}