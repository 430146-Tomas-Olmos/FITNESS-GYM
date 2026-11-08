using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class VwEstadoCuentaSocio
{
    public int IdSocio { get; set; }

    public string Dni { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int? CuotasEmitidas { get; set; }

    public decimal ImporteEmitido { get; set; }

    public decimal ImportePagado { get; set; }

    public decimal? SaldoPendiente { get; set; }
}
