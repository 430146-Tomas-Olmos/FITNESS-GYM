namespace FITLIFENETWORKPROYECTO.DTO
{
    //Consulta 3
     public class ControlAptoMedico
      {
             public int NumeroSocio { get; set; }
             public string? Dni { get; set; }
             public string? NombreCompleto { get; set; }
             public string? Telefono { get; set; }
             public DateTime? FechaEmision { get; set; }
             public DateTime? FechaVencimiento { get; set; }
             public string? EstadoAptoMedico { get; set; }
       }
}
