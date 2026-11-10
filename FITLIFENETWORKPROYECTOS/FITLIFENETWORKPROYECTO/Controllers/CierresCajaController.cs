using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CierresCajaController : ControllerBase
{
    private readonly IServices _services;

    public CierresCajaController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetCierres([FromQuery] int? idCaja = null)
    {
        return Ok(await _services.GetCierresCajaAsync(idCaja));
    }

    [HttpGet("{idCierre:int}")]
    public async Task<IActionResult> GetById(int idCierre)
    {
        try
        {
            var cierre = await _services.GetCierreCajaByIdAsync(idCierre);
            if (cierre == null)
            {
                return NotFound();
            }

            return Ok(cierre);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCierre([FromBody] CierreCajaCreateDto dto)
    {
        try
        {
            var cierre = await _services.CreateCierreCajaAsync(dto);
            return StatusCode(StatusCodes.Status201Created, cierre);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{idCierre:int}")]
    public async Task<IActionResult> UpdateCierre(int idCierre, [FromBody] CierreCajaCreateDto dto)
    {
        try
        {
            var cierre = await _services.UpdateCierreCajaAsync(idCierre, dto);
            if (cierre == null)
            {
                return NotFound();
            }

            return Ok(cierre);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{idCierre:int}")]
    public async Task<IActionResult> DeleteCierre(int idCierre)
    {
        try
        {
            var eliminado = await _services.DeleteCierreCajaAsync(idCierre);
            if (!eliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
