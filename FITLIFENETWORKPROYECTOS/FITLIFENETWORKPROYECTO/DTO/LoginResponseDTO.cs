namespace FITLIFENETWORKPROYECTO.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
