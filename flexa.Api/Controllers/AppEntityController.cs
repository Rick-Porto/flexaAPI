using flexa.App.DTOs.AppEntity;
using flexa.App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace flexa.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    
    public class AppEntityController(IAppEntityService service) : ControllerBase
    {
        private readonly IAppEntityService _service = service;

        // Helper to get the logged-in user's Supabase ID
        private string UserId => User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException();

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
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppEntityRequest request)
        {
            var result = await _service.AddAsync(request);
            return CreatedAtAction(nameof(GetById), 
                new { id = result.Id }, 
                result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AppEntityRequest request)
        {
            await _service.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}