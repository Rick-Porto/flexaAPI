using flexa.App.DTOs.DataEntry;
using flexa.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace flexa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataEntryController(IDataEntryService service) : ControllerBase
{
    private readonly IDataEntryService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("by-screen/{screenId:guid}")]
    public async Task<IActionResult> GetByScreen(Guid screenId) =>
        Ok(await _service.GetByScreenIdAsync(screenId));

    [HttpGet("by-component/{componentId:guid}")]
    public async Task<IActionResult> GetByComponent(Guid componentId) =>
        Ok(await _service.GetByComponentIdAsync(componentId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataEntryRequest request)
    {
        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DataEntryRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}