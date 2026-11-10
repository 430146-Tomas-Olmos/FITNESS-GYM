using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeografiaController : ControllerBase
{
    private readonly IServices _services;

    public GeografiaController(IServices services)
    {
        _services = services;
    }

    [HttpGet("provincias")]
    public async Task<IActionResult> GetProvincias()
    {
        try
        {
            return Ok(await _services.GetProvinciasAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("provincias/{idProvincia:int}")]
    public async Task<IActionResult> GetProvinciaById(int idProvincia)
    {
        try
        {
            var provincia = await _services.GetProvinciaByIdAsync(idProvincia);
            return provincia == null ? NotFound() : Ok(provincia);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("provincias")]
    public async Task<IActionResult> CreateProvincia([FromBody] ProvinciaCreateDto dto)
    {
        try
        {
            var provincia = await _services.CreateProvinciaAsync(dto);
            return CreatedAtAction(nameof(GetProvinciaById), new { idProvincia = provincia.IdProvincia }, provincia);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("localidades")]
    public async Task<IActionResult> GetLocalidades([FromQuery] int? idProvincia = null)
    {
        try
        {
            return Ok(await _services.GetLocalidadesAsync(idProvincia));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("localidades/{idLocalidad:int}")]
    public async Task<IActionResult> GetLocalidadById(int idLocalidad)
    {
        try
        {
            var localidad = await _services.GetLocalidadByIdAsync(idLocalidad);
            return localidad == null ? NotFound() : Ok(localidad);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("localidades")]
    public async Task<IActionResult> CreateLocalidad([FromBody] LocalidadCreateDto dto)
    {
        try
        {
            var localidad = await _services.CreateLocalidadAsync(dto);
            return CreatedAtAction(nameof(GetLocalidadById), new { idLocalidad = localidad.IdLocalidad }, localidad);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("barrios")]
    public async Task<IActionResult> GetBarrios([FromQuery] int? idLocalidad = null)
    {
        try
        {
            return Ok(await _services.GetBarriosAsync(idLocalidad));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("barrios/{idBarrio:int}")]
    public async Task<IActionResult> GetBarrioById(int idBarrio)
    {
        try
        {
            var barrio = await _services.GetBarrioByIdAsync(idBarrio);
            return barrio == null ? NotFound() : Ok(barrio);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("barrios")]
    public async Task<IActionResult> CreateBarrio([FromBody] BarrioCreateDto dto)
    {
        try
        {
            var barrio = await _services.CreateBarrioAsync(dto);
            return CreatedAtAction(nameof(GetBarrioById), new { idBarrio = barrio.IdBarrio }, barrio);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("calles")]
    public async Task<IActionResult> GetCalles([FromQuery] int? idBarrio = null)
    {
        try
        {
            return Ok(await _services.GetCallesAsync(idBarrio));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("calles/{idCalle:int}")]
    public async Task<IActionResult> GetCalleById(int idCalle)
    {
        try
        {
            var calle = await _services.GetCalleByIdAsync(idCalle);
            return calle == null ? NotFound() : Ok(calle);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("calles")]
    public async Task<IActionResult> CreateCalle([FromBody] CalleCreateDto dto)
    {
        try
        {
            var calle = await _services.CreateCalleAsync(dto);
            return CreatedAtAction(nameof(GetCalleById), new { idCalle = calle.IdCalle }, calle);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
