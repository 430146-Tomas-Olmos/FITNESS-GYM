using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Proveedore
{
    public int IdProveedor { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string Cuit { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<ComprasStock> ComprasStocks { get; set; } = new List<ComprasStock>();

    public virtual ICollection<MantenimientoEquipamiento> MantenimientoEquipamientos { get; set; } = new List<MantenimientoEquipamiento>();
}
