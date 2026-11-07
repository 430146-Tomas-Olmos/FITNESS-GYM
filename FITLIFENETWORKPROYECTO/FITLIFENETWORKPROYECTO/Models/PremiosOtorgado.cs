using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class PremiosOtorgado
{
    public int IdOtorgamiento { get; set; }

    public int IdSocio { get; set; }

    public int IdPremio { get; set; }

    public DateTime FechaOtorgamiento { get; set; }

    public string EstadoCanje { get; set; } = null!;

    public virtual PremiosCatalogo IdPremioNavigation { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;
}
