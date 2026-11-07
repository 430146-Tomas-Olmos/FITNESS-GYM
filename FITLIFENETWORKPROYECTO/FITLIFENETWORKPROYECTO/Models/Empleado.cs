using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public int IdPersona { get; set; }

    public int IdSede { get; set; }

    public int IdRol { get; set; }

    public string Legajo { get; set; } = null!;

    public DateOnly FechaIngreso { get; set; }

    public decimal SueldoBase { get; set; }

    public virtual ICollection<Clase> Clases { get; set; } = new List<Clase>();

    public virtual ICollection<EmpleadosDisciplina> EmpleadosDisciplinas { get; set; } = new List<EmpleadosDisciplina>();

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual ICollection<IncidenciasReclamo> IncidenciasReclamos { get; set; } = new List<IncidenciasReclamo>();

    public virtual ICollection<LiquidacionesSueldo> LiquidacionesSueldos { get; set; } = new List<LiquidacionesSueldo>();

    public virtual ICollection<Rutina> Rutinas { get; set; } = new List<Rutina>();

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
