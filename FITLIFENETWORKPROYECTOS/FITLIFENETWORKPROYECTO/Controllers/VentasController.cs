using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS,ROLE_VENDEDOR")]
public class VentasController : ControllerBase
{
    private readonly IServices _services;

    public VentasController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? idSede = null, [FromQuery] int? idSocio = null)
    {
        try
        {
            return Ok(await _services.GetVentasAsync(idSede, idSocio));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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
