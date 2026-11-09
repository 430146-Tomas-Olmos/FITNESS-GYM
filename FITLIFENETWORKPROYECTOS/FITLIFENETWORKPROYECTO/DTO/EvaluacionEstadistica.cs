namespace FITLIFENETWORKPROYECTO.DTO
{
    //Consulta 8
    public class EvaluacionEstadistica
    {
        public string SedeConsultorio { get; set; }
        public string ProfesionalACargo { get; set; }
        public string Especialidad { get; set; }
        public int EvaluacionesRealizadas { get; set; }
        public decimal? PesoPromedioKg { get; set; }
        public decimal? PorcentajeGrasaPromedio { get; set; }
        public decimal? PorcentajeMasaMuscularPromedio { get; set; }
    }
}
