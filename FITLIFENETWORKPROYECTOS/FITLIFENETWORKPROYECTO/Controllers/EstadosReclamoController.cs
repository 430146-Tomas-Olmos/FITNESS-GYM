using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstadosReclamoController : ControllerBase
{
    private readonly IServices _services;

    public EstadosReclamoController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _services.GetEstadosReclamoAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idEstadoReclamo:int}")]
    public async Task<IActionResult> GetById(int idEstadoReclamo)
    {
        try
        {
            var estado = await _services.GetEstadoReclamoByIdAsync(idEstadoReclamo);
            return estado == null ? NotFound() : Ok(estado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EstadoReclamoCreateDto dto)
    {
        try
        {
            var estado = await _services.CreateEstadoReclamoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idEstadoReclamo = estado.IdEstadoReclamo }, estado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
