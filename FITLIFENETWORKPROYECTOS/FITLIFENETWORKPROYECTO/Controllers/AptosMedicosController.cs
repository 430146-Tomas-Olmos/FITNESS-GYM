using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_MIEMBROS")]
public class AptosMedicosController : ControllerBase
{
    private readonly IServices _services;

    public AptosMedicosController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var aptos = await _services.GetAptosMedicosAsync();
            return Ok(aptos);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AptoMedicoCreateDto dto)
    {
        try
        {
            var apto = await _services.CreateAptoMedicoAsync(dto);
            return StatusCode(StatusCodes.Status201Created, apto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
