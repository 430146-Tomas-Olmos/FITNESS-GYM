using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmpleadosController : ControllerBase
{
    private readonly IServices _services;

    public EmpleadosController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    [Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS,ROLE_GERENTE_EQUIPAMIENTO,ROLE_VENDEDOR,ROLE_TECNICO")]
    public async Task<IActionResult> GetEmpleados([FromQuery] int? idSede = null, [FromQuery] int? idRol = null)
    {
        try
        {
            return Ok(await _services.GetEmpleadosAsync(idSede, idRol));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS,ROLE_GERENTE_EQUIPAMIENTO")]
    public async Task<IActionResult> CreateEmpleado([FromBody] EmpleadoCreateDto dto)
    {
        try
        {
            return Ok(await _services.CreateEmpleadoAsync(dto));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
