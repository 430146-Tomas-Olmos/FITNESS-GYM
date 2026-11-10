using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IServices _services;

    public RolesController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _services.GetRolesAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{idRol:int}")]
    public async Task<IActionResult> GetById(int idRol)
    {
        try
        {
            var rol = await _services.GetRoleByIdAsync(idRol);
            if (rol == null)
            {
                return NotFound();
            }

            return Ok(rol);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleCreateDto dto)
    {
        try
        {
            var rol = await _services.CreateRoleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idRol = rol.IdRol }, rol);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
