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
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS,ROLE_GERENTE_EQUIPAMIENTO")]
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
            var p = await _db.Proveedores.FindAsync(id);
            if (p == null) return NotFound();

            _db.Proveedores.Remove(p);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Proveedor eliminado" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
