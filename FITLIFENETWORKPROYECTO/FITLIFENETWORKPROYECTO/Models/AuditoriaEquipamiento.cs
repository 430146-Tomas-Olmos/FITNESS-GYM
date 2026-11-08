using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class AuditoriaEquipamiento
{
    public int IdAuditoria { get; set; }

    public int IdEquipo { get; set; }

    public string EstadoAnterior { get; set; } = null!;

    public string EstadoNuevo { get; set; } = null!;

    public DateTime FechaCambio { get; set; }

    public string? UsuarioSistema { get; set; }

    public virtual Equipamiento IdEquipoNavigation { get; set; } = null!;
}
