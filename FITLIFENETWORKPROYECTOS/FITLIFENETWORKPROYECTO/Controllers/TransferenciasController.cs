using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS")]
public class TransferenciasController : ControllerBase
{
    private readonly IServices _services;

    public TransferenciasController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransferencias([FromQuery] int? idSede = null)
    {
        try
        {
            return Ok(await _services.GetTransferenciasAsync(idSede));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idTransferencia:int}")]
    public async Task<IActionResult> GetById(int idTransferencia)
    {
        try
        {
            var transferencia = await _services.GetTransferenciaByIdAsync(idTransferencia);
            if (transferencia == null)
            {
                return NotFound();
            }

            return Ok(transferencia);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransferencia([FromBody] TransferenciaStockCreateDto dto)
    {
        try
        {
            var transferencia = await _services.CreateTransferenciaAsync(dto);
            return StatusCode(StatusCodes.Status201Created, transferencia);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{idTransferencia:int}")]
    public async Task<IActionResult> UpdateTransferencia(int idTransferencia, [FromBody] TransferenciaStockCreateDto dto)
    {
        try
        {
            var transferencia = await _services.UpdateTransferenciaAsync(idTransferencia, dto);
            if (transferencia == null)
            {
                return NotFound();
            }

            return Ok(transferencia);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{idTransferencia:int}")]
    public async Task<IActionResult> DeleteTransferencia(int idTransferencia)
    {
        try
        {
            var eliminado = await _services.DeleteTransferenciaAsync(idTransferencia);
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
