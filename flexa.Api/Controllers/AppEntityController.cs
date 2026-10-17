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

        // Lê o user ID do JWT validado — nunca do body/query string
        private string UserId => User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("User identity missing from token.");

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync(UserId);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Retorna 404 se o app existir mas pertencer a outro usuário
            var result = await _service.GetByIdAsync(id, UserId);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppEntityRequest request)
        {
            var result = await _service.AddAsync(request, UserId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AppEntityRequest request)
        {
            try
            {
                await _service.UpdateAsync(id, request, UserId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id, UserId);
            return NoContent();
        }

        /// <summary>
        /// Publishes the app by generating a public link.
        /// </summary>
        [HttpPost("{id:guid}/publish")]
        public async Task<IActionResult> Publish(Guid id)
        {
            var result = await _service.PublishAsync(id, UserId);
            if (result is null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Unpublishes the app by removing the public link.
        /// </summary>
        [HttpPost("{id:guid}/unpublish")]
        public async Task<IActionResult> Unpublish(Guid id)
        {
            var result = await _service.UnpublishAsync(id, UserId);
            if (result is null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Gets an app by its public link (no auth required - for public access).
        /// </summary>
        [HttpGet("public/{publicLink}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByPublicLink(string publicLink)
        {
            var result = await _service.GetByPublicLinkAsync(publicLink);
            if (result is null) return NotFound();
            return Ok(result);
        }
    }
}