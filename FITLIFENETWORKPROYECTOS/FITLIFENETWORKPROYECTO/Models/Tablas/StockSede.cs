using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class StockSede
{
    public int IdStockSede { get; set; }

    public int IdSede { get; set; }

    public int IdProducto { get; set; }

    public int CantidadActual { get; set; }

    public byte[] FilaVersion { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;
}
