using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Models.Tablas;

namespace FITLIFENETWORKPROYECTO.Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<Usuario?> ValidateUserAsync(string nombre, string contrasena);
        Task<List<string>> GetUserRolesAsync(int usuarioId);
    }
}
