using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Tablas;

public partial class LiquidacionesSueldo
{
    public int IdLiquidacion { get; set; }

    public int IdEmpleado { get; set; }

    public int PeriodoMes { get; set; }

    public int PeriodoAnio { get; set; }

    public DateOnly FechaPago { get; set; }

    public decimal MontoNeto { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
}
