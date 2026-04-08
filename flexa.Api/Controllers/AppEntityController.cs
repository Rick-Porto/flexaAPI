using Microsoft.AspNetCore.Mvc;
using flexa.App.Interfaces;
using flexa.App.DTOs;

namespace flexa.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppEntityController : ControllerBase
    {
        private readonly IAppEntityService _service;

        public AppEntityController(IAppEntityService service)
        {
            _service = service;
        }

        // Implement controller methods
    }
}