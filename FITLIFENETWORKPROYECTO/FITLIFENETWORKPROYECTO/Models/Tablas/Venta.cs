using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int? IdSocio { get; set; }

    public int IdSede { get; set; }

    public int IdVendedor { get; set; }

    public int IdFormaPago { get; set; }

    public DateTime FechaHoraVenta { get; set; }

    public bool ImputadoACuenta { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<DevolucionesVentum> DevolucionesVenta { get; set; } = new List<DevolucionesVentum>();

    public virtual FormasPago IdFormaPagoNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual Socio? IdSocioNavigation { get; set; }

    public virtual Empleado IdVendedorNavigation { get; set; } = null!;

    public virtual ICollection<MovimientosStock> MovimientosStocks { get; set; } = new List<MovimientosStock>();
}
