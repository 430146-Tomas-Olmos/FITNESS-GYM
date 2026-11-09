using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Views_Dashboard;

public partial class VwStockDisponiblePorSede
{
    public int IdSede { get; set; }

    public string NombreSede { get; set; } = null!;

    public int IdProducto { get; set; }

    public string NombreProducto { get; set; } = null!;

    public int CantidadActual { get; set; }

    public int StockMinimo { get; set; }

    public int StockCritico { get; set; }
}
