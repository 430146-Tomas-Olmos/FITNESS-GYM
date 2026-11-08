using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class Sede
{
    public int IdSede { get; set; }

    public int IdCalle { get; set; }

    public string NombreSede { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<AccesosLog> AccesosLogs { get; set; } = new List<AccesosLog>();

    public virtual ICollection<Caja> Cajas { get; set; } = new List<Caja>();

    public virtual ICollection<ComprasStock> ComprasStocks { get; set; } = new List<ComprasStock>();

    public virtual ICollection<ContratosMembresium> ContratosMembresia { get; set; } = new List<ContratosMembresium>();

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

    public virtual ICollection<Equipamiento> Equipamientos { get; set; } = new List<Equipamiento>();

    public virtual ICollection<HorariosSede> HorariosSedes { get; set; } = new List<HorariosSede>();

    public virtual Calle IdCalleNavigation { get; set; } = null!;

    public virtual ICollection<IncidenciasReclamo> IncidenciasReclamos { get; set; } = new List<IncidenciasReclamo>();

    public virtual ICollection<LogAccesosDenegado> LogAccesosDenegados { get; set; } = new List<LogAccesosDenegado>();

    public virtual ICollection<MovimientosStock> MovimientosStocks { get; set; } = new List<MovimientosStock>();

    public virtual ICollection<Salone> Salones { get; set; } = new List<Salone>();

    public virtual ICollection<StockSede> StockSedes { get; set; } = new List<StockSede>();

    public virtual ICollection<TransferenciasStock> TransferenciasStockIdSedeDestinoNavigations { get; set; } = new List<TransferenciasStock>();

    public virtual ICollection<TransferenciasStock> TransferenciasStockIdSedeOrigenNavigations { get; set; } = new List<TransferenciasStock>();

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
