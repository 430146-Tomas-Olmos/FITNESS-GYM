namespace FITLIFENETWORKPROYECTO.DTO
{
    public class ReporteReservaTT
    {
        public int IdReserva { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaClase { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public string EstadoReserva { get; set; }
    }
}

