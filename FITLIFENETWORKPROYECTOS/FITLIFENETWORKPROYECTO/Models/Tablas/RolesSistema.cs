using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class RolesSistema
{
    public int Id { get; set; }

    public string Rol { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Usuario> Idusuarios { get; set; } = new List<Usuario>();
}
