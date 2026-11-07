using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class ConveniosEmpresa
{
    public int IdConvenio { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string Cuit { get; set; } = null!;

    public decimal PorcentajeDescuento { get; set; }

    public int DiaCierreFacturacion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Socio> Socios { get; set; } = new List<Socio>();
}
