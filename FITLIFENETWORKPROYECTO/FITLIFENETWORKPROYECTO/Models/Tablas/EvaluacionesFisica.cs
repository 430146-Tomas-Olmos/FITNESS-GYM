using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class EvaluacionesFisica
{
    public int IdEvaluacion { get; set; }

    public int IdSocio { get; set; }

    public int IdTurno { get; set; }

    public decimal PesoKg { get; set; }

    public decimal PorcentajeGrasa { get; set; }

    public decimal PorcentajeMasaMuscular { get; set; }

    public string ObjetivosAsignados { get; set; } = null!;

    public string PlanAlimentario { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;

    public virtual Turno IdTurnoNavigation { get; set; } = null!;
}
