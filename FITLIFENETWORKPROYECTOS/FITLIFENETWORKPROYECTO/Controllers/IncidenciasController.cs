using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_EQUIPAMIENTO,ROLE_TECNICO")]
public class IncidenciasController : ControllerBase
{
    private readonly IServices _services;

    public IncidenciasController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetIncidencias([FromQuery] int? idSede = null)
    {
        try
        {
            return Ok(await _services.GetIncidenciasAsync(idSede));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IncidenciaCreateDto dto)
    {
        try
        {
            var incidencia = await _services.CreateIncidenciaAsync(dto);
            return StatusCode(StatusCodes.Status201Created, incidencia);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
