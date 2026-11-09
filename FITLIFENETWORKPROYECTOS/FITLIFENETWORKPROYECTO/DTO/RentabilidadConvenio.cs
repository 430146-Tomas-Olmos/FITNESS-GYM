namespace FITLIFENETWORKPROYECTO.DTO
{
    //4
    public class RentabilidadConvenio
    {
        public string EmpresaConvenio { get; set; }
        public string Cuit { get; set; }
        public decimal PorcentajeDescuentoAcordado { get; set; }
        public int SociosAdheridos { get; set; }
        public int ContratosGenerados { get; set; }
        public decimal MontoTotalFacturado { get; set; }
        public decimal MontoCobrado { get; set; }
        public decimal MontoEnMora { get; set; }
    }
}
