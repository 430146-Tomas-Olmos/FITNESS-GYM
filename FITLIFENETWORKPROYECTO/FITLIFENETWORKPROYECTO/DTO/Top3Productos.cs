namespace FITLIFENETWORKPROYECTO.DTO
{
    public class Top3Productos
    {
        public string Sede { get; set; }
        public long PuestoRanking { get; set; }
        public string Producto { get; set; }
        public string Categoría { get; set; }
        int UnidadesVendidas { get; set; }
        public decimal FacturacionTotal { get; set; }
        public decimal MontoCtaCte { get; set; }
        public decimal? PctImputadoCtaCte { get; set; }
    }
}
