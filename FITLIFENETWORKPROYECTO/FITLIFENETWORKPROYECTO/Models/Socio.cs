using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models;

public partial class Socio
{
    public int IdSocio { get; set; }

    public int IdPersona { get; set; }

    public int IdCalle { get; set; }

    public int? IdConvenio { get; set; }

    public DateOnly FechaIngresoAlta { get; set; }

    public string EstadoCuenta { get; set; } = null!;

    public virtual ICollection<AccesosLog> AccesosLogs { get; set; } = new List<AccesosLog>();

    public virtual ICollection<AptosMedico> AptosMedicos { get; set; } = new List<AptosMedico>();

    public virtual ICollection<ContratosMembresium> ContratosMembresia { get; set; } = new List<ContratosMembresium>();

    public virtual ICollection<EvaluacionesFisica> EvaluacionesFisicas { get; set; } = new List<EvaluacionesFisica>();

    public virtual Calle IdCalleNavigation { get; set; } = null!;

    public virtual ConveniosEmpresa? IdConvenioNavigation { get; set; }

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual ICollection<IncidenciasReclamo> IncidenciasReclamos { get; set; } = new List<IncidenciasReclamo>();

    public virtual ICollection<LogAccesosDenegado> LogAccesosDenegados { get; set; } = new List<LogAccesosDenegado>();

    public virtual ICollection<LogCambiosEstadoSocio> LogCambiosEstadoSocios { get; set; } = new List<LogCambiosEstadoSocio>();

    public virtual ICollection<PremiosOtorgado> PremiosOtorgados { get; set; } = new List<PremiosOtorgado>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<Rutina> Rutinas { get; set; } = new List<Rutina>();

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
