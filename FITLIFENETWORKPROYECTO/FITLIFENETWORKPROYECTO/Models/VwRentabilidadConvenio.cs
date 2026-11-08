using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class VwRentabilidadConvenio
{
    public int IdConvenio { get; set; }

    public string RazonSocial { get; set; } = null!;

    public decimal PorcentajeDescuento { get; set; }

    public int? SociosAsociados { get; set; }

    public decimal ImporteFacturado { get; set; }

    public decimal ImporteCobrado { get; set; }

    public decimal? Deuda { get; set; }

    public decimal? PorcentajeCobrabilidad { get; set; }
}
