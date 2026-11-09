using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Turno
{
    public int IdTurno { get; set; }

    public int IdSocio { get; set; }

    public int IdProfesional { get; set; }

    public int IdSede { get; set; }

    public int IdEstadoTurno { get; set; }

    public DateOnly FechaTurno { get; set; }

    public TimeOnly HoraTurno { get; set; }

    public virtual ICollection<EvaluacionesFisica> EvaluacionesFisicas { get; set; } = new List<EvaluacionesFisica>();

    public virtual EstadosTurno IdEstadoTurnoNavigation { get; set; } = null!;

    public virtual Empleado IdProfesionalNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;
}
