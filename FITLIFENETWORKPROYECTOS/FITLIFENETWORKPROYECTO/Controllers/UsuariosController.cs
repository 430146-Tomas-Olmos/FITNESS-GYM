using FITLIFENETWORKPROYECTO.Models.Context;
using FITLIFENETWORKPROYECTO.Models.Tablas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FITLIFENETWORKPROYECTO.Controllers;

public class UsuarioItemDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string Sede { get; set; } = "Casa Central";
    public string Estado { get; set; } = "Activo";
    public string UltimoAcceso { get; set; } = "Reciente";
}

public class UsuarioCreateRequestDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Contrasena { get; set; }
    public string Rol { get; set; } = "Gerente General";
    public string? Sede { get; set; }
    public string? Estado { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly FITLIFENETWORKDbContext _db;

    public UsuariosController(FITLIFENETWORKDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var usuarios = await _db.Usuarios
                .Include(u => u.Idrols)
                .AsNoTracking()
                .ToListAsync();

            var dtos = usuarios.Select(u => new UsuarioItemDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Alias = u.Nombre,
                Email = $"{u.Nombre.ToLower().Replace(" ", ".")}@fitlife.com",
                Rol = u.Idrols.Select(r => r.Rol).FirstOrDefault() ?? "Gerente General",
                Sede = "Casa Central",
                Estado = "Activo",
                UltimoAcceso = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            }).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var u = await _db.Usuarios.Include(u => u.Idrols).FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) return NotFound();

            return Ok(new UsuarioItemDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Alias = u.Nombre,
                Email = $"{u.Nombre.ToLower().Replace(" ", ".")}@fitlife.com",
                Rol = u.Idrols.Select(r => r.Rol).FirstOrDefault() ?? "Gerente General",
                Sede = "Casa Central",
                Estado = "Activo",
                UltimoAcceso = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UsuarioCreateRequestDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Alias) && string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return BadRequest(new { error = "El nombre de usuario es requerido" });
            }

            var nombreUsuario = !string.IsNullOrWhiteSpace(dto.Alias) ? dto.Alias.Trim() : dto.Nombre.Trim();
            var password = !string.IsNullOrWhiteSpace(dto.Contrasena) ? dto.Contrasena : "123456";

            var nuevoUsuario = new Usuario
            {
                Nombre = nombreUsuario,
                Contrasena = password
            };

            // Map rol
            var rolSistema = await _db.RolesSistemas.FirstOrDefaultAsync(r => r.Rol == dto.Rol || r.Rol.Contains(dto.Rol));
            if (rolSistema != null)
            {
                nuevoUsuario.Idrols.Add(rolSistema);
            }

            _db.Usuarios.Add(nuevoUsuario);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = nuevoUsuario.Id }, new UsuarioItemDto
            {
                Id = nuevoUsuario.Id,
                Nombre = dto.Nombre ?? nuevoUsuario.Nombre,
                Alias = nuevoUsuario.Nombre,
                Email = !string.IsNullOrWhiteSpace(dto.Email) ? dto.Email : $"{nuevoUsuario.Nombre.ToLower()}@fitlife.com",
                Rol = dto.Rol,
                Sede = dto.Sede ?? "Casa Central",
                Estado = dto.Estado ?? "Activo",
                UltimoAcceso = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UsuarioCreateRequestDto dto)
    {
        try
        {
            var u = await _db.Usuarios.Include(x => x.Idrols).FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Alias)) u.Nombre = dto.Alias.Trim();
            else if (!string.IsNullOrWhiteSpace(dto.Nombre)) u.Nombre = dto.Nombre.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Contrasena)) u.Contrasena = dto.Contrasena;

            if (!string.IsNullOrWhiteSpace(dto.Rol))
            {
                var rol = await _db.RolesSistemas.FirstOrDefaultAsync(r => r.Rol == dto.Rol || r.Rol.Contains(dto.Rol));
                if (rol != null)
                {
                    u.Idrols.Clear();
                    u.Idrols.Add(rol);
                }
            }

            await _db.SaveChangesAsync();
            return Ok(new UsuarioItemDto
            {
                Id = u.Id,
                Nombre = dto.Nombre ?? u.Nombre,
                Alias = u.Nombre,
                Email = dto.Email ?? $"{u.Nombre.ToLower()}@fitlife.com",
                Rol = dto.Rol,
                Sede = dto.Sede ?? "Casa Central",
                Estado = dto.Estado ?? "Activo",
                UltimoAcceso = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var u = await _db.Usuarios.Include(x => x.Idrols).FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) return NotFound();

            u.Idrols.Clear();
            _db.Usuarios.Remove(u);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Usuario eliminado con éxito" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
