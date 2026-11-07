using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class Clase
{
    public int IdClase { get; set; }

    public int IdSalon { get; set; }

    public int IdDisciplina { get; set; }

    public int IdInstructor { get; set; }

    public DateOnly FechaClase { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public int CupoLimite { get; set; }

    public virtual Disciplina IdDisciplinaNavigation { get; set; } = null!;

    public virtual Empleado IdInstructorNavigation { get; set; } = null!;

    public virtual Salone IdSalonNavigation { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
