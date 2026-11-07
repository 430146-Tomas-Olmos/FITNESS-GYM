using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class PlanesDisciplina
{
    public int IdPlanDisciplina { get; set; }

    public int IdPlan { get; set; }

    public int IdDisciplina { get; set; }

    public virtual Disciplina IdDisciplinaNavigation { get; set; } = null!;

    public virtual Plane IdPlanNavigation { get; set; } = null!;
}
