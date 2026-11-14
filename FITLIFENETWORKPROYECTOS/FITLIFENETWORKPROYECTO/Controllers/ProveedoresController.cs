using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Models.Context;
using FITLIFENETWORKPROYECTO.Models.Tablas;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS,ROLE_GERENTE_EQUIPAMIENTO,ROLE_TECNICO")]
public class ProveedoresController : ControllerBase
{
    private readonly IServices _services;
    private readonly FITLIFENETWORKDbContext _db;

    public ProveedoresController(IServices services, FITLIFENETWORKDbContext db)
    {
        _services = services;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetProveedores()
    {
        try
        {
            return Ok(await _services.GetProveedoresAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProveedor([FromBody] ProveedorCreateDto dto)
    {
        try
        {
            var proveedor = await _services.CreateProveedorAsync(dto);
            return StatusCode(StatusCodes.Status201Created, proveedor);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProveedor(int id, [FromBody] ProveedorCreateDto dto)
    {
        try
        {
            var p = await _db.Proveedores.FindAsync(id);
            if (p == null) return NotFound();

            p.RazonSocial = dto.RazonSocial;
            p.Cuit = dto.Cuit;
            p.Telefono = dto.Telefono;
            p.Email = dto.Email;

            await _db.SaveChangesAsync();
            return Ok(p);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProveedor(int id)
    {
        try
        {
            var p = await _db.Proveedores.FirstOrDefaultAsync(x => x.IdProveedor == id);
            if (p == null) return NotFound(new { error = "El proveedor no existe." });

            var tieneCompras = await _db.ComprasStocks.AnyAsync(c => c.IdProveedor == id);

            if (tieneCompras)
            {
                // Tiene historial de compras: se conserva el historico y se da de baja (no aparece mas en los listados).
                p.Activo = false;
                await _db.SaveChangesAsync();
                return Ok(new { message = "El proveedor tiene compras registradas, por lo que se dio de baja y ya no aparecera en los listados (se conserva el historial de compras).", baja = true });
            }

            // Sin compras: los mantenimientos quedan sin proveedor (columna opcional) y se elimina fisicamente.
            await _db.MantenimientoEquipamientos
                .Where(m => m.IdProveedor == id)
                .ExecuteUpdateAsync(s => s.SetProperty(m => m.IdProveedor, (int?)null));

            _db.Proveedores.Remove(p);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Proveedor eliminado", baja = false });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
