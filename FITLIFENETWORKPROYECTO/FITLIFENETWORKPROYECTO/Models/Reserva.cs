using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public int IdSocio { get; set; }

    public int IdClase { get; set; }

    public DateTime FechaReserva { get; set; }

    public string EstadoReserva { get; set; } = null!;

    public virtual ICollection<AsistenciasClase> AsistenciasClases { get; set; } = new List<AsistenciasClase>();

    public virtual Clase IdClaseNavigation { get; set; } = null!;

    public virtual Socio IdSocioNavigation { get; set; } = null!;
}
