using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class PremiosCatalogo
{
    public int IdPremio { get; set; }

    public string NombrePremio { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int MesesAntiguedadRequeridos { get; set; }

    public virtual ICollection<PremiosOtorgado> PremiosOtorgados { get; set; } = new List<PremiosOtorgado>();
}
