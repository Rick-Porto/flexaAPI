using flexa.App.DTOs.Screen;
using flexa.App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace flexa.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScreenController(IScreenService service) : ControllerBase
    {
        private readonly IScreenService _service = service;

        // Helper to get the logged-in user's Supabase ID
        private string UserId => User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException();

        [HttpGet("debug-token")]
        [AllowAnonymous]
        public IActionResult DebugToken([FromServices] IConfiguration config)
        {
            var secret = config["Supabase:JwtSecret"];
            var url = config["Supabase:Url"];
            return Ok(new
            {
                secretLength = secret?.Length,
                secretFirst5 = secret?.Substring(0, 5),
                url
            });
        }

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
