using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_MIEMBROS")]
public class ReservasController : ControllerBase
{
    private readonly IServices _services;

    public ReservasController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idSocio = null)
    {
        try
        {
            var reservas = await _services.GetReservasAsync(idSocio);
            return Ok(reservas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("clases")]
    public async Task<IActionResult> GetClases()
    {
        try
        {
            var clases = await _services.GetClasesAsync();
            return Ok(clases);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idReserva:int}")]
    public async Task<IActionResult> GetById(int idReserva)
    {
        try
        {
            var reserva = await _services.GetReservaByIdAsync(idReserva);
            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservaCreateDto dto)
    {
        try
        {
            var reserva = await _services.CreateReservaAsync(dto);
            return StatusCode(StatusCodes.Status201Created, reserva);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{idReserva:int}")]
    public async Task<IActionResult> Update(int idReserva, [FromBody] ReservaCreateDto dto)
    {
        try
        {
            var reserva = await _services.UpdateReservaAsync(idReserva, dto);
            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{idReserva:int}")]
    public async Task<IActionResult> Delete(int idReserva)
    {
        try
        {
            var eliminado = await _services.DeleteReservaAsync(idReserva);
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
