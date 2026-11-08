using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class VwCuotasPendiente
{
    public int IdCobro { get; set; }

    public int IdContrato { get; set; }

    public int IdSocio { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int NumeroCuota { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public decimal Monto { get; set; }

    public decimal ImportePagado { get; set; }

    public decimal? Saldo { get; set; }

    public string EstadoCuota { get; set; } = null!;
}
