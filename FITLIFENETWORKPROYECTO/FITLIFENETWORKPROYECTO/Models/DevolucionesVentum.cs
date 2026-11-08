using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class DevolucionesVentum
{
    public int IdDevolucion { get; set; }

    public int IdVenta { get; set; }

    public DateTime FechaDevolucion { get; set; }

    public string Motivo { get; set; } = null!;

    public int IdEmpleado { get; set; }

    public decimal ImporteReintegrado { get; set; }

    public virtual ICollection<DetalleDevolucionesVentum> DetalleDevolucionesVenta { get; set; } = new List<DetalleDevolucionesVentum>();

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
