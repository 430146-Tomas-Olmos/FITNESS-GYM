using FITLIFENETWORKPROYECTO.DTO;
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

    public CajasController(IServices services)
    {
        _services = services;
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
}
