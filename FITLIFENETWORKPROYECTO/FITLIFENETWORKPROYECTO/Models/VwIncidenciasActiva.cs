using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class VwIncidenciasActiva
{
    public int IdReclamo { get; set; }

    public int? IdEquipo { get; set; }

    public string? NombreMaquina { get; set; }

    public int IdSede { get; set; }

    public string NombreSede { get; set; } = null!;

    public int? IdTecnicoAsignado { get; set; }

    public int IdEstadoReclamo { get; set; }

    public string NombreEstado { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public string Descripcion { get; set; } = null!;
}
