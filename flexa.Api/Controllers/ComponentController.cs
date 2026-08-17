using flexa.App.DTOs.Component;
using flexa.App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace flexa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComponentController(IComponentService service) : ControllerBase
{
    private readonly IComponentService _service = service;

    // Helper to get the logged-in user's Supabase ID
    private string UserId => User.FindFirstValue("sub")
        ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User identity missing from token.");

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync(UserId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // Retorna 404 se o app existir mas pertencer a outro usuário
        var result = await _service.GetByIdAsync(id, UserId);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("by-screen/{screenId:int}")]
    public async Task<IActionResult> GetByScreen(int screenId)
    {
        var result = await _service.GetByScreenIdAsync(screenId, UserId);
        return Ok(result);
    }
        

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ComponentRequest request)
    {
        var result = await _service.CreateAsync(request, UserId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ComponentRequest request)
    {
        var result = await _service.UpdateAsync(id, request, UserId);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id, UserId);
        return deleted ? NoContent() : NotFound();
    }
}