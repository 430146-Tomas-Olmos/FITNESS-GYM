using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Equipamiento
{
    public int IdEquipo { get; set; }

    public int IdSede { get; set; }

    public string NombreMaquina { get; set; } = null!;

    public string NumeroSerie { get; set; } = null!;

    public DateOnly FechaAdquisicion { get; set; }

    public string EstadoOperativo { get; set; } = null!;

    public virtual ICollection<AuditoriaEquipamiento> AuditoriaEquipamientos { get; set; } = new List<AuditoriaEquipamiento>();

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual ICollection<IncidenciasReclamo> IncidenciasReclamos { get; set; } = new List<IncidenciasReclamo>();

    public virtual ICollection<MantenimientoEquipamiento> MantenimientoEquipamientos { get; set; } = new List<MantenimientoEquipamiento>();
}
