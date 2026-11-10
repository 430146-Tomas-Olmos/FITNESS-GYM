using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VentasController : ControllerBase
{
    private readonly IServices _services;

    public VentasController(IServices services)
    {
        _services = services;
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] VentaCreateDto dto)
    {
        try
        {
            var venta = await _services.RegistrarVentaAsync(dto);
            return StatusCode(StatusCodes.Status201Created, venta);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
