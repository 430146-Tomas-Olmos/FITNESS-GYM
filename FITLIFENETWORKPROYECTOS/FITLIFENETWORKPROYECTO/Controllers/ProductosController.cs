using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FITLIFENETWORKPROYECTO.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ROLE_GERENTE_GENERAL,ROLE_GERENTE_VENTAS,ROLE_VENDEDOR")]
public class ProductosController : ControllerBase
{
    private readonly IServices _services;

    public ProductosController(IServices services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductos([FromQuery] int? idSede = null)
    {
        try
        {
            return Ok(await _services.GetProductosAsync(idSede));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{idProducto:int}")]
    public async Task<IActionResult> GetById(int idProducto)
    {
        try
        {
            var producto = await _services.GetProductoByIdAsync(idProducto);
            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductoCreateDto dto)
    {
        try
        {
            var producto = await _services.CreateProductoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { idProducto = producto.IdProducto }, producto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{idProducto:int}")]
    public async Task<IActionResult> Update(int idProducto, [FromBody] ProductoCreateDto dto)
    {
        try
        {
            var producto = await _services.UpdateProductoAsync(idProducto, dto);
            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{idProducto:int}")]
    public async Task<IActionResult> Delete(int idProducto)
    {
        try
        {
            var eliminado = await _services.DeleteProductoAsync(idProducto);
            if (!eliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("stock")]
    public async Task<IActionResult> GetStockPorSede([FromQuery] int? idSede = null)
    {
        try
        {
            return Ok(await _services.GetStockPorSedeAsync(idSede));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
