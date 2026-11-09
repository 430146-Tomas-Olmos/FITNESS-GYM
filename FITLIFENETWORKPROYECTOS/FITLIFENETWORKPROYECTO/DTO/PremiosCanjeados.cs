namespace FITLIFENETWORKPROYECTO.DTO
{
    //Consulta 5
    public class PremiosCanjeados
    {
        public int IdSocio { get; set; }
        public string Dni { get; set; }
        public string Socio { get; set; }
        public DateTime FechaAlta { get; set; }
        public int AntiguedadRealMeses { get; set; }
        public string PremioCatalogo { get; set; }
        public int MesesExigidos { get; set; }
        public DateTime? FechaOtorgado { get; set; }
        public string EstadoCanje { get; set; }
        public string EmailContacto { get; set; }
    
    }
}
