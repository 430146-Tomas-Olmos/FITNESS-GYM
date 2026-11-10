using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MantenimientoController : ControllerBase
{
    private readonly IServices _services;

    public MantenimientoController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetMantenimientos([FromQuery] int? idEquipo = null)
    {
        try
        {
            return Ok(await _services.GetMantenimientosAsync(idEquipo));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idMantenimiento:int}")]
    public async Task<IActionResult> GetById(int idMantenimiento)
    {
        try
        {
            var mantenimiento = await _services.GetMantenimientoByIdAsync(idMantenimiento);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            return Ok(mantenimiento);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateMantenimiento([FromBody] MantenimientoEquipamientoCreateDto dto)
    {
        try
        {
            var mantenimiento = await _services.CreateMantenimientoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idMantenimiento = mantenimiento.IdMantenimiento }, mantenimiento);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{idMantenimiento:int}")]
    public async Task<IActionResult> Update(int idMantenimiento, [FromBody] MantenimientoEquipamientoCreateDto dto)
    {
        try
        {
            var mantenimiento = await _services.UpdateMantenimientoAsync(idMantenimiento, dto);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            return Ok(mantenimiento);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{idMantenimiento:int}")]
    public async Task<IActionResult> Delete(int idMantenimiento)
    {
        try
        {
            var eliminado = await _services.DeleteMantenimientoAsync(idMantenimiento);
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
