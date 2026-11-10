using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormasPagoController : ControllerBase
{
    private readonly IServices _services;

    public FormasPagoController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _services.GetFormasPagoAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idFormaPago:int}")]
    public async Task<IActionResult> GetById(int idFormaPago)
    {
        try
        {
            var formaPago = await _services.GetFormaPagoByIdAsync(idFormaPago);
            return formaPago == null ? NotFound() : Ok(formaPago);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FormaPagoCreateDto dto)
    {
        try
        {
            var formaPago = await _services.CreateFormaPagoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idFormaPago = formaPago.IdFormaPago }, formaPago);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
