using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class VwOcupacionClasesGrupale
{
    public int IdClase { get; set; }

    public DateOnly FechaClase { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public int IdSede { get; set; }

    public string NombreSede { get; set; } = null!;

    public string NombreDisciplina { get; set; } = null!;

    public int CupoLimite { get; set; }

    public int? CuposOcupados { get; set; }

    public int? CuposDisponibles { get; set; }
}
