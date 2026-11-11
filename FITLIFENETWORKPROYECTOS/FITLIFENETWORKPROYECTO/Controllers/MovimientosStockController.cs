using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS")]
public class MovimientosStockController : ControllerBase
{
    private readonly IServices _services;

    public MovimientosStockController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetMovimientos([FromQuery] int? idSede = null, [FromQuery] int? idProducto = null)
    {
        return Ok(await _services.GetMovimientosStockAsync(idSede, idProducto));
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovimiento([FromBody] MovimientoStockCreateDto dto)
    {
        var movimiento = await _services.CreateMovimientoStockAsync(dto);
        return StatusCode(StatusCodes.Status201Created, movimiento);
    }
}
