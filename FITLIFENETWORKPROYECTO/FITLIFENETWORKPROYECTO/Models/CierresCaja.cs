using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class CierresCaja
{
    public int IdCierre { get; set; }

    public int IdCaja { get; set; }

    public DateTime FechaCierre { get; set; }

    public decimal TotalVentas { get; set; }

    public decimal SaldoEsperado { get; set; }

    public decimal SaldoReal { get; set; }

    public decimal Diferencia { get; set; }

    public string? Observaciones { get; set; }

    public virtual Caja IdCajaNavigation { get; set; } = null!;
}
