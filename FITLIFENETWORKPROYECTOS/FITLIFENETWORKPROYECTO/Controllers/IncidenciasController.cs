using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Models.Context;
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
    private readonly FITLIFENETWORKDbContext _db;

    public IncidenciasController(IServices services, FITLIFENETWORKDbContext db)
    {
        _services = services;
        _db = db;
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] IncidenciaCreateDto dto)
    {
        try
        {
            var reclamo = await _db.IncidenciasReclamos.FindAsync(id);
            if (reclamo == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Descripcion)) reclamo.Descripcion = dto.Descripcion;
            if (dto.IdEquipo.HasValue) reclamo.IdEquipo = dto.IdEquipo;
            if (dto.IdSede > 0) reclamo.IdSede = dto.IdSede;

            await _db.SaveChangesAsync();
            return Ok(reclamo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var reclamo = await _db.IncidenciasReclamos.FindAsync(id);
            if (reclamo == null) return NotFound();

            _db.IncidenciasReclamos.Remove(reclamo);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Incidencia eliminada" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
