using Microsoft.AspNetCore.Mvc;
using flexa.App.Interfaces;
using flexa.App.DTOs.AppEntity;

namespace flexa.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppEntityController(IAppEntityService service) : ControllerBase
    {
        private readonly IAppEntityService _service = service;

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