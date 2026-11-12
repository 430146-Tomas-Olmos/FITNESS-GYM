using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_MIEMBROS,ROLE_GERENTE_VENTAS,ROLE_GERENTE_EQUIPAMIENTO")]
public class DashboardController : ControllerBase
{
    private readonly IServices _services;

    public DashboardController(IServices services)
    {
        _services = services;
    }

    [HttpGet("recaudacion-sede")]
    public async Task<IActionResult> GetRecaudacionSede([FromQuery] int? anio, [FromQuery] int? mes)
    {
        try
        {
            return Ok(await _services.GetRecaudacionSede(anio, mes));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("aptos-medicos")]
    public async Task<IActionResult> GetControlAptosMedicos([FromQuery] string? estados)
    {
        try
        {
            return Ok(await _services.GetControlAptosMedicos(estados));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("accesos-denegados")]
    public async Task<IActionResult> GetAccesosDenegados([FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
    {
        try
        {
            return Ok(await _services.GetAccesoDenegadoApto(desde, hasta));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("top-productos")]
    public async Task<IActionResult> GetTopProductos([FromQuery] int? anio)
    {
        try
        {
            return Ok(await _services.GetTop3Productos(anio));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("rentabilidad-convenios")]
    public async Task<IActionResult> GetRentabilidadConvenios([FromQuery] string? estado)
    {
        try
        {
            return Ok(await _services.GetRentabilidadConvenios(estado));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("estado-cuenta-socios")]
    public async Task<IActionResult> GetEstadoCuentaSocios()
    {
        try
        {
            return Ok(await _services.GetEstadoCuentaSociosAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("membresias-vigentes")]
    public async Task<IActionResult> GetMembresiasVigentes()
    {
        try
        {
            return Ok(await _services.GetMembresiasVigentesAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("ocupacion-clases")]
    public async Task<IActionResult> GetOcupacionClases()
    {
        try
        {
            return Ok(await _services.GetOcupacionClasesAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("costos-mantenimiento")]
    public async Task<IActionResult> GetCostosMantenimiento()
    {
        try
        {
            return Ok(await _services.GetCostosMantenimientoAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("premios-canjeados")]
    public async Task<IActionResult> GetPremiosCanjeados([FromQuery] string? estado, [FromQuery] int minMeses = 6)
    {
        try
        {
            return Ok(await _services.GetPremiosCanjeados(estado, minMeses));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("reporte-reservas")]
    public async Task<IActionResult> GetReporteReservas(
        [FromQuery] DateTime desde,
        [FromQuery] DateTime hasta,
        [FromQuery] string disciplina1,
        [FromQuery] string? disciplina2)
    {
        try
        {
            return Ok(await _services.GetReporteReserva(desde, hasta, disciplina1, disciplina2 ?? string.Empty));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("evaluaciones")]
    public async Task<IActionResult> GetEvaluacionesEstadistica([FromQuery] DateTime desde, [FromQuery] DateTime hasta)
    {
        try
        {
            return Ok(await _services.GetEvaluacionesEstadistica(desde, hasta));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
