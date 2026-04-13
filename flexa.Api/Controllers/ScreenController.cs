using Microsoft.AspNetCore.Mvc;
using flexa.App.DTOs.Screen;
using flexa.App.Interfaces;

namespace flexa.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScreenController(IScreenService service) : ControllerBase
    {
        private readonly IScreenService _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScreenResponse>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ScreenResponse>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ScreenResponse>> Create([FromBody] ScreenRequest request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ScreenResponse>> Update(Guid id, [FromBody] ScreenRequest request)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, request);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
