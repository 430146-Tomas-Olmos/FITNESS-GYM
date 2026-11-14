using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public bool Activo { get; set; } = true;

    public virtual ICollection<RolesSistema> Idrols { get; set; } = new List<RolesSistema>();
}
