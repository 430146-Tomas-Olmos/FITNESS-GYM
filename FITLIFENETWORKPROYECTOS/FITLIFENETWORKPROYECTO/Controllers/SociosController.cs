using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_MIEMBROS")]
public class SociosController : ControllerBase
{
    private readonly IServices _services;

    public SociosController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var socios = await _services.GetSociosAsync();
            return Ok(socios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{idSocio:int}")]
    public async Task<IActionResult> GetById(int idSocio)
    {
        try
        {
            var socio = await _services.GetSocioByIdAsync(idSocio);
            if (socio == null)
            {
                return NotFound();
            }

            return Ok(socio);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SocioCreateDto dto)
    {
        try
        {
            var socio = await _services.CreateSocioAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idSocio = socio.IdSocio }, socio);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("alta-con-contrato")]
    public async Task<IActionResult> CreateSocioConContrato([FromBody] SocioConContratoCreateDto dto)
    {
        try
        {
            var resultado = await _services.CreateSocioConContratoAsync(dto);
            return StatusCode(StatusCodes.Status201Created, resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{idSocio:int}")]
    public async Task<IActionResult> Update(int idSocio, [FromBody] SocioCreateDto dto)
    {
        try
        {
            var socio = await _services.UpdateSocioAsync(idSocio, dto);
            if (socio == null)
            {
                return NotFound();
            }

            return Ok(socio);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{idSocio:int}")]
    public async Task<IActionResult> Delete(int idSocio)
    {
        try
        {
            var eliminado = await _services.DeleteSocioAsync(idSocio);
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
