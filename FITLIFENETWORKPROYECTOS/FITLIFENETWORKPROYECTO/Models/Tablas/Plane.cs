using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Plane
{
    public int IdPlan { get; set; }

    public string NombrePlan { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal PrecioActual { get; set; }

    public int DuracionMeses { get; set; }

    public bool PermiteMultisede { get; set; }

    public virtual ICollection<ContratosMembresium> ContratosMembresia { get; set; } = new List<ContratosMembresium>();

    public virtual ICollection<HistorialPreciosPlane> HistorialPreciosPlanes { get; set; } = new List<HistorialPreciosPlane>();

    public virtual ICollection<PlanesDisciplina> PlanesDisciplinas { get; set; } = new List<PlanesDisciplina>();
}
