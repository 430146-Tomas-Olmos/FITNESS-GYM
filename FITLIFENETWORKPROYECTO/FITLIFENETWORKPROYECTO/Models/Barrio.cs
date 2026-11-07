using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class Barrio
{
    public int IdBarrio { get; set; }

    public int IdLocalidad { get; set; }

    public string NombreBarrio { get; set; } = null!;

    public virtual ICollection<Calle> Calles { get; set; } = new List<Calle>();

    public virtual Localidade IdLocalidadNavigation { get; set; } = null!;
}
