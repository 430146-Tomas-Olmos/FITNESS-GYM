using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class ComprasStock
{
    public int IdCompra { get; set; }

    public int IdProveedor { get; set; }

    public int IdSede { get; set; }

    public DateTime FechaCompra { get; set; }

    public virtual ICollection<DetalleComprasStock> DetalleComprasStocks { get; set; } = new List<DetalleComprasStock>();

    public virtual Proveedore IdProveedorNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;
}
