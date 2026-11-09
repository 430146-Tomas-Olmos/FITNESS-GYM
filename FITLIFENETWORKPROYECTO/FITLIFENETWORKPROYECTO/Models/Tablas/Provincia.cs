using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Provincia
{
    public int IdProvincia { get; set; }

    public string NombreProvincia { get; set; } = null!;

    public virtual ICollection<Localidade> Localidades { get; set; } = new List<Localidade>();
}
