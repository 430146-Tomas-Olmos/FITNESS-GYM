using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevolucionesController : ControllerBase
{
    private readonly IServices _services;

    public DevolucionesController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetDevoluciones([FromQuery] int? idVenta = null)
    {
        try
        {
            return Ok(await _services.GetDevolucionesAsync(idVenta));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateDevolucion([FromBody] DevolucionVentaCreateDto dto)
    {
        try
        {
            var devolucion = await _services.CreateDevolucionAsync(dto);
            return StatusCode(StatusCodes.Status201Created, devolucion);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
