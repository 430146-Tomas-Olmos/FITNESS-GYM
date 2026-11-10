using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Models.Tablas;

namespace FITLIFENETWORKPROYECTO.Repositorys.Interfaces
{
    public interface IRepository
    {
        public Task<List<ReporteRecaudacionSede>> GetRecaudacionSede(int? año, int? mes);

        public Task<List<AuditoriaAptoMedico>> GetAccesoDenegadoApto();

        public Task<List<LogAccesosDenegado>> GetAuditoriaAccesosDenegadosAsync();

        public Task<List<LogCambiosEstadoSocio>> GetAuditoriaCambiosEstadoSocioAsync();

        public Task<List<AuditoriaEquipamiento>> GetAuditoriaEquipamientoAsync();

        public Task<List<PremiosCanjeados>> GetPremiosCanjeados(string estadoCanje, int minMeses);

        public Task<List<Top3Productos>> GetTop3Productos(int? año);

        public Task<List<ReporteReservaTT>> GetReporteReserva(DateTime Fecha1, DateTime Fecha2, string Estado1, string Estado2);

        public Task<List<EvaluacionEstadistica>> GetEvaluacionesEstadistica();

        public Task<List<ControlAptoMedico>> GetControlAptosMedicos();

        public Task<List<RentabilidadConvenio>> GetRentabilidadConvenios(string? Estado);

        public Task<List<Socio>> GetSociosAsync();

        public Task<Socio?> GetSocioByIdAsync(int idSocio);

        public Task<Socio?> UpdateSocioAsync(int idSocio, Socio socio);

        public Task<bool> DeleteSocioAsync(int idSocio);

        public Task<Socio> CreateSocioAsync(Socio socio);

        public Task<(Socio Socio, ContratosMembresium Contrato, List<CuotasCobro> Cuotas)> CreateSocioConContratoAsync(Socio socio, ContratosMembresium contrato, int idFormaPago = 1);

        public Task<List<Persona>> GetPersonasAsync();

        public Task<Persona?> GetPersonaByIdAsync(int idPersona);

        public Task<Persona> CreatePersonaAsync(Persona persona);

        public Task<List<Sede>> GetSedesAsync();

        public Task<Sede?> GetSedeByIdAsync(int idSede);

        public Task<Sede> CreateSedeAsync(Sede sede);

        public Task<List<Role>> GetRolesAsync();

        public Task<Role?> GetRoleByIdAsync(int idRol);

        public Task<Role> CreateRoleAsync(Role role);

        public Task<List<CategoriasProducto>> GetCategoriasProductosAsync();

        public Task<CategoriasProducto?> GetCategoriaProductoByIdAsync(int idCategoria);

        public Task<CategoriasProducto> CreateCategoriaProductoAsync(CategoriasProducto categoria);

        public Task<List<FormasPago>> GetFormasPagoAsync();

        public Task<FormasPago?> GetFormaPagoByIdAsync(int idFormaPago);

        public Task<FormasPago> CreateFormaPagoAsync(FormasPago formaPago);

        public Task<List<EstadosReclamo>> GetEstadosReclamoAsync();

        public Task<EstadosReclamo?> GetEstadoReclamoByIdAsync(int idEstadoReclamo);

        public Task<EstadosReclamo> CreateEstadoReclamoAsync(EstadosReclamo estadoReclamo);

        public Task<List<Provincia>> GetProvinciasAsync();

        public Task<Provincia?> GetProvinciaByIdAsync(int idProvincia);

        public Task<Provincia> CreateProvinciaAsync(Provincia provincia);

        public Task<List<Localidade>> GetLocalidadesAsync(int? idProvincia = null);

        public Task<Localidade?> GetLocalidadByIdAsync(int idLocalidad);

        public Task<Localidade> CreateLocalidadAsync(Localidade localidad);

        public Task<List<Barrio>> GetBarriosAsync(int? idLocalidad = null);

        public Task<Barrio?> GetBarrioByIdAsync(int idBarrio);

        public Task<Barrio> CreateBarrioAsync(Barrio barrio);

        public Task<List<Calle>> GetCallesAsync(int? idBarrio = null);

        public Task<Calle?> GetCalleByIdAsync(int idCalle);

        public Task<Calle> CreateCalleAsync(Calle calle);

        public Task<List<Plane>> GetPlanesAsync();

        public Task<List<CuotasCobro>> GetCuotasPendientesAsync(int? idSocio = null);

        public Task<List<PagosCuota>> GetPagosCuotaAsync(int? idCobro = null, int? idSocio = null);

        public Task<PagosCuota> RegistrarPagoCuotaAsync(int idCobro, int idFormaPago, decimal importe, string? usuarioSistema = null);

        public Task<List<AptosMedico>> GetAptosMedicosAsync();

        public Task<AptosMedico> CreateAptoMedicoAsync(AptosMedico apto);

        public Task<AccesoValidacionResponseDto> ValidarAccesoAsync(int idSocio, int idSede, DateTime fechaHora);

        public Task<HistorialSocioDto> GetHistorialSocioAsync(int idSocio);

        public Task<List<Reserva>> GetReservasEnListaEsperaAsync(int idClase);

        public Task<Reserva?> PromoverSiguienteListaEsperaAsync(int idClase);

        public Task<List<Clase>> GetClasesAsync();

        public Task<List<Reserva>> GetReservasAsync(int? idSocio = null);

        public Task<Reserva?> GetReservaByIdAsync(int idReserva);

        public Task<Reserva> CreateReservaAsync(Reserva reserva);

        public Task<Reserva?> UpdateReservaAsync(int idReserva, Reserva reserva);

        public Task<bool> DeleteReservaAsync(int idReserva);

        public Task<List<Producto>> GetProductosAsync(int? idSede = null);

        public Task<Producto?> GetProductoByIdAsync(int idProducto);

        public Task<Producto> CreateProductoAsync(Producto producto);

        public Task<Producto?> UpdateProductoAsync(int idProducto, Producto producto);

        public Task<bool> DeleteProductoAsync(int idProducto);

        public Task<List<StockSede>> GetStockPorSedeAsync(int? idSede = null);

        public Task<Venta> CrearVentaAsync(Venta venta);

        public Task<List<Equipamiento>> GetEquiposAsync(int? idSede = null);

        public Task<List<IncidenciasReclamo>> GetIncidenciasAsync(int? idSede = null);

        public Task<IncidenciasReclamo> CreateIncidenciaAsync(IncidenciasReclamo incidencia);

        public Task<List<ContratosMembresium>> GetContratosAsync(int? idSocio = null);

        public Task<ContratosMembresium?> GetContratoByIdAsync(int idContrato);

        public Task<ContratosMembresium> CreateContratoAsync(ContratosMembresium contrato);

        public Task<ContratosMembresium> RenovarContratoAsync(int idContrato, ContratosMembresium nuevoContrato, int idFormaPago = 1);

        public Task<ContratosMembresium?> UpdateContratoAsync(int idContrato, ContratosMembresium contrato);

        public Task<bool> DeleteContratoAsync(int idContrato);

        public Task<List<ConveniosEmpresa>> GetConveniosAsync();

        public Task<ConveniosEmpresa?> GetConvenioByIdAsync(int idConvenio);

        public Task<ConveniosEmpresa> CreateConvenioAsync(ConveniosEmpresa convenio);

        public Task<ConveniosEmpresa?> UpdateConvenioAsync(int idConvenio, ConveniosEmpresa convenio);

        public Task<bool> DeleteConvenioAsync(int idConvenio);

        public Task<List<Proveedore>> GetProveedoresAsync();

        public Task<Proveedore> CreateProveedorAsync(Proveedore proveedor);

        public Task<List<ComprasStock>> GetComprasAsync(int? idSede = null);

        public Task<ComprasStock> CreateCompraAsync(ComprasStock compra);

        public Task<List<Caja>> GetCajasAsync(int? idSede = null);

        public Task<Caja> CreateCajaAsync(Caja caja);

        public Task<List<TransferenciasStock>> GetTransferenciasAsync(int? idSede = null);

        public Task<TransferenciasStock?> GetTransferenciaByIdAsync(int idTransferencia);

        public Task<TransferenciasStock> CreateTransferenciaAsync(TransferenciasStock transferencia);

        public Task<TransferenciasStock?> UpdateTransferenciaAsync(int idTransferencia, TransferenciasStock transferencia);

        public Task<bool> DeleteTransferenciaAsync(int idTransferencia);

        public Task<List<DevolucionesVentum>> GetDevolucionesAsync(int? idVenta = null);

        public Task<DevolucionesVentum> CreateDevolucionAsync(DevolucionesVentum devolucion);

        public Task<List<MovimientosStock>> GetMovimientosStockAsync(int? idSede = null, int? idProducto = null);

        public Task<MovimientosStock> CreateMovimientoStockAsync(MovimientosStock movimiento);

        public Task<List<CierresCaja>> GetCierresCajaAsync(int? idCaja = null);

        public Task<CierresCaja?> GetCierreCajaByIdAsync(int idCierre);

        public Task<CierresCaja> CreateCierreCajaAsync(CierresCaja cierreCaja);

        public Task<CierresCaja?> UpdateCierreCajaAsync(int idCierre, CierresCaja cierreCaja);

        public Task<bool> DeleteCierreCajaAsync(int idCierre);

        public Task<List<Empleado>> GetEmpleadosAsync(int? idSede = null, int? idRol = null);

        public Task<Empleado> CreateEmpleadoAsync(Empleado empleado);

        public Task<List<Rutina>> GetRutinasAsync(int? idSocio = null, int? idEntrenador = null);

        public Task<Rutina> CreateRutinaAsync(Rutina rutina);

        public Task<List<Turno>> GetTurnosAsync(int? idSocio = null, int? idProfesional = null);

        public Task<Turno?> GetTurnoByIdAsync(int idTurno);

        public Task<Turno> CreateTurnoAsync(Turno turno);

        public Task<Turno?> UpdateTurnoAsync(int idTurno, Turno turno);

        public Task<bool> DeleteTurnoAsync(int idTurno);

        public Task<List<MantenimientoEquipamiento>> GetMantenimientosEquipamientoAsync(int? idEquipo = null);

        public Task<MantenimientoEquipamiento?> GetMantenimientoEquipamientoByIdAsync(int idMantenimiento);

        public Task<MantenimientoEquipamiento> CreateMantenimientoEquipamientoAsync(MantenimientoEquipamiento mantenimiento);

        public Task<MantenimientoEquipamiento?> UpdateMantenimientoEquipamientoAsync(int idMantenimiento, MantenimientoEquipamiento mantenimiento);

        public Task<bool> DeleteMantenimientoEquipamientoAsync(int idMantenimiento);

        public Task<List<FITLIFENETWORKPROYECTO.Models.Views_Dashboard.VwEstadoCuentaSocio>> GetEstadoCuentaSociosAsync();

        public Task<List<FITLIFENETWORKPROYECTO.Models.Views_Dashboard.VwMembresiasVigente>> GetMembresiasVigentesAsync();

        public Task<List<FITLIFENETWORKPROYECTO.Models.Views_Dashboard.VwOcupacionClasesGrupale>> GetOcupacionClasesAsync();

        public Task<List<FITLIFENETWORKPROYECTO.Models.Views_Dashboard.VwCostosMantenimientoEquipamiento>> GetCostosMantenimientoEquipamientoAsync();
    }
}
