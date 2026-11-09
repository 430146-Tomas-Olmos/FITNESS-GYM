using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class CuotasCobro
{
    public int IdCobro { get; set; }

    public int IdContrato { get; set; }

    public int IdFormaPago { get; set; }

    public int NumeroCuota { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public DateTime? FechaPago { get; set; }

    public decimal Monto { get; set; }

    public virtual ContratosMembresium IdContratoNavigation { get; set; } = null!;

    public virtual FormasPago IdFormaPagoNavigation { get; set; } = null!;

    public virtual ICollection<PagosCuota> PagosCuota { get; set; } = new List<PagosCuota>();
}
