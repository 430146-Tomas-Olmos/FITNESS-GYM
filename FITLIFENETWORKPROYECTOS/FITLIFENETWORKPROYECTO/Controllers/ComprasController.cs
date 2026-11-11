using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS,ROLE_GERENTE_EQUIPAMIENTO")]
public class ComprasController : ControllerBase
{
    private readonly IServices _services;

    public ComprasController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompras([FromQuery] int? idSede = null)
    {
        try
        {
            return Ok(await _services.GetComprasAsync(idSede));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompra([FromBody] CompraStockCreateDto dto)
    {
        try
        {
            var compra = await _services.CreateCompraAsync(dto);
            return StatusCode(StatusCodes.Status201Created, compra);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
