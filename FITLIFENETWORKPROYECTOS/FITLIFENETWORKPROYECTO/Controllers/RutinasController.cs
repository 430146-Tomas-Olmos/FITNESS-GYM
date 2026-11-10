using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RutinasController : ControllerBase
{
    private readonly IServices _services;

    public RutinasController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetRutinas([FromQuery] int? idSocio = null, [FromQuery] int? idEntrenador = null)
    {
        try
        {
            return Ok(await _services.GetRutinasAsync(idSocio, idEntrenador));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRutina([FromBody] RutinaCreateDto dto)
    {
        try
        {
            return Ok(await _services.CreateRutinaAsync(dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
