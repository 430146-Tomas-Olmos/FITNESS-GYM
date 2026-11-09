using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Views_Dashboard;

public partial class VwVentasDetalladasProducto
{
    public int IdVenta { get; set; }

    public int IdSede { get; set; }

    public string NombreSede { get; set; } = null!;

    public int IdVendedor { get; set; }

    public int? IdSocio { get; set; }

    public DateTime FechaHoraVenta { get; set; }

    public bool ImputadoACuenta { get; set; }

    public int IdProducto { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string NombreCategoria { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal PrecioUnitarioHistorico { get; set; }

    public decimal? ImporteLinea { get; set; }

    public string NombreFormaPago { get; set; } = null!;
}
