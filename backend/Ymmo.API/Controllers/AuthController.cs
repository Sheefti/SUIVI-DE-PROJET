using Microsoft.AspNetCore.Mvc;
using Ymmo.API.DTOs;
using Ymmo.API.Services;

namespace Ymmo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var result = await _authService.Register(dto);
        if (result == null)
            return BadRequest(new { message = "Email déjà utilisé ou rôle invalide." });

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var result = await _authService.Login(dto);
        if (result == null)
            return Unauthorized(new { message = "Email ou mot de passe incorrect." });

        return Ok(result);
    }
}