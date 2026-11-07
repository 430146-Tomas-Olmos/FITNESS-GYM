using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class EstadosReclamo
{
    public int IdEstadoReclamo { get; set; }

    public string NombreEstado { get; set; } = null!;

    public virtual ICollection<IncidenciasReclamo> IncidenciasReclamos { get; set; } = new List<IncidenciasReclamo>();
}
