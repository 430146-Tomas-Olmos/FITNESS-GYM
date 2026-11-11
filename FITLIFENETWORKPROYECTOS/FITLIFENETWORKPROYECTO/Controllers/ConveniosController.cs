using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_MIEMBROS")]
public class ConveniosController : ControllerBase
{
    private readonly IServices _services;

    public ConveniosController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetConvenios()
    {
        try
        {
            return Ok(await _services.GetConveniosAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idConvenio:int}")]
    public async Task<IActionResult> GetById(int idConvenio)
    {
        try
        {
            var convenio = await _services.GetConvenioByIdAsync(idConvenio);
            if (convenio == null)
            {
                return NotFound();
            }

            return Ok(convenio);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateConvenio([FromBody] ConvenioEmpresaCreateDto dto)
    {
        try
        {
            var convenio = await _services.CreateConvenioAsync(dto);
            return StatusCode(StatusCodes.Status201Created, convenio);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{idConvenio:int}")]
    public async Task<IActionResult> UpdateConvenio(int idConvenio, [FromBody] ConvenioEmpresaCreateDto dto)
    {
        try
        {
            var convenio = await _services.UpdateConvenioAsync(idConvenio, dto);
            if (convenio == null)
            {
                return NotFound();
            }

            return Ok(convenio);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{idConvenio:int}")]
    public async Task<IActionResult> DeleteConvenio(int idConvenio)
    {
        try
        {
            var eliminado = await _services.DeleteConvenioAsync(idConvenio);
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
