using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContratosController : ControllerBase
{
    private readonly IServices _services;

    public ContratosController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetContratos([FromQuery] int? idSocio = null)
    {
        try
        {
            return Ok(await _services.GetContratosAsync(idSocio));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idContrato:int}")]
    public async Task<IActionResult> GetContratoById(int idContrato)
    {
        try
        {
            var contrato = await _services.GetContratoByIdAsync(idContrato);
            return contrato == null ? NotFound() : Ok(contrato);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateContrato([FromBody] ContratoMembresiaCreateDto dto)
    {
        try
        {
            var contrato = await _services.CreateContratoAsync(dto);
            return StatusCode(StatusCodes.Status201Created, contrato);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{idContrato:int}/renovar")]
    public async Task<IActionResult> RenovarContrato(int idContrato, [FromBody] ContratoMembresiaCreateDto dto)
    {
        try
        {
            var contrato = await _services.RenovarContratoAsync(idContrato, dto);
            return StatusCode(StatusCodes.Status201Created, contrato);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{idContrato:int}")]
    public async Task<IActionResult> UpdateContrato(int idContrato, [FromBody] ContratoMembresiaCreateDto dto)
    {
        try
        {
            var contrato = await _services.UpdateContratoAsync(idContrato, dto);
            return contrato == null ? NotFound() : Ok(contrato);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{idContrato:int}")]
    public async Task<IActionResult> DeleteContrato(int idContrato)
    {
        try
        {
            var eliminado = await _services.DeleteContratoAsync(idContrato);
            return eliminado ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
