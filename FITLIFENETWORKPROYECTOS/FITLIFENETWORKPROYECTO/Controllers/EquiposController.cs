using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_EQUIPAMIENTO,ROLE_TECNICO")]
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EquipoCreateDto dto)
    {
        try
        {
            var equipo = await _services.CreateEquipoAsync(dto);
            return StatusCode(StatusCodes.Status201Created, equipo);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EquipoCreateDto dto)
    {
        try
        {
            var equipo = await _services.UpdateEquipoAsync(id, dto);
            if (equipo == null) return NotFound();
            return Ok(equipo);
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
            var deleted = await _services.DeleteEquipoAsync(id);
            if (!deleted) return NotFound();
            return Ok(new { message = "Equipo eliminado" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
