using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int IdCategoria { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string CodigoBarras { get; set; } = null!;

    public decimal PrecioActual { get; set; }

    public int StockMinimo { get; set; }

    public virtual ICollection<DetalleComprasStock> DetalleComprasStocks { get; set; } = new List<DetalleComprasStock>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<HistorialPreciosProducto> HistorialPreciosProductos { get; set; } = new List<HistorialPreciosProducto>();

    public virtual CategoriasProducto IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<StockSede> StockSedes { get; set; } = new List<StockSede>();
}
