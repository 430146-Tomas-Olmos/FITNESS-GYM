using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class AptosMedico
{
    public int IdApto { get; set; }

    public int IdSocio { get; set; }

    public DateOnly FechaEmision { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public string MedicoFirmante { get; set; } = null!;

    public string MatriculaMedico { get; set; } = null!;

    public string Observaciones { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;
}
