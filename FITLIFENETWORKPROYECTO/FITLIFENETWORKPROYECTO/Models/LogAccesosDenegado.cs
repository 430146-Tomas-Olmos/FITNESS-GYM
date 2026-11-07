using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class LogAccesosDenegado
{
    public int IdLog { get; set; }

    public int IdSocio { get; set; }

    public int IdSede { get; set; }

    public DateTime FechaHora { get; set; }

    public string MotivoBloqueo { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;
}
