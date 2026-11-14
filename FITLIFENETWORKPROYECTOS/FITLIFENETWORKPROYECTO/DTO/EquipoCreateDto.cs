using System;

namespace FITLIFENETWORKPROYECTO.DTO
{
    public class EquipoCreateDto
    {
        public string NombreMaquina { get; set; }
        public int IdSede { get; set; }
        public string NumeroSerie { get; set; }
        public DateTime FechaAdquisicion { get; set; }
        public string EstadoOperativo { get; set; }
    }
}
