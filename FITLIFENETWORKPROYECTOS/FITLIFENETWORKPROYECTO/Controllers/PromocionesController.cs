using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_MIEMBROS,ROLE_GERENTE_VENTAS,ROLE_VENDEDOR")]
public class PromocionesController : ControllerBase
{
    private readonly IServices _services;

    public PromocionesController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetPromociones()
    {
        return Ok(await _services.GetPromocionesAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreatePromocion([FromBody] PromocionCreateDto dto)
    {
        var promocion = await _services.CreatePromocionAsync(dto);
        return StatusCode(StatusCodes.Status201Created, promocion);
    }
}
