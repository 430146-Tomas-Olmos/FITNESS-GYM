namespace FITLIFENETWORKPROYECTO.DTO
{
    public class AuditoriaAccesoDto
    {
        public int IdLog { get; set; }
        public int IdSocio { get; set; }
        public string NombreSocio { get; set; } = string.Empty;
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string MotivoBloqueo { get; set; } = string.Empty;
    }

    public class AuditoriaEstadoSocioDto
    {
        public int IdLogEstado { get; set; }
        public int IdSocio { get; set; }
        public string NombreSocio { get; set; } = string.Empty;
        public string EstadoAnterior { get; set; } = string.Empty;
        public string EstadoNuevo { get; set; } = string.Empty;
        public DateTime FechaCambio { get; set; }
        public string UsuarioSistema { get; set; } = string.Empty;
    }

    public class AuditoriaEquipamientoDto
    {
        public int IdAuditoria { get; set; }
        public int IdEquipo { get; set; }
        public string NombreEquipo { get; set; } = string.Empty;
        public string EstadoAnterior { get; set; } = string.Empty;
        public string EstadoNuevo { get; set; } = string.Empty;
        public DateTime FechaCambio { get; set; }
        public string? UsuarioSistema { get; set; }
    }
}
