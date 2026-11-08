using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class PagosCuota
{
    public int IdPago { get; set; }

    public int IdCobro { get; set; }

    public int IdFormaPago { get; set; }

    public decimal Importe { get; set; }

    public DateTime FechaPago { get; set; }

    public string? UsuarioSistema { get; set; }

    public virtual CuotasCobro IdCobroNavigation { get; set; } = null!;

    public virtual FormasPago IdFormaPagoNavigation { get; set; } = null!;
}
