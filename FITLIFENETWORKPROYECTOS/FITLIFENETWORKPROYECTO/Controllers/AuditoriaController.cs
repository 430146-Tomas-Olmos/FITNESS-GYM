using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditoriaController : ControllerBase
{
    private readonly IServices _services;

    public AuditoriaController(IServices services)
    {
        _services = services;
    }

    [HttpGet("accesos-denegados")]
    public async Task<IActionResult> GetAccesosDenegados()
    {
        try
        {
            return Ok(await _services.GetAuditoriaAccesosDenegadosAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("cambios-estado-socio")]
    public async Task<IActionResult> GetCambiosEstadoSocio()
    {
        try
        {
            return Ok(await _services.GetAuditoriaCambiosEstadoSocioAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("equipamiento")]
    public async Task<IActionResult> GetEquipamiento()
    {
        try
        {
            return Ok(await _services.GetAuditoriaEquipamientoAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
