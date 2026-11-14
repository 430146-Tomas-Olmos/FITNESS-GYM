using System.ComponentModel.DataAnnotations;

namespace FITLIFENETWORKPROYECTO.DTO
{
    public class SocioCreateDto
    {
        [Required]
        public string Dni { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        public DateOnly FechaNacimiento { get; set; }

        public string EstadoCivil { get; set; } = "SOLTERO";

        public string Ocupacion { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        [Required]
        public int IdCalle { get; set; }

        public int? IdConvenio { get; set; }

        public DateOnly FechaIngresoAlta { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public string EstadoCuenta { get; set; } = "ACTIVO";
    }

    public class SocioDto
    {
        public int IdSocio { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string EstadoCuenta { get; set; } = string.Empty;
        public DateOnly FechaIngresoAlta { get; set; }
        public int? IdSede { get; set; }
        public string? NombreSede { get; set; }
        public string? Plan { get; set; }
    }

    public class SocioConContratoCreateDto
    {
        [Required]
        public SocioCreateDto Socio { get; set; } = new();

        [Required]
        public ContratoMembresiaCreateDto Contrato { get; set; } = new();

        public int IdFormaPago { get; set; } = 1;
    }

    public class SocioContratoResponseDto
    {
        public int IdSocio { get; set; }
        public int IdContrato { get; set; }
        public int CuotasGeneradas { get; set; }
    }

    public class SocioDetalleDto
    {
        public int IdSocio { get; set; }
        public int IdPersona { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Ocupacion { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public DateOnly FechaIngresoAlta { get; set; }
        public string EstadoCuenta { get; set; } = string.Empty;
        public int IdCalle { get; set; }
        public int? IdConvenio { get; set; }
    }

    public class PersonaCreateDto
    {
        [Required]
        public string Dni { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        public DateOnly FechaNacimiento { get; set; }

        public string EstadoCivil { get; set; } = "SOLTERO";

        public string Ocupacion { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string? Email { get; set; }
    }

    public class PersonaDto
    {
        public int IdPersona { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateOnly FechaNacimiento { get; set; }
        public string EstadoCivil { get; set; } = string.Empty;
        public string Ocupacion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }

    public class SedeCreateDto
    {
        [Required]
        public int IdCalle { get; set; }

        [Required]
        public string NombreSede { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string? Email { get; set; }
    }

    public class SedeDto
    {
        public int IdSede { get; set; }
        public int IdCalle { get; set; }
        public string NombreCalle { get; set; } = string.Empty;
        public string NombreSede { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }

    public class RoleCreateDto
    {
        [Required]
        public string NombreRol { get; set; } = string.Empty;
    }

    public class RoleDto
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
    }

    public class CategoriaProductoCreateDto
    {
        [Required]
        public string NombreCategoria { get; set; } = string.Empty;
    }

    public class CategoriaProductoDto
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
    }

    public class FormaPagoCreateDto
    {
        [Required]
        public string NombreFormaPago { get; set; } = string.Empty;
    }

    public class FormaPagoDto
    {
        public int IdFormaPago { get; set; }
        public string NombreFormaPago { get; set; } = string.Empty;
    }

    public class EstadoReclamoCreateDto
    {
        [Required]
        public string NombreEstado { get; set; } = string.Empty;
    }

    public class EstadoReclamoDto
    {
        public int IdEstadoReclamo { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
    }

    public class ProvinciaCreateDto
    {
        [Required]
        public string NombreProvincia { get; set; } = string.Empty;
    }

    public class ProvinciaDto
    {
        public int IdProvincia { get; set; }
        public string NombreProvincia { get; set; } = string.Empty;
    }

    public class LocalidadCreateDto
    {
        [Required]
        public int IdProvincia { get; set; }

        [Required]
        public string NombreLocalidad { get; set; } = string.Empty;
    }

    public class LocalidadDto
    {
        public int IdLocalidad { get; set; }
        public int IdProvincia { get; set; }
        public string NombreProvincia { get; set; } = string.Empty;
        public string NombreLocalidad { get; set; } = string.Empty;
    }

    public class BarrioCreateDto
    {
        [Required]
        public int IdLocalidad { get; set; }

        [Required]
        public string NombreBarrio { get; set; } = string.Empty;
    }

    public class BarrioDto
    {
        public int IdBarrio { get; set; }
        public int IdLocalidad { get; set; }
        public string NombreLocalidad { get; set; } = string.Empty;
        public string NombreBarrio { get; set; } = string.Empty;
    }

    public class CalleCreateDto
    {
        [Required]
        public int IdBarrio { get; set; }

        [Required]
        public string NombreCalle { get; set; } = string.Empty;

        public int Numeracion { get; set; }
    }

    public class CalleDto
    {
        public int IdCalle { get; set; }
        public int IdBarrio { get; set; }
        public string NombreBarrio { get; set; } = string.Empty;
        public string NombreCalle { get; set; } = string.Empty;
        public int Numeracion { get; set; }
    }

    public class PlanDto
    {
        public int IdPlan { get; set; }
        public string NombrePlan { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioActual { get; set; }
        public int DuracionMeses { get; set; }
        public bool PermiteMultisede { get; set; }
    }

    public class CuotaCobroDto
    {
        public int IdCobro { get; set; }
        public int IdContrato { get; set; }
        public int NumeroCuota { get; set; }
        public decimal Monto { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string SocioNombre { get; set; } = string.Empty;
        public int IdFormaPago { get; set; }
    }


    public class PagoCuotaResponseDto
    {
        public int IdPago { get; set; }
        public int IdCobro { get; set; }
        public decimal Importe { get; set; }
        public DateTime? FechaPago { get; set; }
        public string? UsuarioSistema { get; set; }
        public decimal CuotaPendiente { get; set; }
    }

    public class PagoCuotaHistoricoDto
    {
        public int IdPago { get; set; }
        public int IdCobro { get; set; }
        public int NumeroCuota { get; set; }
        public int IdSocio { get; set; }
        public string NombreSocio { get; set; } = string.Empty;
        public int IdFormaPago { get; set; }
        public string NombreFormaPago { get; set; } = string.Empty;
        public decimal Importe { get; set; }
        public DateTime FechaPago { get; set; }
        public string? UsuarioSistema { get; set; }
    }

    public class AptoMedicoCreateDto
    {
        [Required]
        public int IdSocio { get; set; }

        [Required]
        public DateOnly FechaEmision { get; set; }

        [Required]
        public DateOnly FechaVencimiento { get; set; }

        [Required]
        public string MedicoFirmante { get; set; } = string.Empty;

        [Required]
        public string MatriculaMedico { get; set; } = string.Empty;

        public string Observaciones { get; set; } = string.Empty;
    }

    public class AptoMedicoDto
    {
        public int IdApto { get; set; }
        public int IdSocio { get; set; }
        public DateOnly FechaEmision { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        public string MedicoFirmante { get; set; } = string.Empty;
        public string MatriculaMedico { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
    }

    public class ClaseDto
    {
        public int IdClase { get; set; }
        public int IdDisciplina { get; set; }
        public string? NombreDisciplina { get; set; }
        public int IdInstructor { get; set; }
        public string? NombreInstructor { get; set; }
        public int IdSalon { get; set; }
        public DateOnly FechaClase { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public int CupoLimite { get; set; }
    }

    public class ReservaCreateDto
    {
        [Required]
        public int IdSocio { get; set; }

        [Required]
        public int IdClase { get; set; }

        public string EstadoReserva { get; set; } = "CONFIRMADA";
    }

    public class ReservaDto
    {
        public int IdReserva { get; set; }
        public int IdSocio { get; set; }
        public int IdClase { get; set; }
        public DateTime FechaReserva { get; set; }
        public string EstadoReserva { get; set; } = string.Empty;
    }

    public class AccesoValidacionRequestDto
    {
        [Required]
        public int IdSocio { get; set; }

        [Required]
        public int IdSede { get; set; }

        public DateTime FechaHoraMarcado { get; set; } = DateTime.Now;
    }

    public class AccesoLogDto
    {
        public int IdAcceso { get; set; }
        public int IdSocio { get; set; }
        public string SocioNombre { get; set; } = string.Empty;
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public DateTime FechaHoraMarcado { get; set; }
        public bool AccesoPermitido { get; set; }
        public string? MotivoRechazo { get; set; }
    }

    public class AccesoValidacionResponseDto
    {
        public int IdSocio { get; set; }
        public int IdSede { get; set; }
        public bool Permitido { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string EstadoSocio { get; set; } = string.Empty;
        public string? NombreSocio { get; set; }
        public DateTime FechaHora { get; set; }
        public string? ContratoVigente { get; set; }
        public string? AptoMedico { get; set; }
    }

    public class HistorialSocioDto
    {
        public int IdSocio { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string EstadoCuenta { get; set; } = string.Empty;
        public List<ContratoMembresiaDto> Contratos { get; set; } = new();
        public List<CuotaCobroDto> Cuotas { get; set; } = new();
        public List<PagoCuotaHistoricoDto> Pagos { get; set; } = new();
        public List<AccesoLogDto> Accesos { get; set; } = new();
        public List<ReservaDto> Reservas { get; set; } = new();
        public List<AptoMedicoDto> AptosMedicos { get; set; } = new();
        public List<string> Sanciones { get; set; } = new();
    }

    public class ProductoCreateDto
    {
        [Required]
        public int IdCategoria { get; set; }

        [Required]
        public string NombreProducto { get; set; } = string.Empty;

        [Required]
        public string CodigoBarras { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal PrecioActual { get; set; }

        [Range(0, int.MaxValue)]
        public int StockMinimo { get; set; }
    }

    public class ProductoDto
    {
        public int IdProducto { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public string CodigoBarras { get; set; } = string.Empty;
        public decimal PrecioActual { get; set; }
        public int StockMinimo { get; set; }
    }

    public class StockSedeDto
    {
        public int IdStockSede { get; set; }
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int CantidadActual { get; set; }
    }

    public class VentaDetalleCreateDto
    {
        [Required]
        public int IdProducto { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; } = 1;

        public decimal PrecioUnitarioHistorico { get; set; }
    }

    public class VentaCreateDto
    {
        public int? IdSocio { get; set; }

        [Required]
        public int IdSede { get; set; }

        [Required]
        public int IdVendedor { get; set; }

        [Required]
        public int IdFormaPago { get; set; }

        public bool ImputadoACuenta { get; set; }

        [MinLength(1)]
        public List<VentaDetalleCreateDto> Items { get; set; } = new();
    }

    public class VentaDetalleDto
    {
        public int IdDetalleVenta { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitarioHistorico { get; set; }
    }

    public class VentaDto
    {
        public int IdVenta { get; set; }
        public int? IdSocio { get; set; }
        public int IdSede { get; set; }
        public int IdVendedor { get; set; }
        public int IdFormaPago { get; set; }
        public DateTime FechaHoraVenta { get; set; }
        public bool ImputadoACuenta { get; set; }
        public decimal TotalVenta { get; set; }
        public List<VentaDetalleDto> Detalles { get; set; } = new();
    }

    public class EquipoDto
    {
        public int IdEquipo { get; set; }
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public string NombreMaquina { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;
        public DateOnly FechaAdquisicion { get; set; }
        public string EstadoOperativo { get; set; } = string.Empty;
    }



    public class IncidenciaCreateDto
    {
        public int? IdSocio { get; set; }

        public int? IdEquipo { get; set; }

        [Required]
        public int IdSede { get; set; }

        public int? IdTecnicoAsignado { get; set; }

        [Required]
        public int IdEstadoReclamo { get; set; }

        [Required]
        public string Descripcion { get; set; } = string.Empty;
    }

    public class IncidenciaDto
    {
        public int IdReclamo { get; set; }
        public int? IdSocio { get; set; }
        public int? IdEquipo { get; set; }
        public int IdSede { get; set; }
        public int? IdTecnicoAsignado { get; set; }
        public int IdEstadoReclamo { get; set; }
        public string EstadoReclamo { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class ContratoMembresiaCreateDto
    {
        [Required]
        public int IdSocio { get; set; }

        [Required]
        public int IdPlan { get; set; }

        [Required]
        public int IdSede { get; set; }

        [Required]
        public DateOnly FechaInicio { get; set; }

        [Required]
        public DateOnly FechaVencimiento { get; set; }

        public string EstadoContrato { get; set; } = "ACTIVO";
    }

    public class ContratoMembresiaDto
    {
        public int IdContrato { get; set; }
        public int IdSocio { get; set; }
        public string SocioNombre { get; set; } = string.Empty;
        public int IdPlan { get; set; }
        public string NombrePlan { get; set; } = string.Empty;
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        public string EstadoContrato { get; set; } = string.Empty;
    }

    public class ConvenioEmpresaCreateDto
    {
        [Required]
        public string RazonSocial { get; set; } = string.Empty;

        [Required]
        public string Cuit { get; set; } = string.Empty;

        [Range(0, 100)]
        public decimal PorcentajeDescuento { get; set; }

        public int DiaCierreFacturacion { get; set; } = 30;

        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }

    public class ConvenioEmpresaDto
    {
        public int IdConvenio { get; set; }
        public string RazonSocial { get; set; } = string.Empty;
        public string Cuit { get; set; } = string.Empty;
        public decimal PorcentajeDescuento { get; set; }
        public int DiaCierreFacturacion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }

    public class ProveedorCreateDto
    {
        [Required]
        public string RazonSocial { get; set; } = string.Empty;

        [Required]
        public string Cuit { get; set; } = string.Empty;

        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }

    public class ProveedorDto
    {
        public int IdProveedor { get; set; }
        public string RazonSocial { get; set; } = string.Empty;
        public string Cuit { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }

    public class CompraDetalleStockCreateDto
    {
        [Required]
        public int IdProducto { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; } = 1;

        [Range(0.01, double.MaxValue)]
        public decimal PrecioCostoUnitario { get; set; }
    }

    public class CompraStockCreateDto
    {
        [Required]
        public int IdProveedor { get; set; }

        [Required]
        public int IdSede { get; set; }

        [Required]
        public DateTime FechaCompra { get; set; } = DateTime.Now;

        [MinLength(1)]
        public List<CompraDetalleStockCreateDto> Detalles { get; set; } = new();
    }

    public class CompraDetalleStockDto
    {
        public int IdDetalleCompra { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioCostoUnitario { get; set; }
    }

    public class CompraStockDto
    {
        public int IdCompra { get; set; }
        public int IdProveedor { get; set; }
        public string RazonSocialProveedor { get; set; } = string.Empty;
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public DateTime FechaCompra { get; set; }
        public decimal TotalCompra { get; set; }
        public List<CompraDetalleStockDto> Detalles { get; set; } = new();
    }

    public class PromocionCreateDto
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Tipo { get; set; } = "PRODUCTO";

        public int? IdProducto { get; set; }

        public int? IdCategoria { get; set; }

        [Range(0.01, 100)]
        public decimal DescuentoPorcentaje { get; set; }

        public decimal MontoFijo { get; set; }

        public DateOnly? FechaDesde { get; set; }

        public DateOnly? FechaHasta { get; set; }
    }

    public class PromocionDto
    {
        public int IdPromocion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int? IdProducto { get; set; }
        public int? IdCategoria { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        public decimal MontoFijo { get; set; }
        public DateOnly? FechaDesde { get; set; }
        public DateOnly? FechaHasta { get; set; }
        public bool Activa { get; set; }
    }

    public class MovimientoStockCreateDto
    {
        [Required]
        public int IdSede { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public string TipoMovimiento { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; } = 1;

        public int? IdVenta { get; set; }

        public string? UsuarioSistema { get; set; }

        public string? Observaciones { get; set; }
    }

    public class MovimientoStockDto
    {
        public int IdMovimiento { get; set; }
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public int? IdVenta { get; set; }
        public string? UsuarioSistema { get; set; }
        public string? Observaciones { get; set; }
    }

    public class CajaCreateDto
    {
        [Required]
        public int IdSede { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SaldoInicial { get; set; }
    }

    public class CajaDto
    {
        public int IdCaja { get; set; }
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public DateTime FechaApertura { get; set; }
        public decimal SaldoInicial { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class DetalleTransferenciaCreateDto
    {
        [Required]
        public int IdProducto { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; } = 1;
    }

    public class TransferenciaStockCreateDto
    {
        [Required]
        public int IdSedeOrigen { get; set; }

        [Required]
        public int IdSedeDestino { get; set; }

        public string? UsuarioSistema { get; set; }

        [MinLength(1)]
        public List<DetalleTransferenciaCreateDto> Detalles { get; set; } = new();
    }

    public class DetalleTransferenciaDto
    {
        public int IdDetalleTransferencia { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class TransferenciaStockDto
    {
        public int IdTransferencia { get; set; }
        public int IdSedeOrigen { get; set; }
        public string NombreSedeOrigen { get; set; } = string.Empty;
        public int IdSedeDestino { get; set; }
        public string NombreSedeDestino { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? UsuarioSistema { get; set; }
        public List<DetalleTransferenciaDto> Detalles { get; set; } = new();
    }

    public class CierreCajaCreateDto
    {
        [Required]
        public int IdCaja { get; set; }

        [Required]
        public DateTime FechaCierre { get; set; } = DateTime.Now;

        [Range(0, double.MaxValue)]
        public decimal SaldoReal { get; set; }

        public string? Observaciones { get; set; }
    }

    public class CierreCajaDto
    {
        public int IdCierre { get; set; }
        public int IdCaja { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public DateTime FechaCierre { get; set; }
        public decimal TotalVentas { get; set; }
        public decimal SaldoEsperado { get; set; }
        public decimal SaldoReal { get; set; }
        public decimal Diferencia { get; set; }
        public string? Observaciones { get; set; }
    }

    public class DetalleDevolucionVentaCreateDto
    {
        [Required]
        public int IdProducto { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; } = 1;

        [Range(0.01, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }
    }

    public class DevolucionVentaCreateDto
    {
        [Required]
        public int IdVenta { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [Required]
        public string Motivo { get; set; } = string.Empty;

        [MinLength(1)]
        public List<DetalleDevolucionVentaCreateDto> Detalles { get; set; } = new();
    }

    public class DetalleDevolucionVentaDto
    {
        public int IdDetalleDevolucion { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class DevolucionVentaDto
    {
        public int IdDevolucion { get; set; }
        public int IdVenta { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public decimal ImporteReintegrado { get; set; }
        public List<DetalleDevolucionVentaDto> Detalles { get; set; } = new();
    }

    public class EmpleadoCreateDto
    {
        [Required]
        public string Dni { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        public DateOnly FechaNacimiento { get; set; }
        public string EstadoCivil { get; set; } = "SOLTERO";
        public string Ocupacion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }

        [Required]
        public int IdSede { get; set; }

        [Required]
        public int IdRol { get; set; }

        [Required]
        public string Legajo { get; set; } = string.Empty;

        public DateOnly FechaIngreso { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Range(0.01, double.MaxValue)]
        public decimal SueldoBase { get; set; }
    }

    public class EmpleadoDto
    {
        public int IdEmpleado { get; set; }
        public int IdPersona { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public string Legajo { get; set; } = string.Empty;
        public DateOnly FechaIngreso { get; set; }
        public decimal SueldoBase { get; set; }
    }

    public class DetalleRutinaCreateDto
    {
        [Required]
        public int IdEjercicio { get; set; }

        [Range(1, int.MaxValue)]
        public int Series { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int Repeticiones { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int DescansoSegundos { get; set; }
    }

    public class RutinaCreateDto
    {
        [Required]
        public int IdSocio { get; set; }

        [Required]
        public int IdEntrenador { get; set; }

        public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        public string Objetivo { get; set; } = string.Empty;

        [MinLength(1)]
        public List<DetalleRutinaCreateDto> Detalles { get; set; } = new();
    }

    public class DetalleRutinaDto
    {
        public int IdDetalleRutina { get; set; }
        public int IdEjercicio { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public int DescansoSegundos { get; set; }
    }

    public class RutinaDto
    {
        public int IdRutina { get; set; }
        public int IdSocio { get; set; }
        public string NombreSocio { get; set; } = string.Empty;
        public int IdEntrenador { get; set; }
        public string NombreEntrenador { get; set; } = string.Empty;
        public DateOnly FechaCreacion { get; set; }
        public string Objetivo { get; set; } = string.Empty;
        public List<DetalleRutinaDto> Detalles { get; set; } = new();
    }

    public class TurnoCreateDto
    {
        [Required]
        public int IdSocio { get; set; }

        [Required]
        public int IdProfesional { get; set; }

        [Required]
        public int IdSede { get; set; }

        [Required]
        public int IdEstadoTurno { get; set; }

        public DateOnly FechaTurno { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public TimeOnly HoraTurno { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
    }

    public class TurnoDto
    {
        public int IdTurno { get; set; }
        public int IdSocio { get; set; }
        public string NombreSocio { get; set; } = string.Empty;
        public int IdProfesional { get; set; }
        public string NombreProfesional { get; set; } = string.Empty;
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public int IdEstadoTurno { get; set; }
        public string EstadoTurno { get; set; } = string.Empty;
        public DateOnly FechaTurno { get; set; }
        public TimeOnly HoraTurno { get; set; }
    }

    public class MantenimientoEquipamientoCreateDto
    {
        [Required]
        public int IdEquipo { get; set; }

        public int? IdProveedor { get; set; }

        public DateOnly FechaMantenimiento { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Costo { get; set; }
    }

    public class MantenimientoEquipamientoDto
    {
        public int IdMantenimiento { get; set; }
        public int IdEquipo { get; set; }
        public string NombreEquipo { get; set; } = string.Empty;
        public int? IdProveedor { get; set; }
        public string NombreProveedor { get; set; } = string.Empty;
        public DateOnly FechaMantenimiento { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Costo { get; set; }
    }

    public class EstadoCuentaSocioDto
    {
        public int IdSocio { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int CuotasEmitidas { get; set; }
        public decimal ImporteEmitido { get; set; }
        public decimal ImportePagado { get; set; }
        public decimal SaldoPendiente { get; set; }
    }

    public class MembresiaVigenteDto
    {
        public int IdContrato { get; set; }
        public int IdSocio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int IdPlan { get; set; }
        public string NombrePlan { get; set; } = string.Empty;
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        public string EstadoContrato { get; set; } = string.Empty;
    }

    public class OcupacionClaseDto
    {
        public int IdClase { get; set; }
        public DateOnly FechaClase { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public string NombreDisciplina { get; set; } = string.Empty;
        public int CupoLimite { get; set; }
        public int CuposOcupados { get; set; }
        public int CuposDisponibles { get; set; }
    }

    public class CostoMantenimientoEquipoDto
    {
        public int IdEquipo { get; set; }
        public string NombreMaquina { get; set; } = string.Empty;
        public int IdSede { get; set; }
        public string NombreSede { get; set; } = string.Empty;
        public int CantidadIntervenciones { get; set; }
        public decimal CostoTotal { get; set; }
        public DateOnly? UltimoMantenimiento { get; set; }
    }
}
