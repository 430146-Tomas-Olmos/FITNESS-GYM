using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccesosController : ControllerBase
{
    private readonly IServices _services;

    public AccesosController(IServices services)
    {
        _services = services;
    }

    [HttpPost("validar")]
    public async Task<IActionResult> ValidarAcceso([FromBody] AccesoValidacionRequestDto dto)
    {
        try
        {
            var resultado = await _services.ValidarAccesoAsync(dto.IdSocio, dto.IdSede, dto.FechaHoraMarcado);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("historial/{idSocio:int}")]
    public async Task<IActionResult> GetHistorial(int idSocio)
    {
        try
        {
            var historial = await _services.GetHistorialSocioAsync(idSocio);
            return Ok(historial);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("lista-espera/{idClase:int}")]
    public async Task<IActionResult> GetListaEspera(int idClase)
    {
        try
        {
            var lista = await _services.GetReservasEnListaEsperaAsync(idClase);
            return Ok(lista);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("lista-espera/promover/{idClase:int}")]
    public async Task<IActionResult> PromoverListaEspera(int idClase)
    {
        try
        {
            var reserva = await _services.PromoverSiguienteListaEsperaAsync(idClase);
            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
