using flexa.App.DTOs.Screen;
using flexa.App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace flexa.Api.Controllers
{
    [ApiController]
    [Route("api/screens")]
    [Authorize]
    public class ScreenController : ControllerBase
    {
        private readonly IScreenService _screenService;
        private string UserId => User.FindFirstValue("sub")
        ?? throw new UnauthorizedAccessException();

        public ScreenController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var screens = await _screenService.GetAllAsync(UserId);
            return Ok(screens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var screen = await _screenService.GetByIdAsync(id, UserId);
            return screen != null ? Ok(screen) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ScreenRequest request)
        {
            var screen = await _screenService.AddAsync(request, UserId);
            return CreatedAtAction(nameof(GetById), new { id = screen.Id }, screen);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScreenRequest request)
        {
            try
            {
                await _screenService.UpdateAsync(id, request, UserId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _screenService.DeleteAsync(id, UserId);
            return NoContent();
        }
    }
}

