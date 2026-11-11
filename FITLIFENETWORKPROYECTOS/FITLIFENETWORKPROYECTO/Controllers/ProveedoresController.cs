using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS,ROLE_GERENTE_EQUIPAMIENTO")]
public class ProveedoresController : ControllerBase
{
    private readonly IServices _services;

    public ProveedoresController(IServices services)
    {
        _services = services;
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
}
