using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Models.Tablas;
using FITLIFENETWORKPROYECTO.Models.Views_Dashboard;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FITLIFENETWORKPROYECTO.Models.Context;

public partial class FITLIFENETWORKDbContext : DbContext
{
    public FITLIFENETWORKDbContext()
    {
    }

    public FITLIFENETWORKDbContext(DbContextOptions<FITLIFENETWORKDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccesosLog> AccesosLogs { get; set; }

    public virtual DbSet<AptosMedico> AptosMedicos { get; set; }

    public virtual DbSet<AsistenciasClase> AsistenciasClases { get; set; }

    public virtual DbSet<AuditoriaEquipamiento> AuditoriaEquipamientos { get; set; }

    public virtual DbSet<Barrio> Barrios { get; set; }

    public virtual DbSet<Caja> Cajas { get; set; }

    public virtual DbSet<Calle> Calles { get; set; }

    public virtual DbSet<CategoriasProducto> CategoriasProductos { get; set; }

    public virtual DbSet<CierresCaja> CierresCajas { get; set; }

    public virtual DbSet<Clase> Clases { get; set; }

    public virtual DbSet<ComprasStock> ComprasStocks { get; set; }

    public virtual DbSet<ContratosMembresium> ContratosMembresia { get; set; }

    public virtual DbSet<ConveniosEmpresa> ConveniosEmpresas { get; set; }

    public virtual DbSet<CuotasCobro> CuotasCobros { get; set; }

    public virtual DbSet<DetalleComprasStock> DetalleComprasStocks { get; set; }

    public virtual DbSet<DetalleDevolucionesVentum> DetalleDevolucionesVenta { get; set; }

    public virtual DbSet<DetalleRutina> DetalleRutinas { get; set; }

    public virtual DbSet<DetalleTransferencia> DetalleTransferencias { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    public virtual DbSet<DevolucionesVentum> DevolucionesVenta { get; set; }

    public virtual DbSet<Disciplina> Disciplinas { get; set; }

    public virtual DbSet<Ejercicio> Ejercicios { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<EmpleadosDisciplina> EmpleadosDisciplinas { get; set; }

    public virtual DbSet<Equipamiento> Equipamientos { get; set; }

    public virtual DbSet<EstadosReclamo> EstadosReclamos { get; set; }

    public virtual DbSet<EstadosTurno> EstadosTurnos { get; set; }

    public virtual DbSet<EvaluacionesFisica> EvaluacionesFisicas { get; set; }

    public virtual DbSet<FormasPago> FormasPagos { get; set; }

    public virtual DbSet<HistorialPreciosPlane> HistorialPreciosPlanes { get; set; }

    public virtual DbSet<HistorialPreciosProducto> HistorialPreciosProductos { get; set; }

    public virtual DbSet<HorariosSede> HorariosSedes { get; set; }

    public virtual DbSet<IncidenciasReclamo> IncidenciasReclamos { get; set; }

    public virtual DbSet<LiquidacionesSueldo> LiquidacionesSueldos { get; set; }

    public virtual DbSet<Localidade> Localidades { get; set; }

    public virtual DbSet<LogAccesosDenegado> LogAccesosDenegados { get; set; }

    public virtual DbSet<LogCambiosEstadoSocio> LogCambiosEstadoSocios { get; set; }

    public virtual DbSet<MantenimientoEquipamiento> MantenimientoEquipamientos { get; set; }

    public virtual DbSet<MovimientosStock> MovimientosStocks { get; set; }

    public virtual DbSet<PagosCuota> PagosCuotas { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Plane> Planes { get; set; }

    public virtual DbSet<PlanesDisciplina> PlanesDisciplinas { get; set; }

    public virtual DbSet<PremiosCatalogo> PremiosCatalogos { get; set; }

    public virtual DbSet<PremiosOtorgado> PremiosOtorgados { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolesSistema> RolesSistemas { get; set; }

    public virtual DbSet<Rutina> Rutinas { get; set; }

    public virtual DbSet<Salone> Salones { get; set; }

    public virtual DbSet<Sede> Sedes { get; set; }

    public virtual DbSet<Socio> Socios { get; set; }

    public virtual DbSet<StockSede> StockSedes { get; set; }

    public virtual DbSet<TransferenciasStock> TransferenciasStocks { get; set; }

    public virtual DbSet<Turno> Turnos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<VwControlAptosMedico> VwControlAptosMedicos { get; set; }

    public virtual DbSet<VwCostosMantenimientoEquipamiento> VwCostosMantenimientoEquipamientos { get; set; }

    public virtual DbSet<VwCuotasPendiente> VwCuotasPendientes { get; set; }

    public virtual DbSet<VwEstadoCuentaSocio> VwEstadoCuentaSocios { get; set; }

    public virtual DbSet<VwIncidenciasActiva> VwIncidenciasActivas { get; set; }

    public virtual DbSet<VwMembresiasVigente> VwMembresiasVigentes { get; set; }

    public virtual DbSet<VwOcupacionClasesGrupale> VwOcupacionClasesGrupales { get; set; }

    public virtual DbSet<VwRentabilidadConvenio> VwRentabilidadConvenios { get; set; }

    public virtual DbSet<VwStockDisponiblePorSede> VwStockDisponiblePorSedes { get; set; }

    public virtual DbSet<VwVentasDetalladasProducto> VwVentasDetalladasProductos { get; set; }

    public virtual DbSet<ReporteRecaudacionSede> ReporteRecaudacionSedes { get; set; }

    public virtual DbSet<AuditoriaAptoMedico> AuditoriaAptoMedicos { get; set; }

    public virtual DbSet<PremiosCanjeados> PremioCanjeadoDtos { get; set; }

    public virtual DbSet<Top3Productos> Top3Productos { get; set; }

    public virtual DbSet<ReporteReservaTT> ReporteReservaTT { get; set; }

    public virtual DbSet<EvaluacionEstadistica> EvaluacionEstadisticas { get; set; }

    public virtual DbSet<RentabilidadConvenio> RentabilidadConvenios { get; set; }

    public virtual DbSet<ControlAptoMedico> ControlAptoMedicos { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // segun gpt asi se soluciona el error de conexion a la base de datos, pero no se si es correcto
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditoriaAptoMedico>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<ReporteRecaudacionSede>(entity =>
        {
            entity.HasNoKey(); 
        });

        //lo de arriba hace que Efcore mapee solo la consulta si las columnas coinciden (eso me recomendo que haga gemini)

        modelBuilder.Entity<PremiosCanjeados>(entity => 
        { 
            entity.HasNoKey(); 
        });

        modelBuilder.Entity<Top3Productos>(entity => 
        { 
            entity.HasNoKey();
        });

        modelBuilder.Entity<ReporteReservaTT>(entity => 
        { 
            entity.HasNoKey();
        });

        modelBuilder.Entity<EvaluacionEstadistica>(entity => 
        { 
            entity.HasNoKey(); 
        });

        modelBuilder.Entity<ControlAptoMedico>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<RentabilidadConvenio>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<AccesosLog>(entity =>
        {
            entity.HasKey(e => e.IdAcceso);

            entity.ToTable("ACCESOS_LOG");

            entity.Property(e => e.IdAcceso).HasColumnName("id_acceso");
            entity.Property(e => e.AccesoPermitido).HasColumnName("acceso_permitido");
            entity.Property(e => e.FechaHoraMarcado)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora_marcado");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.MotivoRechazo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("motivo_rechazo");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.AccesosLogs)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ACCESOS_LOG_x_SEDES");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.AccesosLogs)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ACCESOS_LOG_x_SOCIOS");
        });

        modelBuilder.Entity<AptosMedico>(entity =>
        {
            entity.HasKey(e => e.IdApto);

            entity.ToTable("APTOS_MEDICOS");

            entity.Property(e => e.IdApto).HasColumnName("id_apto");
            entity.Property(e => e.FechaEmision).HasColumnName("fecha_emision");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.MatriculaMedico)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("matricula_medico");
            entity.Property(e => e.MedicoFirmante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("medico_firmante");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("observaciones");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.AptosMedicos)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("APTOS_MEDICOS_x_SOCIOS");
        });

        modelBuilder.Entity<AsistenciasClase>(entity =>
        {
            entity.HasKey(e => e.IdAsistencia);

            entity.ToTable("ASISTENCIAS_CLASES");

            entity.Property(e => e.IdAsistencia).HasColumnName("id_asistencia");
            entity.Property(e => e.FechaHoraAsistencia)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora_asistencia");
            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.Presente).HasColumnName("presente");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.AsistenciasClases)
                .HasForeignKey(d => d.IdReserva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ASISTENCIAS_CLASES_x_RESERVAS");
        });

        modelBuilder.Entity<AuditoriaEquipamiento>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria);

            entity.ToTable("AUDITORIA_EQUIPAMIENTO");

            entity.Property(e => e.IdAuditoria).HasColumnName("id_auditoria");
            entity.Property(e => e.EstadoAnterior)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado_anterior");
            entity.Property(e => e.EstadoNuevo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado_nuevo");
            entity.Property(e => e.FechaCambio)
                .HasDefaultValueSql("(getdate())", "DF_AUDITORIA_EQUIPO_FECHA")
                .HasColumnType("datetime")
                .HasColumnName("fecha_cambio");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.UsuarioSistema)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario_sistema");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.AuditoriaEquipamientos)
                .HasForeignKey(d => d.IdEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AUDITORIA_EQUIPAMIENTO_EQUIPO");
        });

        modelBuilder.Entity<Barrio>(entity =>
        {
            entity.HasKey(e => e.IdBarrio);

            entity.ToTable("BARRIOS");

            entity.Property(e => e.IdBarrio).HasColumnName("id_barrio");
            entity.Property(e => e.IdLocalidad).HasColumnName("id_localidad");
            entity.Property(e => e.NombreBarrio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_barrio");

            entity.HasOne(d => d.IdLocalidadNavigation).WithMany(p => p.Barrios)
                .HasForeignKey(d => d.IdLocalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BARRIOS_x_LOCALIDADES");
        });

        modelBuilder.Entity<Caja>(entity =>
        {
            entity.HasKey(e => e.IdCaja);

            entity.ToTable("CAJAS");

            entity.Property(e => e.IdCaja).HasColumnName("id_caja");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("ABIERTA", "DF_CAJAS_ESTADO")
                .HasColumnName("estado");
            entity.Property(e => e.FechaApertura)
                .HasDefaultValueSql("(getdate())", "DF_CAJAS_APERTURA")
                .HasColumnType("datetime")
                .HasColumnName("fecha_apertura");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("datetime")
                .HasColumnName("fecha_cierre");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.SaldoInicial)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("saldo_inicial");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Cajas)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CAJAS_EMPLEADOS");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Cajas)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CAJAS_SEDES");
        });

        modelBuilder.Entity<Calle>(entity =>
        {
            entity.HasKey(e => e.IdCalle);

            entity.ToTable("CALLES");

            entity.Property(e => e.IdCalle).HasColumnName("id_calle");
            entity.Property(e => e.IdBarrio).HasColumnName("id_barrio");
            entity.Property(e => e.NombreCalle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_calle");
            entity.Property(e => e.Numeracion).HasColumnName("numeracion");

            entity.HasOne(d => d.IdBarrioNavigation).WithMany(p => p.Calles)
                .HasForeignKey(d => d.IdBarrio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CALLES_x_BARRIOS");
        });

        modelBuilder.Entity<CategoriasProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.ToTable("CATEGORIAS_PRODUCTOS");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_categoria");
        });

        modelBuilder.Entity<CierresCaja>(entity =>
        {
            entity.HasKey(e => e.IdCierre);

            entity.ToTable("CIERRES_CAJA");

            entity.Property(e => e.IdCierre).HasColumnName("id_cierre");
            entity.Property(e => e.Diferencia)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("diferencia");
            entity.Property(e => e.FechaCierre)
                .HasDefaultValueSql("(getdate())", "DF_CIERRES_CAJA_FECHA")
                .HasColumnType("datetime")
                .HasColumnName("fecha_cierre");
            entity.Property(e => e.IdCaja).HasColumnName("id_caja");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("observaciones");
            entity.Property(e => e.SaldoEsperado)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("saldo_esperado");
            entity.Property(e => e.SaldoReal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("saldo_real");
            entity.Property(e => e.TotalVentas)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_ventas");

            entity.HasOne(d => d.IdCajaNavigation).WithMany(p => p.CierresCajas)
                .HasForeignKey(d => d.IdCaja)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CIERRES_CAJA_CAJAS");
        });

        modelBuilder.Entity<Clase>(entity =>
        {
            entity.HasKey(e => e.IdClase);

            entity.ToTable("CLASES");

            entity.Property(e => e.IdClase).HasColumnName("id_clase");
            entity.Property(e => e.CupoLimite).HasColumnName("cupo_limite");
            entity.Property(e => e.FechaClase).HasColumnName("fecha_clase");
            entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdDisciplina).HasColumnName("id_disciplina");
            entity.Property(e => e.IdInstructor).HasColumnName("id_instructor");
            entity.Property(e => e.IdSalon).HasColumnName("id_salon");

            entity.HasOne(d => d.IdDisciplinaNavigation).WithMany(p => p.Clases)
                .HasForeignKey(d => d.IdDisciplina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CLASES_x_DISCIPLINAS");

            entity.HasOne(d => d.IdInstructorNavigation).WithMany(p => p.Clases)
                .HasForeignKey(d => d.IdInstructor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CLASES_x_EMPLEADOS");

            entity.HasOne(d => d.IdSalonNavigation).WithMany(p => p.Clases)
                .HasForeignKey(d => d.IdSalon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CLASES_x_SALONES");
        });

        modelBuilder.Entity<ComprasStock>(entity =>
        {
            entity.HasKey(e => e.IdCompra);

            entity.ToTable("COMPRAS_STOCK");

            entity.Property(e => e.IdCompra).HasColumnName("id_compra");
            entity.Property(e => e.FechaCompra)
                .HasColumnType("datetime")
                .HasColumnName("fecha_compra");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.ComprasStocks)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("COMPRAS_STOCK_x_PROVEEDORES");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.ComprasStocks)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("COMPRAS_STOCK_x_SEDES");
        });

        modelBuilder.Entity<ContratosMembresium>(entity =>
        {
            entity.HasKey(e => e.IdContrato);

            entity.ToTable("CONTRATOS_MEMBRESIA");

            entity.Property(e => e.IdContrato).HasColumnName("id_contrato");
            entity.Property(e => e.EstadoContrato)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_contrato");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.FilaVersion)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("fila_version");
            entity.Property(e => e.IdPlan).HasColumnName("id_plan");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.ContratosMembresia)
                .HasForeignKey(d => d.IdPlan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CONTRATOS_MEMBRESIA_x_PLANES");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.ContratosMembresia)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CONTRATOS_MEMBRESIA_x_SEDES");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.ContratosMembresia)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CONTRATOS_MEMBRESIA_x_SOCIOS");
        });

        modelBuilder.Entity<ConveniosEmpresa>(entity =>
        {
            entity.HasKey(e => e.IdConvenio);

            entity.ToTable("CONVENIOS_EMPRESA");

            entity.Property(e => e.IdConvenio).HasColumnName("id_convenio");
            entity.Property(e => e.Cuit)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("cuit");
            entity.Property(e => e.DiaCierreFacturacion).HasColumnName("dia_cierre_facturacion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.PorcentajeDescuento)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porcentaje_descuento");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("razon_social");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<CuotasCobro>(entity =>
        {
            entity.HasKey(e => e.IdCobro);

            entity.ToTable("CUOTAS_COBROS");

            entity.HasIndex(e => new { e.IdContrato, e.FechaVencimiento }, "IX_CUOTAS_CONTRATO_VENCIMIENTO");

            entity.Property(e => e.IdCobro).HasColumnName("id_cobro");
            entity.Property(e => e.FechaPago)
                .HasColumnType("datetime")
                .HasColumnName("fecha_pago");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.IdContrato).HasColumnName("id_contrato");
            entity.Property(e => e.IdFormaPago).HasColumnName("id_forma_pago");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.NumeroCuota).HasColumnName("numero_cuota");

            entity.HasOne(d => d.IdContratoNavigation).WithMany(p => p.CuotasCobros)
                .HasForeignKey(d => d.IdContrato)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CUOTAS_COBROS_x_CONTRATOS_MEMBRESIA");

            entity.HasOne(d => d.IdFormaPagoNavigation).WithMany(p => p.CuotasCobros)
                .HasForeignKey(d => d.IdFormaPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CUOTAS_COBROS_x_FORMAS_PAGO");
        });

        modelBuilder.Entity<DetalleComprasStock>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCompra);

            entity.ToTable("DETALLE_COMPRAS_STOCK");

            entity.Property(e => e.IdDetalleCompra).HasColumnName("id_detalle_compra");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdCompra).HasColumnName("id_compra");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.PrecioCostoUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_costo_unitario");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetalleComprasStocks)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DETALLE_COMPRAS_STOCK_x_COMPRAS_STOCK");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleComprasStocks)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DETALLE_COMPRAS_STOCK_x_PRODUCTOS");
        });

        modelBuilder.Entity<DetalleDevolucionesVentum>(entity =>
        {
            entity.HasKey(e => e.IdDetalleDevolucion);

            entity.ToTable("DETALLE_DEVOLUCIONES_VENTA");

            entity.Property(e => e.IdDetalleDevolucion).HasColumnName("id_detalle_devolucion");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdDevolucion).HasColumnName("id_devolucion");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdDevolucionNavigation).WithMany(p => p.DetalleDevolucionesVenta)
                .HasForeignKey(d => d.IdDevolucion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DETALLE_DEVOLUCIONES_DEVOLUCIONES");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleDevolucionesVenta)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DETALLE_DEVOLUCIONES_PRODUCTOS");
        });

        modelBuilder.Entity<DetalleRutina>(entity =>
        {
            entity.HasKey(e => e.IdDetalleRutina);

            entity.ToTable("DETALLE_RUTINAS");

            entity.Property(e => e.IdDetalleRutina).HasColumnName("id_detalle_rutina");
            entity.Property(e => e.DescansoSegundos).HasColumnName("descanso_segundos");
            entity.Property(e => e.IdEjercicio).HasColumnName("id_ejercicio");
            entity.Property(e => e.IdRutina).HasColumnName("id_rutina");
            entity.Property(e => e.Repeticiones).HasColumnName("repeticiones");
            entity.Property(e => e.Series).HasColumnName("series");

            entity.HasOne(d => d.IdEjercicioNavigation).WithMany(p => p.DetalleRutinas)
                .HasForeignKey(d => d.IdEjercicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DETALLE_RUTINAS_x_EJERCICIOS");

            entity.HasOne(d => d.IdRutinaNavigation).WithMany(p => p.DetalleRutinas)
                .HasForeignKey(d => d.IdRutina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DETALLE_RUTINAS_x_RUTINAS");
        });

        modelBuilder.Entity<DetalleTransferencia>(entity =>
        {
            entity.HasKey(e => e.IdDetalleTransferencia);

            entity.ToTable("DETALLE_TRANSFERENCIAS");

            entity.Property(e => e.IdDetalleTransferencia).HasColumnName("id_detalle_transferencia");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdTransferencia).HasColumnName("id_transferencia");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleTransferencia)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DETALLE_TRANSFERENCIAS_PRODUCTOS");

            entity.HasOne(d => d.IdTransferenciaNavigation).WithMany(p => p.DetalleTransferencia)
                .HasForeignKey(d => d.IdTransferencia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DETALLE_TRANSFERENCIAS_TRANSFERENCIAS");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.IdDetalleVenta);

            entity.ToTable("DETALLE_VENTAS", tb => tb.HasTrigger("TR_ActualizarStockVenta"));

            entity.Property(e => e.IdDetalleVenta).HasColumnName("id_detalle_venta");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.PrecioUnitarioHistorico)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario_historico");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DETALLE_VENTAS_x_PRODUCTOS");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DETALLE_VENTAS_x_VENTAS");
        });

        modelBuilder.Entity<DevolucionesVentum>(entity =>
        {
            entity.HasKey(e => e.IdDevolucion);

            entity.ToTable("DEVOLUCIONES_VENTA");

            entity.Property(e => e.IdDevolucion).HasColumnName("id_devolucion");
            entity.Property(e => e.FechaDevolucion)
                .HasDefaultValueSql("(getdate())", "DF_DEVOLUCIONES_VENTA_FECHA")
                .HasColumnType("datetime")
                .HasColumnName("fecha_devolucion");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.ImporteReintegrado)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("importe_reintegrado");
            entity.Property(e => e.Motivo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("motivo");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.DevolucionesVenta)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DEVOLUCIONES_VENTA_EMPLEADOS");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DevolucionesVenta)
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DEVOLUCIONES_VENTA_VENTAS");
        });

        modelBuilder.Entity<Disciplina>(entity =>
        {
            entity.HasKey(e => e.IdDisciplina);

            entity.ToTable("DISCIPLINAS");

            entity.Property(e => e.IdDisciplina).HasColumnName("id_disciplina");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.NombreDisciplina)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_disciplina");
        });

        modelBuilder.Entity<Ejercicio>(entity =>
        {
            entity.HasKey(e => e.IdEjercicio);

            entity.ToTable("EJERCICIOS");

            entity.Property(e => e.IdEjercicio).HasColumnName("id_ejercicio");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.GrupoMuscular)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("grupo_muscular");
            entity.Property(e => e.NombreEjercicio)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_ejercicio");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado);

            entity.ToTable("EMPLEADOS");

            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");
            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.Legajo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("legajo");
            entity.Property(e => e.SueldoBase)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sueldo_base");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EMPLEADOS_x_PERSONAS");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EMPLEADOS_x_ROLES");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EMPLEADOS_x_SEDES");
        });

        modelBuilder.Entity<EmpleadosDisciplina>(entity =>
        {
            entity.HasKey(e => e.IdEmpleadoDisciplina);

            entity.ToTable("EMPLEADOS_DISCIPLINAS");

            entity.Property(e => e.IdEmpleadoDisciplina).HasColumnName("id_empleado_disciplina");
            entity.Property(e => e.IdDisciplina).HasColumnName("id_disciplina");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");

            entity.HasOne(d => d.IdDisciplinaNavigation).WithMany(p => p.EmpleadosDisciplinas)
                .HasForeignKey(d => d.IdDisciplina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EMPLEADOS_DISCIPLINAS_x_DISCIPLINAS");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.EmpleadosDisciplinas)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EMPLEADOS_DISCIPLINAS_x_EMPLEADOS");
        });

        modelBuilder.Entity<Equipamiento>(entity =>
        {
            entity.HasKey(e => e.IdEquipo);

            entity.ToTable("EQUIPAMIENTO", tb => tb.HasTrigger("TR_AuditarCambiosEquipamiento"));

            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.EstadoOperativo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado_operativo");
            entity.Property(e => e.FechaAdquisicion).HasColumnName("fecha_adquisicion");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreMaquina)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_maquina");
            entity.Property(e => e.NumeroSerie)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numero_serie");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Equipamientos)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EQUIPAMIENTO_x_SEDES");
        });

        modelBuilder.Entity<EstadosReclamo>(entity =>
        {
            entity.HasKey(e => e.IdEstadoReclamo);

            entity.ToTable("ESTADOS_RECLAMO");

            entity.Property(e => e.IdEstadoReclamo).HasColumnName("id_estado_reclamo");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_estado");
        });

        modelBuilder.Entity<EstadosTurno>(entity =>
        {
            entity.HasKey(e => e.IdEstadoTurno);

            entity.ToTable("ESTADOS_TURNO");

            entity.Property(e => e.IdEstadoTurno).HasColumnName("id_estado_turno");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_estado");
        });

        modelBuilder.Entity<EvaluacionesFisica>(entity =>
        {
            entity.HasKey(e => e.IdEvaluacion);

            entity.ToTable("EVALUACIONES_FISICAS");

            entity.Property(e => e.IdEvaluacion).HasColumnName("id_evaluacion");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.IdTurno).HasColumnName("id_turno");
            entity.Property(e => e.ObjetivosAsignados)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("objetivos_asignados");
            entity.Property(e => e.PesoKg)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("peso_kg");
            entity.Property(e => e.PlanAlimentario)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("plan_alimentario");
            entity.Property(e => e.PorcentajeGrasa)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porcentaje_grasa");
            entity.Property(e => e.PorcentajeMasaMuscular)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porcentaje_masa_muscular");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.EvaluacionesFisicas)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EVALUACIONES_FISICAS_x_SOCIOS");

            entity.HasOne(d => d.IdTurnoNavigation).WithMany(p => p.EvaluacionesFisicas)
                .HasForeignKey(d => d.IdTurno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EVALUACIONES_FISICAS_x_TURNOS");
        });

        modelBuilder.Entity<FormasPago>(entity =>
        {
            entity.HasKey(e => e.IdFormaPago);

            entity.ToTable("FORMAS_PAGO");

            entity.Property(e => e.IdFormaPago).HasColumnName("id_forma_pago");
            entity.Property(e => e.NombreFormaPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_forma_pago");
        });

        modelBuilder.Entity<HistorialPreciosPlane>(entity =>
        {
            entity.HasKey(e => e.IdHistorialPlan);

            entity.ToTable("HISTORIAL_PRECIOS_PLANES");

            entity.Property(e => e.IdHistorialPlan).HasColumnName("id_historial_plan");
            entity.Property(e => e.FechaCambio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_cambio");
            entity.Property(e => e.IdPlan).HasColumnName("id_plan");
            entity.Property(e => e.PrecioAnterior)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_anterior");
            entity.Property(e => e.PrecioNuevo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_nuevo");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.HistorialPreciosPlanes)
                .HasForeignKey(d => d.IdPlan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HISTORIAL_PRECIOS_PLANES_x_PLANES");
        });

        modelBuilder.Entity<HistorialPreciosProducto>(entity =>
        {
            entity.HasKey(e => e.IdHistorialProd);

            entity.ToTable("HISTORIAL_PRECIOS_PRODUCTOS");

            entity.Property(e => e.IdHistorialProd).HasColumnName("id_historial_prod");
            entity.Property(e => e.FechaCambio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_cambio");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.PrecioAnterior)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_anterior");
            entity.Property(e => e.PrecioNuevo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_nuevo");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.HistorialPreciosProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HISTORIAL_PRECIOS_PRODUCTOS_x_PRODUCTOS");
        });

        modelBuilder.Entity<HorariosSede>(entity =>
        {
            entity.HasKey(e => e.IdHorarioSede);

            entity.ToTable("HORARIOS_SEDE");

            entity.Property(e => e.IdHorarioSede).HasColumnName("id_horario_sede");
            entity.Property(e => e.DiaSemana)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dia_semana");
            entity.Property(e => e.HoraApertura).HasColumnName("hora_apertura");
            entity.Property(e => e.HoraCierre).HasColumnName("hora_cierre");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.HorariosSedes)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("HORARIOS_SEDE_x_SEDES");
        });

        modelBuilder.Entity<IncidenciasReclamo>(entity =>
        {
            entity.HasKey(e => e.IdReclamo);

            entity.ToTable("INCIDENCIAS_RECLAMOS");

            entity.Property(e => e.IdReclamo).HasColumnName("id_reclamo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdEstadoReclamo).HasColumnName("id_estado_reclamo");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.IdTecnicoAsignado).HasColumnName("id_tecnico_asignado");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.IncidenciasReclamos)
                .HasForeignKey(d => d.IdEquipo)
                .HasConstraintName("INCIDENCIAS_RECLAMOS_x_EQUIPAMIENTO");

            entity.HasOne(d => d.IdEstadoReclamoNavigation).WithMany(p => p.IncidenciasReclamos)
                .HasForeignKey(d => d.IdEstadoReclamo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("INCIDENCIAS_RECLAMOS_x_ESTADOS_RECLAMO");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.IncidenciasReclamos)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("INCIDENCIAS_RECLAMOS_x_SEDES");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.IncidenciasReclamos)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("INCIDENCIAS_RECLAMOS_x_SOCIOS");

            entity.HasOne(d => d.IdTecnicoAsignadoNavigation).WithMany(p => p.IncidenciasReclamos)
                .HasForeignKey(d => d.IdTecnicoAsignado)
                .HasConstraintName("INCIDENCIAS_RECLAMOS_x_EMPLEADOS");
        });

        modelBuilder.Entity<LiquidacionesSueldo>(entity =>
        {
            entity.HasKey(e => e.IdLiquidacion);

            entity.ToTable("LIQUIDACIONES_SUELDOS");

            entity.Property(e => e.IdLiquidacion).HasColumnName("id_liquidacion");
            entity.Property(e => e.FechaPago).HasColumnName("fecha_pago");
            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.MontoNeto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto_neto");
            entity.Property(e => e.PeriodoAnio).HasColumnName("periodo_anio");
            entity.Property(e => e.PeriodoMes).HasColumnName("periodo_mes");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.LiquidacionesSueldos)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LIQUIDACIONES_SUELDOS_x_EMPLEADOS");
        });

        modelBuilder.Entity<Localidade>(entity =>
        {
            entity.HasKey(e => e.IdLocalidad);

            entity.ToTable("LOCALIDADES");

            entity.Property(e => e.IdLocalidad).HasColumnName("id_localidad");
            entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");
            entity.Property(e => e.NombreLocalidad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_localidad");

            entity.HasOne(d => d.IdProvinciaNavigation).WithMany(p => p.Localidades)
                .HasForeignKey(d => d.IdProvincia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LOCALIDADES_x_PROVINCIAS");
        });

        modelBuilder.Entity<LogAccesosDenegado>(entity =>
        {
            entity.HasKey(e => e.IdLog);

            entity.ToTable("LOG_ACCESOS_DENEGADOS");

            entity.Property(e => e.IdLog).HasColumnName("id_log");
            entity.Property(e => e.FechaHora)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.MotivoBloqueo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("motivo_bloqueo");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.LogAccesosDenegados)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LOG_ACCESOS_DENEGADOS_x_SEDES");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.LogAccesosDenegados)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LOG_ACCESOS_DENEGADOS_x_SOCIOS");
        });

        modelBuilder.Entity<LogCambiosEstadoSocio>(entity =>
        {
            entity.HasKey(e => e.IdLogEstado);

            entity.ToTable("LOG_CAMBIOS_ESTADO_SOCIO");

            entity.Property(e => e.IdLogEstado).HasColumnName("id_log_estado");
            entity.Property(e => e.EstadoAnterior)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_anterior");
            entity.Property(e => e.EstadoNuevo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_nuevo");
            entity.Property(e => e.FechaCambio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_cambio");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.UsuarioSistema)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario_sistema");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.LogCambiosEstadoSocios)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LOG_CAMBIOS_ESTADO_SOCIO_x_SOCIOS");
        });

        modelBuilder.Entity<MantenimientoEquipamiento>(entity =>
        {
            entity.HasKey(e => e.IdMantenimiento);

            entity.ToTable("MANTENIMIENTO_EQUIPAMIENTO");

            entity.Property(e => e.IdMantenimiento).HasColumnName("id_mantenimiento");
            entity.Property(e => e.Costo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("costo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaMantenimiento).HasColumnName("fecha_mantenimiento");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");

            entity.HasOne(d => d.IdEquipoNavigation).WithMany(p => p.MantenimientoEquipamientos)
                .HasForeignKey(d => d.IdEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MANTENIMIENTO_EQUIPAMIENTO_x_EQUIPAMIENTO");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.MantenimientoEquipamientos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("MANTENIMIENTO_EQUIPAMIENTO_x_PROVEEDORES");
        });

        modelBuilder.Entity<MovimientosStock>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento);

            entity.ToTable("MOVIMIENTOS_STOCK");

            entity.HasIndex(e => new { e.IdProducto, e.IdSede, e.FechaMovimiento }, "IX_MOVIMIENTOS_STOCK_PRODUCTO_SEDE");

            entity.Property(e => e.IdMovimiento).HasColumnName("id_movimiento");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("(getdate())", "DF_MOVIMIENTOS_STOCK_FECHA")
                .HasColumnType("datetime")
                .HasColumnName("fecha_movimiento");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("observaciones");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("tipo_movimiento");
            entity.Property(e => e.UsuarioSistema)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario_sistema");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.MovimientosStocks)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MOVIMIENTOS_STOCK_PRODUCTOS");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.MovimientosStocks)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MOVIMIENTOS_STOCK_SEDES");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.MovimientosStocks)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK_MOVIMIENTOS_STOCK_VENTAS");
        });

        modelBuilder.Entity<PagosCuota>(entity =>
        {
            entity.HasKey(e => e.IdPago);

            entity.ToTable("PAGOS_CUOTAS");

            entity.Property(e => e.IdPago).HasColumnName("id_pago");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())", "DF_PAGOS_CUOTAS_FECHA")
                .HasColumnType("datetime")
                .HasColumnName("fecha_pago");
            entity.Property(e => e.IdCobro).HasColumnName("id_cobro");
            entity.Property(e => e.IdFormaPago).HasColumnName("id_forma_pago");
            entity.Property(e => e.Importe)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("importe");
            entity.Property(e => e.UsuarioSistema)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario_sistema");

            entity.HasOne(d => d.IdCobroNavigation).WithMany(p => p.PagosCuota)
                .HasForeignKey(d => d.IdCobro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PAGOS_CUOTAS_CUOTAS");

            entity.HasOne(d => d.IdFormaPagoNavigation).WithMany(p => p.PagosCuota)
                .HasForeignKey(d => d.IdFormaPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PAGOS_CUOTAS_FORMAS");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona);

            entity.ToTable("PERSONAS");

            entity.HasIndex(e => e.Dni, "UX_PERSONAS_DNI").IsUnique();

            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EstadoCivil)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_civil");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Ocupacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ocupacion");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Plane>(entity =>
        {
            entity.HasKey(e => e.IdPlan);

            entity.ToTable("PLANES");

            entity.Property(e => e.IdPlan).HasColumnName("id_plan");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.DuracionMeses).HasColumnName("duracion_meses");
            entity.Property(e => e.NombrePlan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_plan");
            entity.Property(e => e.PermiteMultisede).HasColumnName("permite_multisede");
            entity.Property(e => e.PrecioActual)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_actual");
        });

        modelBuilder.Entity<PlanesDisciplina>(entity =>
        {
            entity.HasKey(e => e.IdPlanDisciplina);

            entity.ToTable("PLANES_DISCIPLINAS");

            entity.HasIndex(e => new { e.IdPlan, e.IdDisciplina }, "UX_PLANES_DISCIPLINAS").IsUnique();

            entity.Property(e => e.IdPlanDisciplina).HasColumnName("id_plan_disciplina");
            entity.Property(e => e.IdDisciplina).HasColumnName("id_disciplina");
            entity.Property(e => e.IdPlan).HasColumnName("id_plan");

            entity.HasOne(d => d.IdDisciplinaNavigation).WithMany(p => p.PlanesDisciplinas)
                .HasForeignKey(d => d.IdDisciplina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PLANES_DISCIPLINAS_x_DISCIPLINAS");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.PlanesDisciplinas)
                .HasForeignKey(d => d.IdPlan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PLANES_DISCIPLINAS_x_PLANES");
        });

        modelBuilder.Entity<PremiosCatalogo>(entity =>
        {
            entity.HasKey(e => e.IdPremio);

            entity.ToTable("PREMIOS_CATALOGO");

            entity.Property(e => e.IdPremio).HasColumnName("id_premio");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.MesesAntiguedadRequeridos).HasColumnName("meses_antiguedad_requeridos");
            entity.Property(e => e.NombrePremio)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_premio");
        });

        modelBuilder.Entity<PremiosOtorgado>(entity =>
        {
            entity.HasKey(e => e.IdOtorgamiento);

            entity.ToTable("PREMIOS_OTORGADOS");

            entity.Property(e => e.IdOtorgamiento).HasColumnName("id_otorgamiento");
            entity.Property(e => e.EstadoCanje)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_canje");
            entity.Property(e => e.FechaOtorgamiento)
                .HasColumnType("datetime")
                .HasColumnName("fecha_otorgamiento");
            entity.Property(e => e.IdPremio).HasColumnName("id_premio");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");

            entity.HasOne(d => d.IdPremioNavigation).WithMany(p => p.PremiosOtorgados)
                .HasForeignKey(d => d.IdPremio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PREMIOS_OTORGADOS_x_PREMIOS_CATALOGO");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.PremiosOtorgados)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PREMIOS_OTORGADOS_x_SOCIOS");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);

            entity.ToTable("PRODUCTOS", tb => tb.HasTrigger("TR_AuditarCambiosPrecios"));

            entity.HasIndex(e => e.CodigoBarras, "UX_PRODUCTOS_CODIGO_BARRAS").IsUnique();

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.CodigoBarras)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigo_barras");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_producto");
            entity.Property(e => e.PrecioActual)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_actual");
            entity.Property(e => e.StockMinimo).HasColumnName("stock_minimo");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PRODUCTOS_x_CATEGORIAS_PRODUCTOS");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor);

            entity.ToTable("PROVEEDORES");

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.Cuit)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("cuit");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("razon_social");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.IdProvincia);

            entity.ToTable("PROVINCIAS");

            entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");
            entity.Property(e => e.NombreProvincia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_provincia");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva);

            entity.ToTable("RESERVAS");

            entity.HasIndex(e => new { e.IdSocio, e.IdClase }, "UX_RESERVAS_SOCIO_CLASE").IsUnique();

            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.EstadoReserva)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_reserva");
            entity.Property(e => e.FechaReserva)
                .HasColumnType("datetime")
                .HasColumnName("fecha_reserva");
            entity.Property(e => e.IdClase).HasColumnName("id_clase");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");

            entity.HasOne(d => d.IdClaseNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdClase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RESERVAS_x_CLASES");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RESERVAS_x_SOCIOS");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("ROLES");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<RolesSistema>(entity =>
        {
            entity.ToTable("ROLES_SISTEMA");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rol");
        });

        modelBuilder.Entity<Rutina>(entity =>
        {
            entity.HasKey(e => e.IdRutina);

            entity.ToTable("RUTINAS");

            entity.Property(e => e.IdRutina).HasColumnName("id_rutina");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.IdEntrenador).HasColumnName("id_entrenador");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.Objetivo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("objetivo");

            entity.HasOne(d => d.IdEntrenadorNavigation).WithMany(p => p.Rutinas)
                .HasForeignKey(d => d.IdEntrenador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RUTINAS_x_EMPLEADOS");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.Rutinas)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RUTINAS_x_SOCIOS");
        });

        modelBuilder.Entity<Salone>(entity =>
        {
            entity.HasKey(e => e.IdSalon);

            entity.ToTable("SALONES");

            entity.Property(e => e.IdSalon).HasColumnName("id_salon");
            entity.Property(e => e.CapacidadMaxima).HasColumnName("capacidad_maxima");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreSalon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_salon");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Salones)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SALONES_x_SEDES");
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede);

            entity.ToTable("SEDES");

            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IdCalle).HasColumnName("id_calle");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_sede");
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdCalleNavigation).WithMany(p => p.Sedes)
                .HasForeignKey(d => d.IdCalle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SEDES_x_CALLES");
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.HasKey(e => e.IdSocio);

            entity.ToTable("SOCIOS", tb => tb.HasTrigger("TR_AuditarCambiosEstadoSocio"));

            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.EstadoCuenta)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_cuenta");
            entity.Property(e => e.FechaIngresoAlta).HasColumnName("fecha_ingreso_alta");
            entity.Property(e => e.IdCalle).HasColumnName("id_calle");
            entity.Property(e => e.IdConvenio).HasColumnName("id_convenio");
            entity.Property(e => e.IdPersona).HasColumnName("id_persona");

            entity.HasOne(d => d.IdCalleNavigation).WithMany(p => p.Socios)
                .HasForeignKey(d => d.IdCalle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SOCIOS_x_CALLES");

            entity.HasOne(d => d.IdConvenioNavigation).WithMany(p => p.Socios)
                .HasForeignKey(d => d.IdConvenio)
                .HasConstraintName("SOCIOS_x_CONVENIOS_EMPRESA");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Socios)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SOCIOS_x_PERSONAS");
        });

        modelBuilder.Entity<StockSede>(entity =>
        {
            entity.HasKey(e => e.IdStockSede);

            entity.ToTable("STOCK_SEDE", tb =>
                {
                    tb.HasTrigger("TR_ControlarStockNegativo");
                    tb.HasTrigger("TR_RegistrarMovimientosStock");
                });

            entity.HasIndex(e => new { e.IdSede, e.IdProducto }, "UX_STOCK_SEDE_PRODUCTO").IsUnique();

            entity.Property(e => e.IdStockSede).HasColumnName("id_stock_sede");
            entity.Property(e => e.CantidadActual).HasColumnName("cantidad_actual");
            entity.Property(e => e.FilaVersion)
                .IsRowVersion()
                .IsConcurrencyToken()
                .HasColumnName("fila_version");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.StockSedes)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("STOCK_SEDE_x_PRODUCTOS");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.StockSedes)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("STOCK_SEDE_x_SEDES");
        });

        modelBuilder.Entity<TransferenciasStock>(entity =>
        {
            entity.HasKey(e => e.IdTransferencia);

            entity.ToTable("TRANSFERENCIAS_STOCK");

            entity.Property(e => e.IdTransferencia).HasColumnName("id_transferencia");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("PENDIENTE", "DF_TRANSFERENCIAS_ESTADO")
                .HasColumnName("estado");
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())", "DF_TRANSFERENCIAS_FECHA")
                .HasColumnType("datetime")
                .HasColumnName("fecha_solicitud");
            entity.Property(e => e.IdSedeDestino).HasColumnName("id_sede_destino");
            entity.Property(e => e.IdSedeOrigen).HasColumnName("id_sede_origen");
            entity.Property(e => e.UsuarioSistema)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario_sistema");

            entity.HasOne(d => d.IdSedeDestinoNavigation).WithMany(p => p.TransferenciasStockIdSedeDestinoNavigations)
                .HasForeignKey(d => d.IdSedeDestino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TRANSFERENCIAS_DESTINO");

            entity.HasOne(d => d.IdSedeOrigenNavigation).WithMany(p => p.TransferenciasStockIdSedeOrigenNavigations)
                .HasForeignKey(d => d.IdSedeOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TRANSFERENCIAS_ORIGEN");
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.IdTurno);

            entity.ToTable("TURNOS");

            entity.Property(e => e.IdTurno).HasColumnName("id_turno");
            entity.Property(e => e.FechaTurno).HasColumnName("fecha_turno");
            entity.Property(e => e.HoraTurno).HasColumnName("hora_turno");
            entity.Property(e => e.IdEstadoTurno).HasColumnName("id_estado_turno");
            entity.Property(e => e.IdProfesional).HasColumnName("id_profesional");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");

            entity.HasOne(d => d.IdEstadoTurnoNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdEstadoTurno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TURNOS_x_ESTADOS_TURNO");

            entity.HasOne(d => d.IdProfesionalNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdProfesional)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TURNOS_x_EMPLEADOS");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TURNOS_x_SEDES");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TURNOS_x_SOCIOS");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("USUARIOS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasMany(d => d.Idrols).WithMany(p => p.Idusuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "Rolusuario",
                    r => r.HasOne<RolesSistema>().WithMany()
                        .HasForeignKey("Idrol")
                        .HasConstraintName("FK_ROLUSUARIO_ROLES_SISTEMA"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("Idusuario")
                        .HasConstraintName("FK_ROLUSUARIO_USUARIOS"),
                    j =>
                    {
                        j.HasKey("Idusuario", "Idrol");
                        j.ToTable("ROLUSUARIO");
                        j.IndexerProperty<int>("Idusuario").HasColumnName("idusuario");
                        j.IndexerProperty<int>("Idrol").HasColumnName("idrol");
                    });
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta);

            entity.ToTable("VENTAS");

            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.FechaHoraVenta)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora_venta");
            entity.Property(e => e.IdFormaPago).HasColumnName("id_forma_pago");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.IdVendedor).HasColumnName("id_vendedor");
            entity.Property(e => e.ImputadoACuenta).HasColumnName("imputado_a_cuenta");

            entity.HasOne(d => d.IdFormaPagoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdFormaPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VENTAS_x_FORMAS_PAGO");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VENTAS_x_SEDES");

            entity.HasOne(d => d.IdSocioNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdSocio)
                .HasConstraintName("VENTAS_x_SOCIOS");

            entity.HasOne(d => d.IdVendedorNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdVendedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VENTAS_x_EMPLEADOS");
        });

        modelBuilder.Entity<VwControlAptosMedico>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ControlAptosMedicos");

            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.EstadoApto)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("estado_apto");
            entity.Property(e => e.FechaEmision).HasColumnName("fecha_emision");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.IdApto).HasColumnName("id_apto");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<VwCostosMantenimientoEquipamiento>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_CostosMantenimientoEquipamiento");

            entity.Property(e => e.CantidadIntervenciones).HasColumnName("cantidad_intervenciones");
            entity.Property(e => e.CostoTotal)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("costo_total");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreMaquina)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_maquina");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_sede");
            entity.Property(e => e.UltimoMantenimiento).HasColumnName("ultimo_mantenimiento");
        });

        modelBuilder.Entity<VwCuotasPendiente>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_CuotasPendientes");

            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.EstadoCuota)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("estado_cuota");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.IdCobro).HasColumnName("id_cobro");
            entity.Property(e => e.IdContrato).HasColumnName("id_contrato");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.ImportePagado)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("importe_pagado");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NumeroCuota).HasColumnName("numero_cuota");
            entity.Property(e => e.Saldo)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("saldo");
        });

        modelBuilder.Entity<VwEstadoCuentaSocio>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_EstadoCuentaSocios");

            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.CuotasEmitidas).HasColumnName("cuotas_emitidas");
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.ImporteEmitido)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("importe_emitido");
            entity.Property(e => e.ImportePagado)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("importe_pagado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.SaldoPendiente)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("saldo_pendiente");
        });

        modelBuilder.Entity<VwIncidenciasActiva>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_IncidenciasActivas");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.IdEstadoReclamo).HasColumnName("id_estado_reclamo");
            entity.Property(e => e.IdReclamo).HasColumnName("id_reclamo");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.IdTecnicoAsignado).HasColumnName("id_tecnico_asignado");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_estado");
            entity.Property(e => e.NombreMaquina)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_maquina");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_sede");
        });

        modelBuilder.Entity<VwMembresiasVigente>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_MembresiasVigentes");

            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.EstadoContrato)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_contrato");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.IdContrato).HasColumnName("id_contrato");
            entity.Property(e => e.IdPlan).HasColumnName("id_plan");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NombrePlan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_plan");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_sede");
        });

        modelBuilder.Entity<VwOcupacionClasesGrupale>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_OcupacionClasesGrupales");

            entity.Property(e => e.CupoLimite).HasColumnName("cupo_limite");
            entity.Property(e => e.CuposDisponibles).HasColumnName("cupos_disponibles");
            entity.Property(e => e.CuposOcupados).HasColumnName("cupos_ocupados");
            entity.Property(e => e.FechaClase).HasColumnName("fecha_clase");
            entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.IdClase).HasColumnName("id_clase");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreDisciplina)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_disciplina");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_sede");
        });

        modelBuilder.Entity<VwRentabilidadConvenio>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_RentabilidadConvenios");

            entity.Property(e => e.Deuda)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("deuda");
            entity.Property(e => e.IdConvenio).HasColumnName("id_convenio");
            entity.Property(e => e.ImporteCobrado)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("importe_cobrado");
            entity.Property(e => e.ImporteFacturado)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("importe_facturado");
            entity.Property(e => e.PorcentajeCobrabilidad)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("porcentaje_cobrabilidad");
            entity.Property(e => e.PorcentajeDescuento)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porcentaje_descuento");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("razon_social");
            entity.Property(e => e.SociosAsociados).HasColumnName("socios_asociados");
        });

        modelBuilder.Entity<VwStockDisponiblePorSede>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_StockDisponiblePorSede");

            entity.Property(e => e.CantidadActual).HasColumnName("cantidad_actual");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_producto");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_sede");
            entity.Property(e => e.StockCritico).HasColumnName("stock_critico");
            entity.Property(e => e.StockMinimo).HasColumnName("stock_minimo");
        });

        modelBuilder.Entity<VwVentasDetalladasProducto>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_VentasDetalladasProductos");

            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaHoraVenta)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hora_venta");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.IdSocio).HasColumnName("id_socio");
            entity.Property(e => e.IdVendedor).HasColumnName("id_vendedor");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.ImporteLinea)
                .HasColumnType("decimal(21, 2)")
                .HasColumnName("importe_linea");
            entity.Property(e => e.ImputadoACuenta).HasColumnName("imputado_a_cuenta");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_categoria");
            entity.Property(e => e.NombreFormaPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_forma_pago");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_producto");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_sede");
            entity.Property(e => e.PrecioUnitarioHistorico)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario_historico");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
