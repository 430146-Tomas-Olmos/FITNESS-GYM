using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquiposController : ControllerBase
{
    private readonly IServices _services;

    public EquiposController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetEquipos([FromQuery] int? idSede = null)
    {
        try
        {
            return Ok(await _services.GetEquiposAsync(idSede));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
