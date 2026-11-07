using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class IncidenciasReclamo
{
    public int IdReclamo { get; set; }

    public int IdSocio { get; set; }

    public int? IdEquipo { get; set; }

    public int IdSede { get; set; }

    public int? IdTecnicoAsignado { get; set; }

    public int IdEstadoReclamo { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual Equipamiento? IdEquipoNavigation { get; set; }

    public virtual EstadosReclamo IdEstadoReclamoNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;

    public virtual Empleado? IdTecnicoAsignadoNavigation { get; set; }
}
