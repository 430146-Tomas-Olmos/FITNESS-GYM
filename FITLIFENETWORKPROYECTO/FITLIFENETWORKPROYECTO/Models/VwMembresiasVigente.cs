using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class VwMembresiasVigente
{
    public int IdContrato { get; set; }

    public int IdSocio { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int IdPlan { get; set; }

    public string NombrePlan { get; set; } = null!;

    public int IdSede { get; set; }

    public string NombreSede { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public string EstadoContrato { get; set; } = null!;
}
