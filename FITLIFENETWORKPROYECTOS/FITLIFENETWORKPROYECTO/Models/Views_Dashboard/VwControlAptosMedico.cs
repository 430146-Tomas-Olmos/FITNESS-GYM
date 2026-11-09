using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Views_Dashboard;

public partial class VwControlAptosMedico
{
    public int IdSocio { get; set; }

    public string Dni { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int? IdApto { get; set; }

    public DateOnly? FechaEmision { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    public string EstadoApto { get; set; } = null!;
}
