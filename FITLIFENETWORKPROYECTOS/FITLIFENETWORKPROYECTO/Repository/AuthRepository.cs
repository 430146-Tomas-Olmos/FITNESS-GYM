using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Models.Context;
using FITLIFENETWORKPROYECTO.Models.Tablas;
using FITLIFENETWORKPROYECTO.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FITLIFENETWORKPROYECTO.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly FITLIFENETWORKDbContext _db;

        public AuthRepository(FITLIFENETWORKDbContext db)
        {
            _db = db;
        }

        public async Task<Usuario?> ValidateUserAsync(string nombre, string contrasena)
        {
            return await _db.Usuarios
                .FirstOrDefaultAsync(x => x.Nombre == nombre && x.Contrasena == contrasena);
        }

        public async Task<List<string>> GetUserRolesAsync(int usuarioId)
        {
            return await _db.Usuarios
                .Where(u => u.Id == usuarioId)
                .SelectMany(u => u.Idrols.Select(r => r.Rol))
                .ToListAsync();
        }
    }
}
