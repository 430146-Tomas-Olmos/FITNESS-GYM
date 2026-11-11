using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_MIEMBROS,ROLE_GERENTE_VENTAS,ROLE_VENDEDOR")]
public class CuotasController : ControllerBase
{
    private readonly IServices _services;

    public CuotasController(IServices services)
    {
        _services = services;
    }

    [HttpGet("pendientes")]
    public async Task<IActionResult> GetCuotasPendientes([FromQuery] int? idSocio = null)
    {
        try
        {
            return Ok(await _services.GetCuotasPendientesAsync(idSocio));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("historial")]
    public async Task<IActionResult> GetHistorial([FromQuery] int? idCobro = null, [FromQuery] int? idSocio = null)
    {
        try
        {
            return Ok(await _services.GetPagosCuotaAsync(idCobro, idSocio));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("cobrar")]
    public async Task<IActionResult> CobrarCuota([FromBody] PagoCuotaCreateDto dto)
    {
        try
        {
            var pago = await _services.RegistrarPagoCuotaAsync(dto);
            return StatusCode(StatusCodes.Status201Created, pago);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
