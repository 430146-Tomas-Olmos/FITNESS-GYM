using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonasController : ControllerBase
{
    private readonly IServices _services;

    public PersonasController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _services.GetPersonasAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{idPersona:int}")]
    public async Task<IActionResult> GetById(int idPersona)
    {
        try
        {
            var persona = await _services.GetPersonaByIdAsync(idPersona);
            if (persona == null)
            {
                return NotFound();
            }

            return Ok(persona);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonaCreateDto dto)
    {
        try
        {
            var persona = await _services.CreatePersonaAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idPersona = persona.IdPersona }, persona);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
