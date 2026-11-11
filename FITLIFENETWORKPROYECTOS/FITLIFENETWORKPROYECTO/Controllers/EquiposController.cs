using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_EQUIPAMIENTO,ROLE_TECNICO")]
public class EquiposController : ControllerBase
{
    private readonly IServices _services;

    public EquiposController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetEquipos([FromQuery] int? idSede = null)
    {
        try
        {
            return Ok(await _services.GetEquiposAsync(idSede));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
