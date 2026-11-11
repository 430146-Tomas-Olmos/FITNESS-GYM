using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_MIEMBROS,ROLE_GERENTE_VENTAS,ROLE_GERENTE_EQUIPAMIENTO,ROLE_TECNICO")]
public class TurnosController : ControllerBase
{
    private readonly IServices _services;

    public TurnosController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetTurnos([FromQuery] int? idSocio = null, [FromQuery] int? idProfesional = null)
    {
        try
        {
            return Ok(await _services.GetTurnosAsync(idSocio, idProfesional));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idTurno:int}")]
    public async Task<IActionResult> GetById(int idTurno)
    {
        try
        {
            var turno = await _services.GetTurnoByIdAsync(idTurno);
            if (turno == null)
            {
                return NotFound();
            }

            return Ok(turno);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTurno([FromBody] TurnoCreateDto dto)
    {
        try
        {
            return Ok(await _services.CreateTurnoAsync(dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{idTurno:int}")]
    public async Task<IActionResult> UpdateTurno(int idTurno, [FromBody] TurnoCreateDto dto)
    {
        try
        {
            var turno = await _services.UpdateTurnoAsync(idTurno, dto);
            if (turno == null)
            {
                return NotFound();
            }

            return Ok(turno);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{idTurno:int}")]
    public async Task<IActionResult> DeleteTurno(int idTurno)
    {
        try
        {
            var eliminado = await _services.DeleteTurnoAsync(idTurno);
            if (!eliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
