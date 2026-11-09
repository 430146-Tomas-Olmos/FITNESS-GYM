using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class HorariosSede
{
    public int IdHorarioSede { get; set; }

    public int IdSede { get; set; }

    public string DiaSemana { get; set; } = null!;

    public TimeOnly HoraApertura { get; set; }

    public TimeOnly HoraCierre { get; set; }

    public virtual Sede IdSedeNavigation { get; set; } = null!;
}
