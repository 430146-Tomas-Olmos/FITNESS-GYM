using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class AccesosLog
{
    public int IdAcceso { get; set; }

    public int IdSocio { get; set; }

    public int IdSede { get; set; }

    public DateTime FechaHoraMarcado { get; set; }

    public bool AccesoPermitido { get; set; }

    public string? MotivoRechazo { get; set; }

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;
}
