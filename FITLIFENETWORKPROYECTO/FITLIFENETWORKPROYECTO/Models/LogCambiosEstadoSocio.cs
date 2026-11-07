using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class LogCambiosEstadoSocio
{
    public int IdLogEstado { get; set; }

    public int IdSocio { get; set; }

    public string EstadoAnterior { get; set; } = null!;

    public string EstadoNuevo { get; set; } = null!;

    public DateTime FechaCambio { get; set; }

    public string UsuarioSistema { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;
}
