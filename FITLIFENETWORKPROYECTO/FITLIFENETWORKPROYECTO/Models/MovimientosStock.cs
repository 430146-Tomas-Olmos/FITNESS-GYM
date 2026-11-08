using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class MovimientosStock
{
    public int IdMovimiento { get; set; }

    public int IdSede { get; set; }

    public int IdProducto { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public int Cantidad { get; set; }

    public DateTime FechaMovimiento { get; set; }

    public int? IdVenta { get; set; }

    public string? UsuarioSistema { get; set; }

    public string? Observaciones { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual Venta? IdVentaNavigation { get; set; }
}
