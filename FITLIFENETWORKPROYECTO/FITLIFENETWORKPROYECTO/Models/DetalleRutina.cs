using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class DetalleRutina
{
    public int IdDetalleRutina { get; set; }

    public int IdRutina { get; set; }

    public int IdEjercicio { get; set; }

    public int Series { get; set; }

    public int Repeticiones { get; set; }

    public int DescansoSegundos { get; set; }

    public virtual Ejercicio IdEjercicioNavigation { get; set; } = null!;

    public virtual Rutina IdRutinaNavigation { get; set; } = null!;
}
