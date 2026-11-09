using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Disciplina
{
    public int IdDisciplina { get; set; }

    public string NombreDisciplina { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Clase> Clases { get; set; } = new List<Clase>();

    public virtual ICollection<EmpleadosDisciplina> EmpleadosDisciplinas { get; set; } = new List<EmpleadosDisciplina>();

    public virtual ICollection<PlanesDisciplina> PlanesDisciplinas { get; set; } = new List<PlanesDisciplina>();
}
