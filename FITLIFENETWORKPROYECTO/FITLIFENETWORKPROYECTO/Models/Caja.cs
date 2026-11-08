using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class Caja
{
    public int IdCaja { get; set; }

    public int IdSede { get; set; }

    public int IdEmpleado { get; set; }

    public DateTime FechaApertura { get; set; }

    public decimal SaldoInicial { get; set; }

    public DateTime? FechaCierre { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<CierresCaja> CierresCajas { get; set; } = new List<CierresCaja>();

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual Sede IdSedeNavigation { get; set; } = null!;
}
