using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class HistorialPreciosPlane
{
    public int IdHistorialPlan { get; set; }

    public int IdPlan { get; set; }

    public decimal PrecioAnterior { get; set; }

    public decimal PrecioNuevo { get; set; }

    public DateTime FechaCambio { get; set; }

    public virtual Plane IdPlanNavigation { get; set; } = null!;
}
