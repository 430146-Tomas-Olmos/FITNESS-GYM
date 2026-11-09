using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Ejercicio
{
    public int IdEjercicio { get; set; }

    public string NombreEjercicio { get; set; } = null!;

    public string GrupoMuscular { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<DetalleRutina> DetalleRutinas { get; set; } = new List<DetalleRutina>();
}
