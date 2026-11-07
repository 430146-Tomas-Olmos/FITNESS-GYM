using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class HistorialPreciosProducto
{
    public int IdHistorialProd { get; set; }

    public int IdProducto { get; set; }

    public decimal PrecioAnterior { get; set; }

    public decimal PrecioNuevo { get; set; }

    public DateTime FechaCambio { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
