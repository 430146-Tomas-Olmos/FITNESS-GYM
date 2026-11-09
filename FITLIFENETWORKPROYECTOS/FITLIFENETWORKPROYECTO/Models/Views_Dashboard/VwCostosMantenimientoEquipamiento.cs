using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Views_Dashboard;

public partial class VwCostosMantenimientoEquipamiento
{
    public int IdEquipo { get; set; }

    public string NombreMaquina { get; set; } = null!;

    public int IdSede { get; set; }

    public string NombreSede { get; set; } = null!;

    public int? CantidadIntervenciones { get; set; }

    public decimal CostoTotal { get; set; }

    public DateOnly? UltimoMantenimiento { get; set; }
}
