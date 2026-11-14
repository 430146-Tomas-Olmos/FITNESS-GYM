using System;
using System.ComponentModel.DataAnnotations;

namespace FITLIFENETWORKPROYECTO.DTO;

public class PagoCuotaCreateDto
{
    [Required]
    public int IdCobro { get; set; }

    [Required]
    public int IdFormaPago { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Importe { get; set; }

    // Opcional: quien ejecuta la acción en el backend
    public string? UsuarioSistema { get; set; }
}
