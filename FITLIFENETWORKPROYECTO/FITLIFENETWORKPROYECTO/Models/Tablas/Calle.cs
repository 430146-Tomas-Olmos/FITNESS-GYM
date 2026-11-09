using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Calle
{
    public int IdCalle { get; set; }

    public int IdBarrio { get; set; }

    public string NombreCalle { get; set; } = null!;

    public int Numeracion { get; set; }

    public virtual Barrio IdBarrioNavigation { get; set; } = null!;

    public virtual ICollection<Sede> Sedes { get; set; } = new List<Sede>();

    public virtual ICollection<Socio> Socios { get; set; } = new List<Socio>();
}
