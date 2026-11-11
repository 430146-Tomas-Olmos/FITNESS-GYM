using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SedesController : ControllerBase
{
    private readonly IServices _services;

    public SedesController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _services.GetSedesAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{idSede:int}")]
    public async Task<IActionResult> GetById(int idSede)
    {
        try
        {
            var sede = await _services.GetSedeByIdAsync(idSede);
            if (sede == null)
            {
                return NotFound();
            }

            return Ok(sede);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SedeCreateDto dto)
    {
        try
        {
            var sede = await _services.CreateSedeAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idSede = sede.IdSede }, sede);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
