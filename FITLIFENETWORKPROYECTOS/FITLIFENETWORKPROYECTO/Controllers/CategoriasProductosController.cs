using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS,ROLE_VENDEDOR")]
public class CategoriasProductosController : ControllerBase
{
    private readonly IServices _services;

    public CategoriasProductosController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _services.GetCategoriasProductosAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idCategoria:int}")]
    public async Task<IActionResult> GetById(int idCategoria)
    {
        try
        {
            var categoria = await _services.GetCategoriaProductoByIdAsync(idCategoria);
            return categoria == null ? NotFound() : Ok(categoria);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoriaProductoCreateDto dto)
    {
        try
        {
            var categoria = await _services.CreateCategoriaProductoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idCategoria = categoria.IdCategoria }, categoria);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
