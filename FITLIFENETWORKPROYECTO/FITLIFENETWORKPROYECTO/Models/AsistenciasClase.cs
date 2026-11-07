using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class AsistenciasClase
{
    public int IdAsistencia { get; set; }

    public int IdReserva { get; set; }

    public DateTime FechaHoraAsistencia { get; set; }

    public bool Presente { get; set; }

    public virtual Reserva IdReservaNavigation { get; set; } = null!;
}
