using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Rutina
{
    public int IdRutina { get; set; }

    public int IdSocio { get; set; }

    public int IdEntrenador { get; set; }

    public DateOnly FechaCreacion { get; set; }

    public string Objetivo { get; set; } = null!;

    public virtual ICollection<DetalleRutina> DetalleRutinas { get; set; } = new List<DetalleRutina>();

    public virtual Empleado IdEntrenadorNavigation { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;
}
