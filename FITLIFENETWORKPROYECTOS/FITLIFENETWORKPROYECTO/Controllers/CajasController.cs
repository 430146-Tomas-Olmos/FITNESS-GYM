using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Models.Context;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS,ROLE_VENDEDOR")]
public class CajasController : ControllerBase
{
    private readonly IServices _services;
    private readonly FITLIFENETWORKDbContext _db;

    public CajasController(IServices services, FITLIFENETWORKDbContext db)
    {
        _services = services;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetCajas([FromQuery] int? idSede = null)
    {
        try
        {
            return Ok(await _services.GetCajasAsync(idSede));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCaja([FromBody] CajaCreateDto dto)
    {
        try
        {
            var caja = await _services.CreateCajaAsync(dto);
            return StatusCode(StatusCodes.Status201Created, caja);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}/cerrar")]
    public async Task<IActionResult> CerrarCaja(int id)
    {
        try
        {
            var caja = await _db.Cajas.FindAsync(id);
            if (caja == null) return NotFound();

            caja.Estado = "CERRADA";
            caja.FechaCierre = DateTime.Now;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Caja cerrada con éxito", idCaja = caja.IdCaja, estado = caja.Estado, fechaCierre = caja.FechaCierre });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}/toggle-estado")]
    public async Task<IActionResult> ToggleEstado(int id)
    {
        try
        {
            var caja = await _db.Cajas.FindAsync(id);
            if (caja == null) return NotFound();

            if (caja.Estado == "ABIERTA")
            {
                caja.Estado = "CERRADA";
                caja.FechaCierre = DateTime.Now;
            }
            else
            {
                caja.Estado = "ABIERTA";
                caja.FechaCierre = null;
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Estado de caja actualizado", idCaja = caja.IdCaja, estado = caja.Estado });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
