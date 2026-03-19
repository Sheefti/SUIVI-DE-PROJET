using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ymmo.API.DTOs;
using Ymmo.API.Services;

namespace Ymmo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BiensController : ControllerBase
{
    private readonly BienService _bienService;

    public BiensController(BienService bienService)
    {
        _bienService = bienService;
    }

    // GET api/Biens
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? ville,
        [FromQuery] string? type,
        [FromQuery] decimal? prixMax)
    {
        var biens = await _bienService.GetAll(ville, type, prixMax);
        return Ok(biens);
    }

    // GET api/Biens/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var bien = await _bienService.GetById(id);
        if (bien == null)
            return NotFound(new { message = "Bien introuvable." });

        return Ok(bien);
    }

    // POST api/Biens
    [HttpPost]
    [Authorize(Roles = "Commercial,Direction")]
    public async Task<IActionResult> Create(CreerBienDTO dto)
    {
        var agenceId = int.Parse(User.FindFirstValue("agenceId")!);
        var result = await _bienService.Create(dto, agenceId);

        if (result == null)
            return BadRequest(new { message = "Type de bien invalide." });

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT api/Biens/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Commercial,Direction")]
    public async Task<IActionResult> Update(int id, ModifierBienDTO dto)
    {
        var agenceId = int.Parse(User.FindFirstValue("agenceId")!);
        var role = User.FindFirstValue(ClaimTypes.Role)!;

        var result = await _bienService.Update(id, dto, agenceId, role);
        if (result == null)
            return NotFound(new { message = "Bien introuvable ou accès refusé." });

        return Ok(result);
    }

    // DELETE api/Biens/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Direction")]
    public async Task<IActionResult> Delete(int id)
    {
        var agenceId = int.Parse(User.FindFirstValue("agenceId")!);
        var role = User.FindFirstValue(ClaimTypes.Role)!;

        var success = await _bienService.Delete(id, agenceId, role);
        if (!success)
            return NotFound(new { message = "Bien introuvable ou accès refusé." });

        return NoContent();
    }
}