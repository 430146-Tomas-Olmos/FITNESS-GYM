using FITLIFENETWORKPROYECTO.DTO;

namespace FITLIFENETWORKPROYECTO.Services.Interfaces
{
    public interface IServices
    {
        public Task<List<ReporteRecaudacionSede>> GetRecaudacionSede(int? año, int? mes);

        public Task<List<AuditoriaAptoMedico>> GetAccesoDenegadoApto(DateTime? desde, DateTime? hasta);

        public Task<List<AuditoriaAccesoDto>> GetAuditoriaAccesosDenegadosAsync();

        public Task<List<AuditoriaEstadoSocioDto>> GetAuditoriaCambiosEstadoSocioAsync();

        public Task<List<AuditoriaEquipamientoDto>> GetAuditoriaEquipamientoAsync();

        public Task<List<PremiosCanjeados>> GetPremiosCanjeados(string? estadoCanje, int minMeses);

        public Task<List<Top3Productos>> GetTop3Productos(int? año);

        public Task<List<ReporteReservaTT>> GetReporteReserva(DateTime desde, DateTime hasta, string disciplina1, string disciplina2);

        public Task<List<EvaluacionEstadistica>> GetEvaluacionesEstadistica(DateTime desde, DateTime hasta);

        public Task<List<ControlAptoMedico>> GetControlAptosMedicos(string? estados);

        public Task<List<RentabilidadConvenio>> GetRentabilidadConvenios(string? Estado);

        public Task<List<SocioDto>> GetSociosAsync();

        public Task<SocioDetalleDto?> GetSocioByIdAsync(int idSocio);

        public Task<SocioDto> CreateSocioAsync(SocioCreateDto dto);

        public Task<SocioContratoResponseDto> CreateSocioConContratoAsync(SocioConContratoCreateDto dto);

        public Task<SocioDetalleDto?> UpdateSocioAsync(int idSocio, SocioCreateDto dto);

        public Task<bool> DeleteSocioAsync(int idSocio);

        public Task<List<PersonaDto>> GetPersonasAsync();

        public Task<PersonaDto?> GetPersonaByIdAsync(int idPersona);

        public Task<PersonaDto> CreatePersonaAsync(PersonaCreateDto dto);

        public Task<List<SedeDto>> GetSedesAsync();

        public Task<SedeDto?> GetSedeByIdAsync(int idSede);

        public Task<SedeDto> CreateSedeAsync(SedeCreateDto dto);

        public Task<List<RoleDto>> GetRolesAsync();

        public Task<RoleDto?> GetRoleByIdAsync(int idRol);

        public Task<RoleDto> CreateRoleAsync(RoleCreateDto dto);

        public Task<List<CategoriaProductoDto>> GetCategoriasProductosAsync();

        public Task<CategoriaProductoDto?> GetCategoriaProductoByIdAsync(int idCategoria);

        public Task<CategoriaProductoDto> CreateCategoriaProductoAsync(CategoriaProductoCreateDto dto);

        public Task<List<FormaPagoDto>> GetFormasPagoAsync();

        public Task<FormaPagoDto?> GetFormaPagoByIdAsync(int idFormaPago);

        public Task<FormaPagoDto> CreateFormaPagoAsync(FormaPagoCreateDto dto);

        public Task<List<EstadoReclamoDto>> GetEstadosReclamoAsync();

        public Task<EstadoReclamoDto?> GetEstadoReclamoByIdAsync(int idEstadoReclamo);

        public Task<EstadoReclamoDto> CreateEstadoReclamoAsync(EstadoReclamoCreateDto dto);

        public Task<List<ProvinciaDto>> GetProvinciasAsync();

        public Task<ProvinciaDto?> GetProvinciaByIdAsync(int idProvincia);

        public Task<ProvinciaDto> CreateProvinciaAsync(ProvinciaCreateDto dto);

        public Task<List<LocalidadDto>> GetLocalidadesAsync(int? idProvincia = null);

        public Task<LocalidadDto?> GetLocalidadByIdAsync(int idLocalidad);

        public Task<LocalidadDto> CreateLocalidadAsync(LocalidadCreateDto dto);

        public Task<List<BarrioDto>> GetBarriosAsync(int? idLocalidad = null);

        public Task<BarrioDto?> GetBarrioByIdAsync(int idBarrio);

        public Task<BarrioDto> CreateBarrioAsync(BarrioCreateDto dto);

        public Task<List<CalleDto>> GetCallesAsync(int? idBarrio = null);

        public Task<CalleDto?> GetCalleByIdAsync(int idCalle);

        public Task<CalleDto> CreateCalleAsync(CalleCreateDto dto);

        public Task<List<PlanDto>> GetPlanesAsync();

        public Task<List<CuotaCobroDto>> GetCuotasPendientesAsync(int? idSocio = null);

        public Task<List<PagoCuotaHistoricoDto>> GetPagosCuotaAsync(int? idCobro = null, int? idSocio = null);

        public Task<PagoCuotaResponseDto> RegistrarPagoCuotaAsync(PagoCuotaCreateDto dto);

        public Task<List<AptoMedicoDto>> GetAptosMedicosAsync();

        public Task<AptoMedicoDto> CreateAptoMedicoAsync(AptoMedicoCreateDto dto);

        public Task<AccesoValidacionResponseDto> ValidarAccesoAsync(int idSocio, int idSede, DateTime fechaHora);

        public Task<HistorialSocioDto> GetHistorialSocioAsync(int idSocio);

        public Task<List<ReservaDto>> GetReservasEnListaEsperaAsync(int idClase);

        public Task<ReservaDto?> PromoverSiguienteListaEsperaAsync(int idClase);

        public Task<List<ClaseDto>> GetClasesAsync();

        public Task<List<ReservaDto>> GetReservasAsync(int? idSocio = null);

        public Task<ReservaDto?> GetReservaByIdAsync(int idReserva);

        public Task<ReservaDto> CreateReservaAsync(ReservaCreateDto dto);

        public Task<ReservaDto?> UpdateReservaAsync(int idReserva, ReservaCreateDto dto);

        public Task<bool> DeleteReservaAsync(int idReserva);

        public Task<List<ProductoDto>> GetProductosAsync(int? idSede = null);

        public Task<ProductoDto?> GetProductoByIdAsync(int idProducto);

        public Task<ProductoDto> CreateProductoAsync(ProductoCreateDto dto);

        public Task<ProductoDto?> UpdateProductoAsync(int idProducto, ProductoCreateDto dto);

        public Task<bool> DeleteProductoAsync(int idProducto);

        public Task<List<StockSedeDto>> GetStockPorSedeAsync(int? idSede = null);

        public Task<VentaDto> RegistrarVentaAsync(VentaCreateDto dto);

        public Task<List<EquipoDto>> GetEquiposAsync(int? idSede = null);

        public Task<List<IncidenciaDto>> GetIncidenciasAsync(int? idSede = null);

        public Task<IncidenciaDto> CreateIncidenciaAsync(IncidenciaCreateDto dto);

        public Task<List<ContratoMembresiaDto>> GetContratosAsync(int? idSocio = null);

        public Task<ContratoMembresiaDto?> GetContratoByIdAsync(int idContrato);

        public Task<ContratoMembresiaDto> CreateContratoAsync(ContratoMembresiaCreateDto dto);

        public Task<ContratoMembresiaDto> RenovarContratoAsync(int idContrato, ContratoMembresiaCreateDto dto, int idFormaPago = 1);

        public Task<ContratoMembresiaDto?> UpdateContratoAsync(int idContrato, ContratoMembresiaCreateDto dto);

        public Task<bool> DeleteContratoAsync(int idContrato);

        public Task<List<ConvenioEmpresaDto>> GetConveniosAsync();

        public Task<ConvenioEmpresaDto?> GetConvenioByIdAsync(int idConvenio);

        public Task<ConvenioEmpresaDto> CreateConvenioAsync(ConvenioEmpresaCreateDto dto);

        public Task<ConvenioEmpresaDto?> UpdateConvenioAsync(int idConvenio, ConvenioEmpresaCreateDto dto);

        public Task<bool> DeleteConvenioAsync(int idConvenio);

        public Task<List<ProveedorDto>> GetProveedoresAsync();

        public Task<ProveedorDto> CreateProveedorAsync(ProveedorCreateDto dto);

        public Task<List<CompraStockDto>> GetComprasAsync(int? idSede = null);

        public Task<CompraStockDto> CreateCompraAsync(CompraStockCreateDto dto);

        public Task<List<CajaDto>> GetCajasAsync(int? idSede = null);

        public Task<CajaDto> CreateCajaAsync(CajaCreateDto dto);

        public Task<List<TransferenciaStockDto>> GetTransferenciasAsync(int? idSede = null);

        public Task<TransferenciaStockDto?> GetTransferenciaByIdAsync(int idTransferencia);

        public Task<TransferenciaStockDto> CreateTransferenciaAsync(TransferenciaStockCreateDto dto);

        public Task<TransferenciaStockDto?> UpdateTransferenciaAsync(int idTransferencia, TransferenciaStockCreateDto dto);

        public Task<bool> DeleteTransferenciaAsync(int idTransferencia);

        public Task<List<DevolucionVentaDto>> GetDevolucionesAsync(int? idVenta = null);

        public Task<DevolucionVentaDto> CreateDevolucionAsync(DevolucionVentaCreateDto dto);

        public Task<List<PromocionDto>> GetPromocionesAsync();

        public Task<PromocionDto> CreatePromocionAsync(PromocionCreateDto dto);

        public Task<List<MovimientoStockDto>> GetMovimientosStockAsync(int? idSede = null, int? idProducto = null);

        public Task<MovimientoStockDto> CreateMovimientoStockAsync(MovimientoStockCreateDto dto);

        public Task<List<CierreCajaDto>> GetCierresCajaAsync(int? idCaja = null);

        public Task<CierreCajaDto?> GetCierreCajaByIdAsync(int idCierre);

        public Task<CierreCajaDto> CreateCierreCajaAsync(CierreCajaCreateDto dto);

        public Task<CierreCajaDto?> UpdateCierreCajaAsync(int idCierre, CierreCajaCreateDto dto);

        public Task<bool> DeleteCierreCajaAsync(int idCierre);

        public Task<List<EmpleadoDto>> GetEmpleadosAsync(int? idSede = null, int? idRol = null);

        public Task<EmpleadoDto> CreateEmpleadoAsync(EmpleadoCreateDto dto);

        public Task<List<RutinaDto>> GetRutinasAsync(int? idSocio = null, int? idEntrenador = null);

        public Task<RutinaDto> CreateRutinaAsync(RutinaCreateDto dto);

        public Task<List<TurnoDto>> GetTurnosAsync(int? idSocio = null, int? idProfesional = null);

        public Task<TurnoDto?> GetTurnoByIdAsync(int idTurno);

        public Task<TurnoDto> CreateTurnoAsync(TurnoCreateDto dto);

        public Task<TurnoDto?> UpdateTurnoAsync(int idTurno, TurnoCreateDto dto);

        public Task<bool> DeleteTurnoAsync(int idTurno);

        public Task<List<MantenimientoEquipamientoDto>> GetMantenimientosAsync(int? idEquipo = null);

        public Task<MantenimientoEquipamientoDto?> GetMantenimientoByIdAsync(int idMantenimiento);

        public Task<MantenimientoEquipamientoDto> CreateMantenimientoAsync(MantenimientoEquipamientoCreateDto dto);

        public Task<MantenimientoEquipamientoDto?> UpdateMantenimientoAsync(int idMantenimiento, MantenimientoEquipamientoCreateDto dto);

        public Task<bool> DeleteMantenimientoAsync(int idMantenimiento);

        public Task<List<EstadoCuentaSocioDto>> GetEstadoCuentaSociosAsync();

        public Task<List<MembresiaVigenteDto>> GetMembresiasVigentesAsync();

        public Task<List<OcupacionClaseDto>> GetOcupacionClasesAsync();

        public Task<List<CostoMantenimientoEquipoDto>> GetCostosMantenimientoAsync();
    }
}
