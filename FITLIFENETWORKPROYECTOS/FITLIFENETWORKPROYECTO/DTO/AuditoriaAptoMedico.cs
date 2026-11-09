using System.ComponentModel.DataAnnotations;

namespace FITLIFENETWORKPROYECTO.DTO
{
    //Consulta 2
    public class AuditoriaAptoMedico
    {
        public int IdSocio { get; set; }
        public string? Dni { get; set; }
        public string? Socio { get; set; }
        public string? SedeDelIntento { get; set; }
        public int IntentosDenegados { get; set; }
        public DateTime UltimoIntentoDenegado { get; set; }
        public DateTime? VencimientoAptoMedico { get; set; }
        public string? MedicoRegistrado { get; set; }
    }
}
