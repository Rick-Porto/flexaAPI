using flexa.App.DTOs.DataEntry;
using flexa.App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace flexa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DataEntryController(IDataEntryService service) : ControllerBase
{
    private readonly IDataEntryService _service = service;

    private string UserId => User.FindFirstValue("sub")
        ?? throw new UnauthorizedAccessException("User identity missing from token.");

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync(UserId);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id, UserId);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-screen/{screenId:int}")]
    public async Task<IActionResult> GetByScreen(int screenId)
    {
        var result = await _service.GetByScreenIdAsync(screenId, UserId);
        return Ok(result);
    }

    [HttpGet("by-component/{componentId:int}")]
    public async Task<IActionResult> GetByComponent(int componentId)
    {
        var result = await _service.GetByComponentIdAsync(componentId, UserId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataEntryRequest request)
    {
        try
        {
            var result = await _service.CreateAsync(request, UserId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(); // ScreenId ou ComponentId não pertencem ao usuário
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataEntryRequest request)
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
