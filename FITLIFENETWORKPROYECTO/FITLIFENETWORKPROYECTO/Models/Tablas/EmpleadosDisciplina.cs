using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class EmpleadosDisciplina
{
    public int IdEmpleadoDisciplina { get; set; }

    public int IdEmpleado { get; set; }

    public int IdDisciplina { get; set; }

    public virtual Disciplina IdDisciplinaNavigation { get; set; } = null!;

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
}
