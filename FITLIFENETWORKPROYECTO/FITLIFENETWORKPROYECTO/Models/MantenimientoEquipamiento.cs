using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class MantenimientoEquipamiento
{
    public int IdMantenimiento { get; set; }

    public int IdEquipo { get; set; }

    public int? IdProveedor { get; set; }

    public DateOnly FechaMantenimiento { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Costo { get; set; }

    public virtual Equipamiento IdEquipoNavigation { get; set; } = null!;

    public virtual Proveedore? IdProveedorNavigation { get; set; }
}
