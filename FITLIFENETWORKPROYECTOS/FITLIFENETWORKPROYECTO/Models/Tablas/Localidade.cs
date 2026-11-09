using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Localidade
{
    public int IdLocalidad { get; set; }

    public int IdProvincia { get; set; }

    public string NombreLocalidad { get; set; } = null!;

    public virtual ICollection<Barrio> Barrios { get; set; } = new List<Barrio>();

    public virtual Provincia IdProvinciaNavigation { get; set; } = null!;
}
