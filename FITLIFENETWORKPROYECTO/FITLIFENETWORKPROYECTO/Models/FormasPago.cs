using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class FormasPago
{
    public int IdFormaPago { get; set; }

    public string NombreFormaPago { get; set; } = null!;

    public virtual ICollection<CuotasCobro> CuotasCobros { get; set; } = new List<CuotasCobro>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
