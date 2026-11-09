using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Salone
{
    public int IdSalon { get; set; }

    public int IdSede { get; set; }

    public string NombreSalon { get; set; } = null!;

    public int CapacidadMaxima { get; set; }

    public virtual ICollection<Clase> Clases { get; set; } = new List<Clase>();

    public virtual Sede IdSedeNavigation { get; set; } = null!;
}
