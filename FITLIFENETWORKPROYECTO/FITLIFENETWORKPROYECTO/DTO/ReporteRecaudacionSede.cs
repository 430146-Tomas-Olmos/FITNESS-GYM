using System.ComponentModel.DataAnnotations;
namespace FITLIFENETWORKPROYECTO.DTO
{
    public class ReporteRecaudacionSede
    {
        public string? Sede { get; set; }
        public string? PlanMembresia { get; set; }
        public int CuotasCobradas { get; set; }
        public decimal TotalRecaudado { get; set; }
        public decimal MontoPromedio { get; set; }
        public decimal ParticipacionSedePorcentaje { get; set; }
    }
}