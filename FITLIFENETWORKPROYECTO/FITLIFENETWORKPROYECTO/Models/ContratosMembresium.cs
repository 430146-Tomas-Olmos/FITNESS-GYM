using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class ContratosMembresium
{
    public int IdContrato { get; set; }

    public int IdSocio { get; set; }

    public int IdPlan { get; set; }

    public int IdSede { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public string EstadoContrato { get; set; } = null!;

    public virtual ICollection<CuotasCobro> CuotasCobros { get; set; } = new List<CuotasCobro>();

    public virtual Plane IdPlanNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;
}
