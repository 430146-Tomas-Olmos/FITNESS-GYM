using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Models.Context;
using FITLIFENETWORKPROYECTO.Models.Tablas;
using FITLIFENETWORKPROYECTO.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FITLIFENETWORKPROYECTO.Repositories
{
    public class RepositoryFitLife : IRepository
    {
        private readonly FITLIFENETWORKDbContext db;

        public RepositoryFitLife(FITLIFENETWORKDbContext db)
        {
            this.db = db;
        }

        public async Task<List<ReporteRecaudacionSede>> GetRecaudacionSede(int? año, int? mes)
        {
            string query = @$"
                             DECLARE @Año INT = {año};
                             DECLARE @Mes INT = {mes};

                             WITH RecaudacionSede AS (
                                                      SELECT
                                                             S.id_sede,
                                                             SUM(CC.monto) AS total_general_sede
                                                      FROM CUOTAS_COBROS CC
                                                      INNER JOIN CONTRATOS_MEMBRESIA CM ON CC.id_contrato = CM.id_contrato
                                                      INNER JOIN SEDES S ON CM.id_sede = S.id_sede
                                                      WHERE CC.fecha_pago IS NOT NULL
                                                        AND YEAR(CC.fecha_pago) = @Año
                                                        AND MONTH(CC.fecha_pago) = @Mes
                                                      GROUP BY S.id_sede
                                                                                        )
                                                      SELECT
                                                            S.nombre_sede AS Sede,
                                                            P.nombre_plan AS PlanMembresia,
                                                            COUNT(CC.id_cobro) AS CuotasCobradas,
                                                            SUM(CC.monto) AS TotalRecaudado,
                                                            AVG(CC.monto) AS MontoPromedio,
                                                            CAST(ROUND((SUM(CC.monto) * 100.0) / NULLIF(RS.total_general_sede, 0), 2) AS DECIMAL(18,2)) AS ParticipacionSedePorcentaje
                                                      FROM CUOTAS_COBROS CC
                                                      INNER JOIN CONTRATOS_MEMBRESIA CM ON CC.id_contrato = CM.id_contrato
                                                      INNER JOIN SEDES S ON CM.id_sede = S.id_sede
                                                      INNER JOIN PLANES P ON CM.id_plan = P.id_plan
                                                      INNER JOIN RecaudacionSede RS ON S.id_sede = RS.id_sede
                                                      WHERE CC.fecha_pago IS NOT NULL
                                                        AND YEAR(CC.fecha_pago) = @Año
                                                        AND MONTH(CC.fecha_pago) = @Mes 
                                                      GROUP BY
                                                            S.id_sede,
                                                            S.nombre_sede,
                                                            P.nombre_plan,
                                                            RS.total_general_sede
                                                      ORDER BY
                                                            S.nombre_sede ASC,
                                                            TotalRecaudado DESC;";
            return await db.ReporteRecaudacionSedes.FromSqlRaw(query, año, mes).ToListAsync();
        }

        public async Task<List<AuditoriaAptoMedico>> GetAccesoDenegadoApto(DateTime? desde, DateTime? hasta)
        {
            // Rango de fechas ingresado por el usuario (por defecto: último año). El día "hasta" se incluye completo.
            var fechaDesde = (desde ?? DateTime.Today.AddYears(-1)).Date;
            var fechaHasta = (hasta ?? DateTime.Today).Date.AddDays(1);

            return await db.AuditoriaAptoMedicos.FromSql($@"
                            SELECT
                                  S.id_socio AS IdSocio,
                                  PER.dni AS Dni,
                                  CONCAT(PER.apellido, ', ', PER.nombre) AS Socio,
                                  SED.nombre_sede AS SedeDelIntento,
                                  COUNT(AL.id_acceso) AS IntentosDenegados,
                                  MAX(AL.fecha_hora_marcado) AS UltimoIntentoDenegado,
                                  AM.fecha_vencimiento AS VencimientoAptoMedico,
                                  AM.medico_firmante AS MedicoRegistrado
                            FROM ACCESOS_LOG AL
                            INNER JOIN SOCIOS S ON AL.id_socio = S.id_socio
                            INNER JOIN PERSONAS PER ON S.id_persona = PER.id_persona
                            INNER JOIN SEDES SED ON AL.id_sede = SED.id_sede
                            LEFT JOIN APTOS_MEDICOS AM ON S.id_socio = AM.id_socio
                            WHERE AL.acceso_permitido = 0
                              AND (AL.motivo_rechazo LIKE '%apto%' OR AL.motivo_rechazo LIKE '%vencid%')
                              AND AL.fecha_hora_marcado >= {fechaDesde}
                              AND AL.fecha_hora_marcado < {fechaHasta}
                            GROUP BY
                                S.id_socio,
                                PER.dni,
                                PER.apellido,
                                PER.nombre,
                                SED.nombre_sede,
                                AM.fecha_vencimiento,
                                AM.medico_firmante
                            HAVING COUNT(AL.id_acceso) >= 1
                            ORDER BY IntentosDenegados DESC, UltimoIntentoDenegado DESC").ToListAsync();
        }

        public async Task<List<LogAccesosDenegado>> GetAuditoriaAccesosDenegadosAsync()
        {
            return await db.LogAccesosDenegados
                .AsNoTracking()
                .Include(l => l.IdSocioNavigation)
                    .ThenInclude(s => s.IdPersonaNavigation)
                .Include(l => l.IdSedeNavigation)
                .OrderByDescending(l => l.FechaHora)
                .ToListAsync();
        }

        public async Task<List<LogCambiosEstadoSocio>> GetAuditoriaCambiosEstadoSocioAsync()
        {
            return await db.LogCambiosEstadoSocios
                .AsNoTracking()
                .Include(l => l.IdSocioNavigation)
                    .ThenInclude(s => s.IdPersonaNavigation)
                .OrderByDescending(l => l.FechaCambio)
                .ToListAsync();
        }

        public async Task<List<AuditoriaEquipamiento>> GetAuditoriaEquipamientoAsync()
        {
            return await db.AuditoriaEquipamientos
                .AsNoTracking()
                .Include(a => a.IdEquipoNavigation)
                .OrderByDescending(a => a.FechaCambio)
                .ToListAsync();
        }

        public async Task<List<PremiosCanjeados>> GetPremiosCanjeados(string? estadoCanje, int minMeses)
        {
            // Los valores viajan como parámetros SQL (no se concatenan), por eso no hay inyección.
            return await db.PremioCanjeadoDtos
                .FromSql($"EXEC sp_AuditoriaPremiosCanjeados @EstadoCanje = {estadoCanje}, @MinimoMesesAntiguedad = {minMeses}")
                .ToListAsync();
        }

        public async Task<List<Top3Productos>> GetTop3Productos(int? año)
        {
            var añoValor = año ?? DateTime.Now.Year;
            string query = $@"
                            WITH TotalesPorProducto AS (
                                SELECT
                                    V.nombre_sede AS nombre_sede,
                                    V.nombre_categoria AS nombre_categoria,
                                    V.nombre_producto AS nombre_producto,
                                    SUM(V.cantidad) AS unidades_vendidas,
                                    SUM(CAST(V.importe_linea AS decimal(18,2))) AS facturacion_total,
                                    SUM(CASE WHEN V.imputado_a_cuenta = 1 THEN CAST(V.importe_linea AS decimal(18,2)) ELSE 0 END) AS facturacion_cuenta_corriente,
                                    SUM(CASE WHEN V.imputado_a_cuenta = 0 THEN CAST(V.importe_linea AS decimal(18,2)) ELSE 0 END) AS facturacion_contado
                                FROM vw_VentasDetalladasProductos V
                                WHERE YEAR(V.fecha_hora_venta) = {añoValor}
                                GROUP BY V.nombre_sede, V.nombre_categoria, V.nombre_producto
                            ),
                            RankingProductos AS (
                                SELECT
                                    T.nombre_sede,
                                    T.nombre_categoria,
                                    T.nombre_producto,
                                    T.unidades_vendidas,
                                    T.facturacion_total,
                                    T.facturacion_cuenta_corriente,
                                    T.facturacion_contado,
                                    ROUND((T.facturacion_cuenta_corriente * 100.0) / NULLIF(T.facturacion_total, 0), 2) AS pct_cuenta_corriente,
                                    ROW_NUMBER() OVER (PARTITION BY T.nombre_sede ORDER BY T.facturacion_total DESC) AS posicion
                                FROM TotalesPorProducto T
                            )
                            SELECT
                                nombre_sede AS Sede,
                                posicion AS PuestoRanking,
                                nombre_producto AS Producto,
                                nombre_categoria AS Categoría,
                                unidades_vendidas AS UnidadesVendidas,
                                facturacion_total AS FacturacionTotal,
                                facturacion_cuenta_corriente AS MontoCtaCte,
                                pct_cuenta_corriente AS PctImputadoCtaCte
                            FROM RankingProductos
                            WHERE posicion <= 3
                            ORDER BY nombre_sede ASC, posicion ASC;";

            return await db.Top3Productos.FromSqlRaw(query).ToListAsync();
        }

        public async Task<List<ReporteReservaTT>> GetReporteReserva(DateTime desde, DateTime hasta, string disciplina1, string disciplina2)
        {
            // Filtros: rango de fechas (BETWEEN) y pertenencia a una lista de disciplinas (IN).
            var fechaDesde = desde.Date;
            var fechaHasta = hasta.Date.AddDays(1);

            return await db.ReporteReservaTT.FromSql($@"
                                SELECT
                                    R.id_reserva AS IdReserva,
                                    P.nombre AS Nombre,
                                    P.apellido AS Apellido,
                                    C.fecha_clase AS FechaClase,
                                    C.hora_inicio AS HoraInicio,
                                    D.nombre_disciplina AS NombreDisciplina,
                                    R.estado_reserva AS EstadoReserva
                                FROM RESERVAS R
                                INNER JOIN SOCIOS S ON R.id_socio = S.id_socio
                                INNER JOIN PERSONAS P ON S.id_persona = P.id_persona
                                INNER JOIN CLASES C ON R.id_clase = C.id_clase
                                INNER JOIN DISCIPLINAS D ON C.id_disciplina = D.id_disciplina
                                WHERE D.nombre_disciplina IN ({disciplina1}, {disciplina2})
                                  AND C.fecha_clase >= {fechaDesde}
                                  AND C.fecha_clase < {fechaHasta}
                                ORDER BY C.fecha_clase, C.hora_inicio").ToListAsync();
        }

        public async Task<List<EvaluacionEstadistica>> GetEvaluacionesEstadistica(DateTime desde, DateTime hasta)
        {
            // Rango de fechas del turno (por defecto: último año). Solo turnos ATENDIDOS, como en el enunciado.
            var fechaDesde = desde.Date;
            var fechaHasta = hasta.Date.AddDays(1);

            return await db.EvaluacionEstadisticas.FromSql($@"
                                SELECT
                                    SED.nombre_sede AS SedeConsultorio,
                                    CONCAT(P_PROF.apellido, ', ', P_PROF.nombre) AS ProfesionalACargo,
                                    R.nombre_rol AS Especialidad,
                                    COUNT(EF.id_evaluacion) AS EvaluacionesRealizadas,
                                    ROUND(AVG(EF.peso_kg), 2) AS PesoPromedioKg,
                                    ROUND(AVG(EF.porcentaje_grasa), 2) AS PorcentajeGrasaPromedio,
                                    ROUND(AVG(EF.porcentaje_masa_muscular), 2) AS PorcentajeMasaMuscularPromedio
                                FROM EVALUACIONES_FISICAS EF
                                INNER JOIN TURNOS T ON EF.id_turno = T.id_turno
                                INNER JOIN SEDES SED ON T.id_sede = SED.id_sede
                                INNER JOIN EMPLEADOS E ON T.id_profesional = E.id_empleado
                                INNER JOIN PERSONAS P_PROF ON E.id_persona = P_PROF.id_persona
                                INNER JOIN ROLES R ON E.id_rol = R.id_rol
                                INNER JOIN ESTADOS_TURNO ET ON T.id_estado_turno = ET.id_estado_turno
                                WHERE ET.nombre_estado = 'ATENDIDO'
                                  AND T.fecha_turno >= {fechaDesde}
                                  AND T.fecha_turno < {fechaHasta}
                                GROUP BY
                                    SED.id_sede,
                                    SED.nombre_sede,
                                    P_PROF.apellido,
                                    P_PROF.nombre,
                                    R.nombre_rol
                                HAVING COUNT(EF.id_evaluacion) >= 1
                                ORDER BY
                                    SED.nombre_sede ASC,
                                    EvaluacionesRealizadas DESC").ToListAsync();
        }

        public async Task<List<ControlAptoMedico>> GetControlAptosMedicos(List<string> estados)
        {
            // Consulta base componible (sin ';' ni ORDER BY): el filtro por pertenencia (IN) se agrega con LINQ.
            var query = db.ControlAptoMedicos.FromSqlRaw(@"
                SELECT
                    s.id_socio AS NumeroSocio,
                    p.dni AS Dni,
                    p.apellido + ', ' + p.nombre AS NombreCompleto,
                    p.telefono AS Telefono,
                    a.fecha_emision AS FechaEmision,
                    a.fecha_vencimiento AS FechaVencimiento,
                    CASE
                        WHEN a.fecha_vencimiento IS NULL THEN 'SIN APTA MEDICA'
                        WHEN a.fecha_vencimiento < CAST(GETDATE() AS DATE) THEN 'VENCIDO'
                        WHEN a.fecha_vencimiento <= DATEADD(DAY, 30, CAST(GETDATE() AS DATE)) THEN 'PROXIMO A VENCER'
                        ELSE 'VIGENTE'
                    END AS EstadoAptoMedico
                FROM APTOS_MEDICOS a
                JOIN SOCIOS s ON a.id_socio = s.id_socio
                JOIN PERSONAS p ON p.id_persona = s.id_persona");

            if (estados.Count > 0)
            {
                query = query.Where(x => estados.Contains(x.EstadoAptoMedico!));
            }

            return await query.OrderBy(x => x.FechaVencimiento).ToListAsync();
        }

        public async Task<List<RentabilidadConvenio>> GetRentabilidadConvenios(string? Estado)
        {
            // Estado de cuenta opcional (comparación). Va como parámetro SQL: si viene vacío no filtra.
            var estado = string.IsNullOrWhiteSpace(Estado) ? null : Estado.Trim().ToUpperInvariant();

            return await db.RentabilidadConvenios.FromSql($@"
                            SELECT
                                CE.razon_social AS EmpresaConvenio,
                                CE.cuit AS Cuit,
                                CE.porcentaje_descuento AS PorcentajeDescuentoAcordado,
                                COUNT(DISTINCT SOC.id_socio) AS SociosAdheridos,
                                COUNT(DISTINCT CM.id_contrato) AS ContratosGenerados,
                                ISNULL(SUM(CC.monto), 0) AS MontoTotalFacturado,
                                ISNULL(SUM(CASE WHEN CC.fecha_pago IS NOT NULL THEN CC.monto ELSE 0 END), 0) AS MontoCobrado,
                                ISNULL(SUM(CASE WHEN CC.fecha_pago IS NULL AND CC.fecha_vencimiento < GETDATE() THEN CC.monto ELSE 0 END), 0) AS MontoEnMora
                            FROM CONVENIOS_EMPRESA CE
                            INNER JOIN SOCIOS SOC ON CE.id_convenio = SOC.id_convenio
                            INNER JOIN CONTRATOS_MEMBRESIA CM ON SOC.id_socio = CM.id_socio
                            INNER JOIN CUOTAS_COBROS CC ON CM.id_contrato = CC.id_contrato
                            WHERE ({estado} IS NULL OR SOC.estado_cuenta = {estado})
                            GROUP BY
                                CE.id_convenio,
                                CE.razon_social,
                                CE.cuit,
                                CE.porcentaje_descuento
                            ORDER BY
                                MontoCobrado DESC,
                                SociosAdheridos DESC").ToListAsync();
        }

        public async Task<List<Models.Tablas.Socio>> GetSociosAsync()
        {
            return await db.Socios
                .AsNoTracking()
                .Include(s => s.IdPersonaNavigation)
                .OrderBy(s => s.IdSocio)
                .ToListAsync();
        }

        public async Task<Models.Tablas.Socio?> GetSocioByIdAsync(int idSocio)
        {
            return await db.Socios
                .Include(s => s.IdPersonaNavigation)
                .Include(s => s.IdCalleNavigation)
                .Include(s => s.IdConvenioNavigation)
                .FirstOrDefaultAsync(s => s.IdSocio == idSocio);
        }

        public async Task<Models.Tablas.Socio?> UpdateSocioAsync(int idSocio, Models.Tablas.Socio socio)
        {
            var entidad = await db.Socios
                .Include(s => s.IdPersonaNavigation)
                .FirstOrDefaultAsync(s => s.IdSocio == idSocio);

            if (entidad == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(socio.IdPersonaNavigation?.Dni))
            {
                throw new ArgumentException("El DNI es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(socio.IdPersonaNavigation?.Nombre) || string.IsNullOrWhiteSpace(socio.IdPersonaNavigation?.Apellido))
            {
                throw new ArgumentException("Nombre y apellido son obligatorios.");
            }

            var dniDuplicado = await db.Personas
                .AnyAsync(p => p.IdPersona != entidad.IdPersona && p.Dni == socio.IdPersonaNavigation.Dni.Trim());
            if (dniDuplicado)
            {
                throw new InvalidOperationException("Ya existe otro socio o persona con ese DNI.");
            }

            entidad.IdCalle = socio.IdCalle;
            entidad.IdConvenio = socio.IdConvenio;
            entidad.FechaIngresoAlta = socio.FechaIngresoAlta;
            entidad.EstadoCuenta = string.IsNullOrWhiteSpace(socio.EstadoCuenta) ? "ACTIVO" : socio.EstadoCuenta;

            entidad.IdPersonaNavigation.Dni = socio.IdPersonaNavigation.Dni.Trim();
            entidad.IdPersonaNavigation.Nombre = socio.IdPersonaNavigation.Nombre.Trim();
            entidad.IdPersonaNavigation.Apellido = socio.IdPersonaNavigation.Apellido.Trim();
            entidad.IdPersonaNavigation.FechaNacimiento = socio.IdPersonaNavigation.FechaNacimiento;
            entidad.IdPersonaNavigation.EstadoCivil = socio.IdPersonaNavigation.EstadoCivil;
            entidad.IdPersonaNavigation.Ocupacion = socio.IdPersonaNavigation.Ocupacion;
            entidad.IdPersonaNavigation.Telefono = socio.IdPersonaNavigation.Telefono;
            entidad.IdPersonaNavigation.Email = socio.IdPersonaNavigation.Email;

            await db.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> DeleteSocioAsync(int idSocio)
        {
            var socio = await db.Socios
                .Include(s => s.ContratosMembresia)
                .FirstOrDefaultAsync(s => s.IdSocio == idSocio);

            if (socio == null)
            {
                return false;
            }

            var contratoActivo = socio.ContratosMembresia.Any(c => c.EstadoContrato == "ACTIVO" || c.FechaVencimiento >= DateOnly.FromDateTime(DateTime.Today));
            if (contratoActivo)
            {
                throw new InvalidOperationException("No se puede eliminar un socio con un contrato activo o vigente.");
            }

            var persona = await db.Personas.FirstOrDefaultAsync(p => p.IdPersona == socio.IdPersona);
            if (persona != null)
            {
                db.Personas.Remove(persona);
            }

            db.Socios.Remove(socio);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Tablas.Socio> CreateSocioAsync(Models.Tablas.Socio socio)
        {
            db.Socios.Add(socio);
            await db.SaveChangesAsync();
            return socio;
        }

        public async Task<(Models.Tablas.Socio Socio, Models.Tablas.ContratosMembresium Contrato, List<Models.Tablas.CuotasCobro> Cuotas)> CreateSocioConContratoAsync(Models.Tablas.Socio socio, Models.Tablas.ContratosMembresium contrato, int idFormaPago = 1)
        {
            if (socio.IdPersonaNavigation == null)
            {
                throw new ArgumentException("La persona del socio es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(socio.IdPersonaNavigation.Dni))
            {
                throw new ArgumentException("El DNI del socio es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(socio.IdPersonaNavigation.Nombre) || string.IsNullOrWhiteSpace(socio.IdPersonaNavigation.Apellido))
            {
                throw new ArgumentException("Nombre y apellido del socio son obligatorios.");
            }

            if (contrato.FechaVencimiento <= contrato.FechaInicio)
            {
                throw new InvalidOperationException("La fecha de vencimiento debe ser posterior a la fecha de inicio.");
            }

            if (idFormaPago <= 0)
            {
                throw new ArgumentException("La forma de pago es obligatoria.");
            }

            var formaPagoExiste = await db.FormasPagos.AnyAsync(f => f.IdFormaPago == idFormaPago);
            if (!formaPagoExiste)
            {
                throw new KeyNotFoundException("La forma de pago indicada no existe.");
            }

            var planExiste = await db.Planes.AnyAsync(p => p.IdPlan == contrato.IdPlan);
            if (!planExiste)
            {
                throw new KeyNotFoundException("El plan indicado no existe.");
            }

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == contrato.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            var dniDuplicado = await db.Personas.AnyAsync(p => p.Dni == socio.IdPersonaNavigation.Dni.Trim());
            if (dniDuplicado)
            {
                throw new InvalidOperationException("Ya existe una persona con ese DNI.");
            }

            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                var persona = new Models.Tablas.Persona
                {
                    Dni = socio.IdPersonaNavigation.Dni.Trim(),
                    Nombre = socio.IdPersonaNavigation.Nombre.Trim(),
                    Apellido = socio.IdPersonaNavigation.Apellido.Trim(),
                    FechaNacimiento = socio.IdPersonaNavigation.FechaNacimiento,
                    EstadoCivil = socio.IdPersonaNavigation.EstadoCivil,
                    Ocupacion = socio.IdPersonaNavigation.Ocupacion,
                    Telefono = socio.IdPersonaNavigation.Telefono,
                    Email = socio.IdPersonaNavigation.Email
                };

                db.Personas.Add(persona);
                await db.SaveChangesAsync();

                socio.IdPersona = persona.IdPersona;
                socio.EstadoCuenta = string.IsNullOrWhiteSpace(socio.EstadoCuenta) ? "ACTIVO" : socio.EstadoCuenta;
                db.Socios.Add(socio);
                await db.SaveChangesAsync();

                var contratoActivo = await db.ContratosMembresia.AnyAsync(c =>
                    c.IdSocio == socio.IdSocio &&
                    c.EstadoContrato == "ACTIVO" &&
                    c.FechaVencimiento >= contrato.FechaInicio);
                if (contratoActivo)
                {
                    throw new InvalidOperationException("El socio ya tiene un contrato activo que se solapa con el período solicitado.");
                }

                contrato.IdSocio = socio.IdSocio;
                contrato.EstadoContrato = string.IsNullOrWhiteSpace(contrato.EstadoContrato) ? "ACTIVO" : contrato.EstadoContrato;
                db.ContratosMembresia.Add(contrato);
                await db.SaveChangesAsync();

                var plan = await db.Planes.FirstAsync(p => p.IdPlan == contrato.IdPlan);
                var mesesContrato = Math.Max(1, (contrato.FechaVencimiento.Year - contrato.FechaInicio.Year) * 12 + (contrato.FechaVencimiento.Month - contrato.FechaInicio.Month));
                var montoPorCuota = mesesContrato > 0 ? plan.PrecioActual / mesesContrato : plan.PrecioActual;

                var cuotas = new List<Models.Tablas.CuotasCobro>();
                var fechaCuota = contrato.FechaInicio;
                for (var i = 1; i <= mesesContrato; i++)
                {
                    var vencimiento = fechaCuota.AddMonths(1);
                    cuotas.Add(new Models.Tablas.CuotasCobro
                    {
                        IdContrato = contrato.IdContrato,
                        IdFormaPago = idFormaPago,
                        NumeroCuota = i,
                        FechaVencimiento = DateOnly.FromDateTime(vencimiento.ToDateTime(TimeOnly.MinValue)),
                        Monto = montoPorCuota,
                        FechaPago = null
                    });

                    fechaCuota = vencimiento;
                }

                if (cuotas.Count == 0)
                {
                    cuotas.Add(new Models.Tablas.CuotasCobro
                    {
                        IdContrato = contrato.IdContrato,
                        IdFormaPago = idFormaPago,
                        NumeroCuota = 1,
                        FechaVencimiento = contrato.FechaVencimiento,
                        Monto = plan.PrecioActual,
                        FechaPago = null
                    });
                }

                db.CuotasCobros.AddRange(cuotas);
                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return (socio, contrato, cuotas);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Models.Tablas.CategoriasProducto>> GetCategoriasProductosAsync()
        {
            return await db.CategoriasProductos
                .AsNoTracking()
                .OrderBy(c => c.NombreCategoria)
                .ToListAsync();
        }

        public async Task<Models.Tablas.CategoriasProducto?> GetCategoriaProductoByIdAsync(int idCategoria)
        {
            return await db.CategoriasProductos
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCategoria == idCategoria);
        }

        public async Task<Models.Tablas.CategoriasProducto> CreateCategoriaProductoAsync(Models.Tablas.CategoriasProducto categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.NombreCategoria))
            {
                throw new ArgumentException("El nombre de la categoría es obligatorio.");
            }

            var existe = await db.CategoriasProductos.AnyAsync(c => c.NombreCategoria == categoria.NombreCategoria);
            if (existe)
            {
                throw new InvalidOperationException("Ya existe una categoría con ese nombre.");
            }

            db.CategoriasProductos.Add(categoria);
            await db.SaveChangesAsync();
            return categoria;
        }

        public async Task<List<Models.Tablas.FormasPago>> GetFormasPagoAsync()
        {
            return await db.FormasPagos
                .AsNoTracking()
                .OrderBy(f => f.NombreFormaPago)
                .ToListAsync();
        }

        public async Task<Models.Tablas.FormasPago?> GetFormaPagoByIdAsync(int idFormaPago)
        {
            return await db.FormasPagos
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.IdFormaPago == idFormaPago);
        }

        public async Task<Models.Tablas.FormasPago> CreateFormaPagoAsync(Models.Tablas.FormasPago formaPago)
        {
            if (string.IsNullOrWhiteSpace(formaPago.NombreFormaPago))
            {
                throw new ArgumentException("El nombre de la forma de pago es obligatorio.");
            }

            var existe = await db.FormasPagos.AnyAsync(f => f.NombreFormaPago == formaPago.NombreFormaPago);
            if (existe)
            {
                throw new InvalidOperationException("Ya existe una forma de pago con ese nombre.");
            }

            db.FormasPagos.Add(formaPago);
            await db.SaveChangesAsync();
            return formaPago;
        }

        public async Task<List<Models.Tablas.EstadosReclamo>> GetEstadosReclamoAsync()
        {
            return await db.EstadosReclamos
                .AsNoTracking()
                .OrderBy(e => e.NombreEstado)
                .ToListAsync();
        }

        public async Task<Models.Tablas.EstadosReclamo?> GetEstadoReclamoByIdAsync(int idEstadoReclamo)
        {
            return await db.EstadosReclamos
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.IdEstadoReclamo == idEstadoReclamo);
        }

        public async Task<Models.Tablas.EstadosReclamo> CreateEstadoReclamoAsync(Models.Tablas.EstadosReclamo estadoReclamo)
        {
            if (string.IsNullOrWhiteSpace(estadoReclamo.NombreEstado))
            {
                throw new ArgumentException("El nombre del estado es obligatorio.");
            }

            var existe = await db.EstadosReclamos.AnyAsync(e => e.NombreEstado == estadoReclamo.NombreEstado);
            if (existe)
            {
                throw new InvalidOperationException("Ya existe un estado de reclamo con ese nombre.");
            }

            db.EstadosReclamos.Add(estadoReclamo);
            await db.SaveChangesAsync();
            return estadoReclamo;
        }

        public async Task<List<Models.Tablas.Provincia>> GetProvinciasAsync()
        {
            return await db.Provincias
                .AsNoTracking()
                .OrderBy(p => p.NombreProvincia)
                .ToListAsync();
        }

        public async Task<Models.Tablas.Provincia?> GetProvinciaByIdAsync(int idProvincia)
        {
            return await db.Provincias
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdProvincia == idProvincia);
        }

        public async Task<Models.Tablas.Provincia> CreateProvinciaAsync(Models.Tablas.Provincia provincia)
        {
            if (string.IsNullOrWhiteSpace(provincia.NombreProvincia))
            {
                throw new ArgumentException("El nombre de la provincia es obligatorio.");
            }

            var existe = await db.Provincias.AnyAsync(p => p.NombreProvincia == provincia.NombreProvincia);
            if (existe)
            {
                throw new InvalidOperationException("Ya existe una provincia con ese nombre.");
            }

            db.Provincias.Add(provincia);
            await db.SaveChangesAsync();
            return provincia;
        }

        public async Task<List<Models.Tablas.Localidade>> GetLocalidadesAsync(int? idProvincia = null)
        {
            var query = db.Localidades
                .AsNoTracking()
                .Include(l => l.IdProvinciaNavigation)
                .AsQueryable();

            if (idProvincia.HasValue)
            {
                query = query.Where(l => l.IdProvincia == idProvincia.Value);
            }

            return await query.OrderBy(l => l.NombreLocalidad).ToListAsync();
        }

        public async Task<Models.Tablas.Localidade?> GetLocalidadByIdAsync(int idLocalidad)
        {
            return await db.Localidades
                .AsNoTracking()
                .Include(l => l.IdProvinciaNavigation)
                .FirstOrDefaultAsync(l => l.IdLocalidad == idLocalidad);
        }

        public async Task<Models.Tablas.Localidade> CreateLocalidadAsync(Models.Tablas.Localidade localidad)
        {
            if (string.IsNullOrWhiteSpace(localidad.NombreLocalidad))
            {
                throw new ArgumentException("El nombre de la localidad es obligatorio.");
            }

            var provinciaExiste = await db.Provincias.AnyAsync(p => p.IdProvincia == localidad.IdProvincia);
            if (!provinciaExiste)
            {
                throw new KeyNotFoundException("La provincia indicada no existe.");
            }

            var existeDuplicado = await db.Localidades.AnyAsync(l => l.IdProvincia == localidad.IdProvincia && l.NombreLocalidad == localidad.NombreLocalidad);
            if (existeDuplicado)
            {
                throw new InvalidOperationException("Ya existe una localidad con ese nombre en la provincia indicada.");
            }

            db.Localidades.Add(localidad);
            await db.SaveChangesAsync();
            return localidad;
        }

        public async Task<List<Models.Tablas.Barrio>> GetBarriosAsync(int? idLocalidad = null)
        {
            var query = db.Barrios
                .AsNoTracking()
                .Include(b => b.IdLocalidadNavigation)
                .AsQueryable();

            if (idLocalidad.HasValue)
            {
                query = query.Where(b => b.IdLocalidad == idLocalidad.Value);
            }

            return await query.OrderBy(b => b.NombreBarrio).ToListAsync();
        }

        public async Task<Models.Tablas.Barrio?> GetBarrioByIdAsync(int idBarrio)
        {
            return await db.Barrios
                .AsNoTracking()
                .Include(b => b.IdLocalidadNavigation)
                .FirstOrDefaultAsync(b => b.IdBarrio == idBarrio);
        }

        public async Task<Models.Tablas.Barrio> CreateBarrioAsync(Models.Tablas.Barrio barrio)
        {
            if (string.IsNullOrWhiteSpace(barrio.NombreBarrio))
            {
                throw new ArgumentException("El nombre del barrio es obligatorio.");
            }

            var localidadExiste = await db.Localidades.AnyAsync(l => l.IdLocalidad == barrio.IdLocalidad);
            if (!localidadExiste)
            {
                throw new KeyNotFoundException("La localidad indicada no existe.");
            }

            var existeDuplicado = await db.Barrios.AnyAsync(b => b.IdLocalidad == barrio.IdLocalidad && b.NombreBarrio == barrio.NombreBarrio);
            if (existeDuplicado)
            {
                throw new InvalidOperationException("Ya existe un barrio con ese nombre en la localidad indicada.");
            }

            db.Barrios.Add(barrio);
            await db.SaveChangesAsync();
            return barrio;
        }

        public async Task<List<Models.Tablas.Calle>> GetCallesAsync(int? idBarrio = null)
        {
            var query = db.Calles
                .AsNoTracking()
                .Include(c => c.IdBarrioNavigation)
                    .ThenInclude(b => b.IdLocalidadNavigation)
                .AsQueryable();

            if (idBarrio.HasValue)
            {
                query = query.Where(c => c.IdBarrio == idBarrio.Value);
            }

            return await query.OrderBy(c => c.NombreCalle).ToListAsync();
        }

        public async Task<Models.Tablas.Calle?> GetCalleByIdAsync(int idCalle)
        {
            return await db.Calles
                .AsNoTracking()
                .Include(c => c.IdBarrioNavigation)
                    .ThenInclude(b => b.IdLocalidadNavigation)
                .FirstOrDefaultAsync(c => c.IdCalle == idCalle);
        }

        public async Task<Models.Tablas.Calle> CreateCalleAsync(Models.Tablas.Calle calle)
        {
            if (string.IsNullOrWhiteSpace(calle.NombreCalle))
            {
                throw new ArgumentException("El nombre de la calle es obligatorio.");
            }

            var barrioExiste = await db.Barrios.AnyAsync(b => b.IdBarrio == calle.IdBarrio);
            if (!barrioExiste)
            {
                throw new KeyNotFoundException("El barrio indicado no existe.");
            }

            var existeDuplicado = await db.Calles.AnyAsync(c => c.IdBarrio == calle.IdBarrio && c.NombreCalle == calle.NombreCalle && c.Numeracion == calle.Numeracion);
            if (existeDuplicado)
            {
                throw new InvalidOperationException("Ya existe una calle con ese nombre y numeración en el barrio indicado.");
            }

            db.Calles.Add(calle);
            await db.SaveChangesAsync();
            return calle;
        }

        public async Task<List<Models.Tablas.Plane>> GetPlanesAsync()
        {
            return await db.Planes
                .AsNoTracking()
                .OrderBy(p => p.NombrePlan)
                .ToListAsync();
        }

        public async Task<List<Models.Tablas.CuotasCobro>> GetCuotasPendientesAsync(int? idSocio = null)
        {
            var query = db.CuotasCobros
                .Include(c => c.IdContratoNavigation)
                    .ThenInclude(c => c.IdSocioNavigation)
                        .ThenInclude(s => s.IdPersonaNavigation)
                .Include(c => c.IdFormaPagoNavigation)
                .Where(c => c.FechaPago == null);

            if (idSocio.HasValue)
            {
                query = query.Where(c => c.IdContratoNavigation.IdSocio == idSocio.Value);
            }

            return await query.OrderBy(c => c.FechaVencimiento).ToListAsync();
        }

        public async Task<List<Models.Tablas.PagosCuota>> GetPagosCuotaAsync(int? idCobro = null, int? idSocio = null)
        {
            var query = db.PagosCuotas
                .AsNoTracking()
                .Include(p => p.IdCobroNavigation)
                    .ThenInclude(c => c.IdContratoNavigation)
                        .ThenInclude(c => c.IdSocioNavigation)
                            .ThenInclude(s => s.IdPersonaNavigation)
                .Include(p => p.IdFormaPagoNavigation)
                .AsQueryable();

            if (idCobro.HasValue)
            {
                query = query.Where(p => p.IdCobro == idCobro.Value);
            }

            if (idSocio.HasValue)
            {
                query = query.Where(p => p.IdCobroNavigation.IdContratoNavigation.IdSocio == idSocio.Value);
            }

            return await query.OrderByDescending(p => p.FechaPago).ToListAsync();
        }

        public async Task<Models.Tablas.PagosCuota> RegistrarPagoCuotaAsync(int idCobro, int idFormaPago, decimal importe, string? usuarioSistema = null)
        {
            var cuota = await db.CuotasCobros
                .FirstOrDefaultAsync(c => c.IdCobro == idCobro)
                ?? throw new KeyNotFoundException("La cuota no existe.");

            if (cuota.FechaPago.HasValue)
            {
                throw new InvalidOperationException("La cuota ya fue cancelada.");
            }

            if (importe <= 0)
            {
                throw new InvalidOperationException("El importe debe ser mayor a cero.");
            }

            var pagosPrevios = await db.PagosCuotas
                .Where(p => p.IdCobro == idCobro)
                .SumAsync(p => (decimal?)p.Importe) ?? 0m;

            if (pagosPrevios + importe > cuota.Monto)
            {
                throw new InvalidOperationException("El importe excede el monto de la cuota.");
            }

            var pago = new Models.Tablas.PagosCuota
            {
                IdCobro = idCobro,
                IdFormaPago = idFormaPago,
                Importe = importe,
                FechaPago = DateTime.Now,
                UsuarioSistema = usuarioSistema
            };

            db.PagosCuotas.Add(pago);
            await db.SaveChangesAsync();

            var totalPagado = pagosPrevios + importe;
            if (totalPagado >= cuota.Monto)
            {
                cuota.FechaPago = DateTime.Now;
                await db.SaveChangesAsync();
            }

            return pago;
        }

        public async Task<List<Models.Tablas.AptosMedico>> GetAptosMedicosAsync()
        {
            return await db.AptosMedicos
                .AsNoTracking()
                .OrderBy(a => a.FechaVencimiento)
                .ToListAsync();
        }

        public async Task<AccesoValidacionResponseDto> ValidarAccesoAsync(int idSocio, int idSede, DateTime fechaHora)
        {
            var fechaActual = DateOnly.FromDateTime(fechaHora.Date);

            if (!await db.Sedes.AnyAsync(s => s.IdSede == idSede))
            {
                return new AccesoValidacionResponseDto
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    Permitido = false,
                    Motivo = "Sede inexistente.",
                    FechaHora = fechaHora
                };
            }

            var socio = await db.Socios
                .Include(s => s.IdPersonaNavigation)
                .FirstOrDefaultAsync(s => s.IdSocio == idSocio);

            if (socio == null)
            {
                return new AccesoValidacionResponseDto
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    Permitido = false,
                    Motivo = "El socio no existe.",
                    FechaHora = fechaHora
                };
            }

            var estado = (socio.EstadoCuenta ?? string.Empty).Trim();
            if (!string.Equals(estado, "ACTIVO", StringComparison.OrdinalIgnoreCase))
            {
                var motivo = "El socio no está activo para ingresar.";
                db.AccesosLogs.Add(new Models.Tablas.AccesosLog
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    FechaHoraMarcado = fechaHora,
                    AccesoPermitido = false,
                    MotivoRechazo = motivo
                });
                db.LogAccesosDenegados.Add(new Models.Tablas.LogAccesosDenegado
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    FechaHora = fechaHora,
                    MotivoBloqueo = motivo
                });
                await db.SaveChangesAsync();
                return new AccesoValidacionResponseDto
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    Permitido = false,
                    Motivo = motivo,
                    EstadoSocio = estado,
                    NombreSocio = socio.IdPersonaNavigation != null ? $"{socio.IdPersonaNavigation.Apellido}, {socio.IdPersonaNavigation.Nombre}" : null,
                    FechaHora = fechaHora
                };
            }

            var contratoVigente = await db.ContratosMembresia
                .FirstOrDefaultAsync(c => c.IdSocio == idSocio
                    && c.IdSede == idSede
                    && c.EstadoContrato == "ACTIVO"
                    && c.FechaInicio <= fechaActual
                    && c.FechaVencimiento >= fechaActual);

            if (contratoVigente == null)
            {
                var motivo = "No existe una membresía vigente para la sede seleccionada.";
                db.AccesosLogs.Add(new Models.Tablas.AccesosLog
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    FechaHoraMarcado = fechaHora,
                    AccesoPermitido = false,
                    MotivoRechazo = motivo
                });
                db.LogAccesosDenegados.Add(new Models.Tablas.LogAccesosDenegado
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    FechaHora = fechaHora,
                    MotivoBloqueo = motivo
                });
                await db.SaveChangesAsync();
                return new AccesoValidacionResponseDto
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    Permitido = false,
                    Motivo = motivo,
                    EstadoSocio = estado,
                    NombreSocio = socio.IdPersonaNavigation != null ? $"{socio.IdPersonaNavigation.Apellido}, {socio.IdPersonaNavigation.Nombre}" : null,
                    FechaHora = fechaHora
                };
            }

            var aptoVigente = await db.AptosMedicos
                .Where(a => a.IdSocio == idSocio && a.FechaVencimiento >= fechaActual)
                .OrderByDescending(a => a.FechaVencimiento)
                .FirstOrDefaultAsync();

            if (aptoVigente == null)
            {
                var motivo = "Falta apto médico vigente.";
                db.AccesosLogs.Add(new Models.Tablas.AccesosLog
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    FechaHoraMarcado = fechaHora,
                    AccesoPermitido = false,
                    MotivoRechazo = motivo
                });
                db.LogAccesosDenegados.Add(new Models.Tablas.LogAccesosDenegado
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    FechaHora = fechaHora,
                    MotivoBloqueo = motivo
                });
                await db.SaveChangesAsync();
                return new AccesoValidacionResponseDto
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    Permitido = false,
                    Motivo = motivo,
                    EstadoSocio = estado,
                    NombreSocio = socio.IdPersonaNavigation != null ? $"{socio.IdPersonaNavigation.Apellido}, {socio.IdPersonaNavigation.Nombre}" : null,
                    FechaHora = fechaHora,
                    ContratoVigente = $"{contratoVigente.IdContrato} - {contratoVigente.FechaInicio:dd/MM/yyyy} a {contratoVigente.FechaVencimiento:dd/MM/yyyy}"
                };
            }

            var sancion = new[] { "SANCIONADO", "BLOQUEADO", "RESTRINGIDO" };
            if (sancion.Contains(estado, StringComparer.OrdinalIgnoreCase))
            {
                var motivo = "Acceso bloqueado por sanción o restricción administrativa.";
                db.AccesosLogs.Add(new Models.Tablas.AccesosLog
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    FechaHoraMarcado = fechaHora,
                    AccesoPermitido = false,
                    MotivoRechazo = motivo
                });
                db.LogAccesosDenegados.Add(new Models.Tablas.LogAccesosDenegado
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    FechaHora = fechaHora,
                    MotivoBloqueo = motivo
                });
                await db.SaveChangesAsync();
                return new AccesoValidacionResponseDto
                {
                    IdSocio = idSocio,
                    IdSede = idSede,
                    Permitido = false,
                    Motivo = motivo,
                    EstadoSocio = estado,
                    NombreSocio = socio.IdPersonaNavigation != null ? $"{socio.IdPersonaNavigation.Apellido}, {socio.IdPersonaNavigation.Nombre}" : null,
                    FechaHora = fechaHora,
                    ContratoVigente = $"{contratoVigente.IdContrato} - {contratoVigente.FechaInicio:dd/MM/yyyy} a {contratoVigente.FechaVencimiento:dd/MM/yyyy}",
                    AptoMedico = $"{aptoVigente.IdApto} - vigente hasta {aptoVigente.FechaVencimiento:dd/MM/yyyy}"
                };
            }

            db.AccesosLogs.Add(new Models.Tablas.AccesosLog
            {
                IdSocio = idSocio,
                IdSede = idSede,
                FechaHoraMarcado = fechaHora,
                AccesoPermitido = true,
                MotivoRechazo = null
            });
            await db.SaveChangesAsync();

            return new AccesoValidacionResponseDto
            {
                IdSocio = idSocio,
                IdSede = idSede,
                Permitido = true,
                Motivo = "Acceso autorizado.",
                EstadoSocio = estado,
                NombreSocio = socio.IdPersonaNavigation != null ? $"{socio.IdPersonaNavigation.Apellido}, {socio.IdPersonaNavigation.Nombre}" : null,
                FechaHora = fechaHora,
                ContratoVigente = $"{contratoVigente.IdContrato} - {contratoVigente.FechaInicio:dd/MM/yyyy} a {contratoVigente.FechaVencimiento:dd/MM/yyyy}",
                AptoMedico = $"{aptoVigente.IdApto} - vigente hasta {aptoVigente.FechaVencimiento:dd/MM/yyyy}"
            };
        }

        public async Task<HistorialSocioDto> GetHistorialSocioAsync(int idSocio)
        {
            var socio = await db.Socios
                .Include(s => s.IdPersonaNavigation)
                .FirstOrDefaultAsync(s => s.IdSocio == idSocio)
                ?? throw new KeyNotFoundException("El socio no existe.");

            var contratos = await db.ContratosMembresia
                .Where(c => c.IdSocio == idSocio)
                .Include(c => c.IdPlanNavigation)
                .Include(c => c.IdSedeNavigation)
                .OrderByDescending(c => c.FechaInicio)
                .ToListAsync();

            var cuotas = await db.CuotasCobros
                .Include(c => c.IdContratoNavigation)
                .Where(c => c.IdContratoNavigation.IdSocio == idSocio)
                .OrderByDescending(c => c.FechaVencimiento)
                .ToListAsync();

            var pagos = await db.PagosCuotas
                .Include(p => p.IdCobroNavigation)
                    .ThenInclude(c => c.IdContratoNavigation)
                .Include(p => p.IdFormaPagoNavigation)
                .Where(p => p.IdCobroNavigation.IdContratoNavigation.IdSocio == idSocio)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();

            var accesos = await db.AccesosLogs
                .Include(a => a.IdSedeNavigation)
                .Where(a => a.IdSocio == idSocio)
                .OrderByDescending(a => a.FechaHoraMarcado)
                .ToListAsync();

            var reservas = await db.Reservas
                .Where(r => r.IdSocio == idSocio)
                .OrderByDescending(r => r.FechaReserva)
                .ToListAsync();

            var aptos = await db.AptosMedicos
                .Where(a => a.IdSocio == idSocio)
                .OrderByDescending(a => a.FechaEmision)
                .ToListAsync();

            var sanciones = new List<string>();
            if (string.Equals(socio.EstadoCuenta, "SANCIONADO", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(socio.EstadoCuenta, "BLOQUEADO", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(socio.EstadoCuenta, "RESTRINGIDO", StringComparison.OrdinalIgnoreCase))
            {
                sanciones.Add(socio.EstadoCuenta);
            }

            return new HistorialSocioDto
            {
                IdSocio = socio.IdSocio,
                NombreCompleto = socio.IdPersonaNavigation != null ? $"{socio.IdPersonaNavigation.Apellido}, {socio.IdPersonaNavigation.Nombre}" : string.Empty,
                EstadoCuenta = socio.EstadoCuenta,
                Contratos = contratos.Select(c => new ContratoMembresiaDto
                {
                    IdContrato = c.IdContrato,
                    IdSocio = c.IdSocio,
                    IdPlan = c.IdPlan,
                    NombrePlan = c.IdPlanNavigation?.NombrePlan ?? string.Empty,
                    IdSede = c.IdSede,
                    NombreSede = c.IdSedeNavigation?.NombreSede ?? string.Empty,
                    FechaInicio = c.FechaInicio,
                    FechaVencimiento = c.FechaVencimiento,
                    EstadoContrato = c.EstadoContrato
                }).ToList(),
                Cuotas = cuotas.Select(c => new CuotaCobroDto
                {
                    IdCobro = c.IdCobro,
                    IdContrato = c.IdContrato,
                    NumeroCuota = c.NumeroCuota,
                    Monto = c.Monto,
                    FechaVencimiento = c.FechaVencimiento,
                    Estado = c.FechaPago.HasValue ? "CANCELADA" : "PENDIENTE",
                    SocioNombre = socio.IdPersonaNavigation != null ? $"{socio.IdPersonaNavigation.Apellido}, {socio.IdPersonaNavigation.Nombre}" : string.Empty,
                    IdFormaPago = c.IdFormaPago
                }).ToList(),
                Pagos = pagos.Select(p => new PagoCuotaHistoricoDto
                {
                    IdPago = p.IdPago,
                    IdCobro = p.IdCobro,
                    NumeroCuota = p.IdCobroNavigation?.NumeroCuota ?? 0,
                    IdSocio = idSocio,
                    NombreSocio = socio.IdPersonaNavigation != null ? $"{socio.IdPersonaNavigation.Apellido}, {socio.IdPersonaNavigation.Nombre}" : string.Empty,
                    IdFormaPago = p.IdFormaPago,
                    NombreFormaPago = p.IdFormaPagoNavigation?.NombreFormaPago ?? string.Empty,
                    Importe = p.Importe,
                    FechaPago = p.FechaPago,
                    UsuarioSistema = p.UsuarioSistema
                }).ToList(),
                Accesos = accesos.Select(a => new AccesoLogDto
                {
                    IdAcceso = a.IdAcceso,
                    IdSocio = a.IdSocio,
                    IdSede = a.IdSede,
                    NombreSede = a.IdSedeNavigation?.NombreSede ?? string.Empty,
                    FechaHoraMarcado = a.FechaHoraMarcado,
                    AccesoPermitido = a.AccesoPermitido,
                    MotivoRechazo = a.MotivoRechazo
                }).ToList(),
                Reservas = reservas.Select(r => new ReservaDto
                {
                    IdReserva = r.IdReserva,
                    IdSocio = r.IdSocio,
                    IdClase = r.IdClase,
                    FechaReserva = r.FechaReserva,
                    EstadoReserva = r.EstadoReserva
                }).ToList(),
                AptosMedicos = aptos.Select(a => new AptoMedicoDto
                {
                    IdApto = a.IdApto,
                    IdSocio = a.IdSocio,
                    FechaEmision = a.FechaEmision,
                    FechaVencimiento = a.FechaVencimiento,
                    MedicoFirmante = a.MedicoFirmante,
                    MatriculaMedico = a.MatriculaMedico,
                    Observaciones = a.Observaciones
                }).ToList(),
                Sanciones = sanciones
            };
        }

        public async Task<List<Models.Tablas.Reserva>> GetReservasEnListaEsperaAsync(int idClase)
        {
            return await db.Reservas
                .Where(r => r.IdClase == idClase && r.EstadoReserva == "LISTA_ESPERA")
                .OrderBy(r => r.FechaReserva)
                .ToListAsync();
        }

        public async Task<Models.Tablas.Reserva?> PromoverSiguienteListaEsperaAsync(int idClase)
        {
            var clase = await db.Clases.FirstOrDefaultAsync(c => c.IdClase == idClase);
            if (clase == null)
            {
                return null;
            }

            var ocupadas = await db.Reservas.CountAsync(r => r.IdClase == idClase && r.EstadoReserva == "CONFIRMADA");
            if (ocupadas >= clase.CupoLimite)
            {
                return null;
            }

            var reserva = await db.Reservas
                .Where(r => r.IdClase == idClase && r.EstadoReserva == "LISTA_ESPERA")
                .OrderBy(r => r.FechaReserva)
                .FirstOrDefaultAsync();

            if (reserva == null)
            {
                return null;
            }

            reserva.EstadoReserva = "CONFIRMADA";
            await db.SaveChangesAsync();
            return reserva;
        }

        public async Task<Models.Tablas.AptosMedico> CreateAptoMedicoAsync(Models.Tablas.AptosMedico apto)
        {
            if (string.IsNullOrWhiteSpace(apto.MedicoFirmante))
            {
                throw new ArgumentException("El nombre del médico firmante es obligatorio.");
            }

            if (apto.FechaVencimiento <= apto.FechaEmision)
            {
                throw new ArgumentException("La fecha de vencimiento debe ser posterior a la fecha de emisión.");
            }

            db.AptosMedicos.Add(apto);
            await db.SaveChangesAsync();
            return apto;
        }

        public async Task<List<Models.Tablas.Clase>> GetClasesAsync()
        {
            return await db.Clases
                .AsNoTracking()
                .OrderBy(c => c.FechaClase)
                .ToListAsync();
        }

        public async Task<List<Models.Tablas.Reserva>> GetReservasAsync(int? idSocio = null)
        {
            var query = db.Reservas
                .AsNoTracking()
                .Include(r => r.IdClaseNavigation)
                .Include(r => r.IdSocioNavigation)
                .AsQueryable();

            if (idSocio.HasValue)
            {
                query = query.Where(r => r.IdSocio == idSocio.Value);
            }

            return await query.OrderByDescending(r => r.FechaReserva).ToListAsync();
        }

        public async Task<Models.Tablas.Reserva?> GetReservaByIdAsync(int idReserva)
        {
            return await db.Reservas
                .Include(r => r.IdClaseNavigation)
                .Include(r => r.IdSocioNavigation)
                .FirstOrDefaultAsync(r => r.IdReserva == idReserva);
        }

        public async Task<Models.Tablas.Reserva> CreateReservaAsync(Models.Tablas.Reserva reserva)
        {
            if (string.IsNullOrWhiteSpace(reserva.EstadoReserva))
            {
                reserva.EstadoReserva = "CONFIRMADA";
            }

            var clase = await db.Clases
                .FirstOrDefaultAsync(c => c.IdClase == reserva.IdClase)
                ?? throw new KeyNotFoundException("La clase no existe.");

            var yaExiste = await db.Reservas
                .AnyAsync(r => r.IdSocio == reserva.IdSocio && r.IdClase == reserva.IdClase && r.EstadoReserva != "CANCELADA");

            if (yaExiste)
            {
                throw new InvalidOperationException("Ya existe una reserva activa para este socio en la clase seleccionada.");
            }

            var reservasActivas = await db.Reservas
                .CountAsync(r => r.IdClase == reserva.IdClase && r.EstadoReserva != "CANCELADA");

            if (reservasActivas >= clase.CupoLimite)
            {
                reserva.EstadoReserva = "LISTA_ESPERA";
            }

            reserva.FechaReserva = DateTime.Now;
            db.Reservas.Add(reserva);
            await db.SaveChangesAsync();
            return reserva;
        }

        public async Task<Models.Tablas.Reserva?> UpdateReservaAsync(int idReserva, Models.Tablas.Reserva reserva)
        {
            var entidad = await db.Reservas.FirstOrDefaultAsync(r => r.IdReserva == idReserva);
            if (entidad == null)
            {
                return null;
            }

            if (reserva.IdSocio <= 0 || reserva.IdClase <= 0)
            {
                throw new ArgumentException("Socio y clase son obligatorios.");
            }

            entidad.IdSocio = reserva.IdSocio;
            entidad.IdClase = reserva.IdClase;
            entidad.EstadoReserva = string.IsNullOrWhiteSpace(reserva.EstadoReserva) ? entidad.EstadoReserva : reserva.EstadoReserva;
            await db.SaveChangesAsync();

            if (string.Equals(entidad.EstadoReserva, "CANCELADA", StringComparison.OrdinalIgnoreCase))
            {
                await PromoverSiguienteListaEsperaAsync(entidad.IdClase);
            }

            return entidad;
        }

        public async Task<bool> DeleteReservaAsync(int idReserva)
        {
            var reserva = await db.Reservas.FirstOrDefaultAsync(r => r.IdReserva == idReserva);
            if (reserva == null)
            {
                return false;
            }

            reserva.EstadoReserva = "CANCELADA";
            await db.SaveChangesAsync();
            await PromoverSiguienteListaEsperaAsync(reserva.IdClase);
            return true;
        }

        public async Task<List<Models.Tablas.Producto>> GetProductosAsync(int? idSede = null)
        {
            var query = db.Productos
                .AsNoTracking()
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.StockSedes)
                    .ThenInclude(s => s.IdSedeNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(p => p.StockSedes.Any(s => s.IdSede == idSede.Value));
            }

            return await query.OrderBy(p => p.NombreProducto).ToListAsync();
        }

        public async Task<Models.Tablas.Producto?> GetProductoByIdAsync(int idProducto)
        {
            return await db.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.StockSedes)
                    .ThenInclude(s => s.IdSedeNavigation)
                .Include(p => p.DetalleVenta)
                .Include(p => p.DetalleComprasStocks)
                .Include(p => p.DetalleTransferencia)
                .FirstOrDefaultAsync(p => p.IdProducto == idProducto);
        }

        public async Task<Models.Tablas.Producto> CreateProductoAsync(Models.Tablas.Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
            {
                throw new ArgumentException("El nombre del producto es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(producto.CodigoBarras))
            {
                throw new ArgumentException("El código de barras es obligatorio.");
            }

            if (producto.PrecioActual <= 0)
            {
                throw new ArgumentException("El precio del producto debe ser mayor a cero.");
            }

            var categoriaExiste = await db.CategoriasProductos.AnyAsync(c => c.IdCategoria == producto.IdCategoria);
            if (!categoriaExiste)
            {
                throw new KeyNotFoundException("La categoría indicada no existe.");
            }

            var existeCodigo = await db.Productos.AnyAsync(p => p.CodigoBarras == producto.CodigoBarras);
            if (existeCodigo)
            {
                throw new InvalidOperationException("Ya existe un producto con ese código de barras.");
            }

            db.Productos.Add(producto);
            await db.SaveChangesAsync();
            return producto;
        }

        public async Task<Models.Tablas.Producto?> UpdateProductoAsync(int idProducto, Models.Tablas.Producto producto)
        {
            var entidad = await db.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == idProducto);

            if (entidad == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
            {
                throw new ArgumentException("El nombre del producto es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(producto.CodigoBarras))
            {
                throw new ArgumentException("El código de barras es obligatorio.");
            }

            if (producto.PrecioActual <= 0)
            {
                throw new ArgumentException("El precio del producto debe ser mayor a cero.");
            }

            var categoriaExiste = await db.CategoriasProductos.AnyAsync(c => c.IdCategoria == producto.IdCategoria);
            if (!categoriaExiste)
            {
                throw new KeyNotFoundException("La categoría indicada no existe.");
            }

            var codigoDuplicado = await db.Productos.AnyAsync(p => p.IdProducto != idProducto && p.CodigoBarras == producto.CodigoBarras);
            if (codigoDuplicado)
            {
                throw new InvalidOperationException("Ya existe otro producto con ese código de barras.");
            }

            entidad.IdCategoria = producto.IdCategoria;
            entidad.NombreProducto = producto.NombreProducto.Trim();
            entidad.CodigoBarras = producto.CodigoBarras.Trim();
            entidad.PrecioActual = producto.PrecioActual;
            entidad.StockMinimo = producto.StockMinimo;

            await db.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> DeleteProductoAsync(int idProducto)
        {
            var producto = await db.Productos
                .Include(p => p.StockSedes)
                .Include(p => p.DetalleVenta)
                .Include(p => p.DetalleComprasStocks)
                .Include(p => p.DetalleTransferencia)
                .FirstOrDefaultAsync(p => p.IdProducto == idProducto);

            if (producto == null)
            {
                return false;
            }

            if (producto.StockSedes.Any(s => s.CantidadActual > 0) || producto.DetalleVenta.Any() || producto.DetalleComprasStocks.Any() || producto.DetalleTransferencia.Any())
            {
                throw new InvalidOperationException("No se puede eliminar un producto que tiene stock, movimientos o ventas asociadas.");
            }

            db.Productos.Remove(producto);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Models.Tablas.StockSede>> GetStockPorSedeAsync(int? idSede = null)
        {
            var query = db.StockSedes
                .AsNoTracking()
                .Include(s => s.IdProductoNavigation)
                    .ThenInclude(p => p.IdCategoriaNavigation)
                .Include(s => s.IdSedeNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(s => s.IdSede == idSede.Value);
            }

            return await query.OrderBy(s => s.IdSede).ThenBy(s => s.IdProducto).ToListAsync();
        }

        public async Task<Models.Tablas.Venta> CrearVentaAsync(Models.Tablas.Venta venta)
        {
            if (venta.DetalleVenta == null || !venta.DetalleVenta.Any())
            {
                throw new InvalidOperationException("La venta debe incluir al menos un producto.");
            }

            if (venta.IdSede <= 0)
            {
                throw new ArgumentException("La sede de la venta es obligatoria.");
            }

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == venta.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                foreach (var detalle in venta.DetalleVenta)
                {
                    var productoExiste = await db.Productos.AnyAsync(p => p.IdProducto == detalle.IdProducto);
                    if (!productoExiste)
                    {
                        throw new KeyNotFoundException($"El producto {detalle.IdProducto} no existe.");
                    }

                    var stock = await db.StockSedes
                        .FirstOrDefaultAsync(s => s.IdSede == venta.IdSede && s.IdProducto == detalle.IdProducto)
                        ?? throw new InvalidOperationException($"No existe stock para el producto {detalle.IdProducto} en la sede {venta.IdSede}.");

                    if (detalle.Cantidad <= 0)
                    {
                        throw new InvalidOperationException($"La cantidad del producto {detalle.IdProducto} debe ser mayor a cero.");
                    }

                    if (stock.CantidadActual < detalle.Cantidad)
                    {
                        throw new InvalidOperationException($"Stock insuficiente para el producto {detalle.IdProducto}. Disponible: {stock.CantidadActual}.");
                    }

                    stock.CantidadActual -= detalle.Cantidad;
                    detalle.PrecioUnitarioHistorico = detalle.PrecioUnitarioHistorico > 0
                        ? detalle.PrecioUnitarioHistorico
                        : await db.Productos.AsNoTracking().Where(p => p.IdProducto == detalle.IdProducto).Select(p => p.PrecioActual).FirstAsync();
                }

                venta.FechaHoraVenta = DateTime.Now;
                db.Ventas.Add(venta);
                await db.SaveChangesAsync();

                foreach (var detalle in venta.DetalleVenta)
                {
                    detalle.IdVenta = venta.IdVenta;
                    detalle.IdVentaNavigation = venta;

                    db.MovimientosStocks.Add(new Models.Tablas.MovimientosStock
                    {
                        IdSede = venta.IdSede,
                        IdProducto = detalle.IdProducto,
                        TipoMovimiento = "SALIDA",
                        Cantidad = detalle.Cantidad,
                        FechaMovimiento = DateTime.Now,
                        IdVenta = venta.IdVenta,
                        UsuarioSistema = "POS",
                        Observaciones = "Venta POS"
                    });
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();
                return venta;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Models.Tablas.Equipamiento>> GetEquiposAsync(int? idSede = null)
        {
            var query = db.Equipamientos
                .AsNoTracking()
                .Include(e => e.IdSedeNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(e => e.IdSede == idSede.Value);
            }

            return await query.OrderBy(e => e.NombreMaquina).ToListAsync();
        }

        public async Task<List<Models.Tablas.IncidenciasReclamo>> GetIncidenciasAsync(int? idSede = null)
        {
            var query = db.IncidenciasReclamos
                .AsNoTracking()
                .Include(i => i.IdEstadoReclamoNavigation)
                .Include(i => i.IdSedeNavigation)
                .Include(i => i.IdEquipoNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(i => i.IdSede == idSede.Value);
            }

            return await query.OrderByDescending(i => i.FechaRegistro).ToListAsync();
        }

        public async Task<Models.Tablas.IncidenciasReclamo> CreateIncidenciaAsync(Models.Tablas.IncidenciasReclamo incidencia)
        {
            if (string.IsNullOrWhiteSpace(incidencia.Descripcion))
            {
                throw new ArgumentException("La descripción de la incidencia es obligatoria.");
            }

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == incidencia.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            var estadoExiste = await db.EstadosReclamos.AnyAsync(e => e.IdEstadoReclamo == incidencia.IdEstadoReclamo);
            if (!estadoExiste)
            {
                throw new KeyNotFoundException("El estado del reclamo no existe.");
            }

            incidencia.FechaRegistro = DateTime.Now;
            db.IncidenciasReclamos.Add(incidencia);
            await db.SaveChangesAsync();
            return incidencia;
        }

        public async Task<List<Models.Tablas.ContratosMembresium>> GetContratosAsync(int? idSocio = null)
        {
            var query = db.ContratosMembresia
                .AsNoTracking()
                .Include(c => c.IdSocioNavigation)
                    .ThenInclude(s => s.IdPersonaNavigation)
                .Include(c => c.IdPlanNavigation)
                .Include(c => c.IdSedeNavigation)
                .AsQueryable();

            if (idSocio.HasValue)
            {
                query = query.Where(c => c.IdSocio == idSocio.Value);
            }

            return await query.OrderByDescending(c => c.FechaInicio).ToListAsync();
        }

        public async Task<Models.Tablas.ContratosMembresium?> GetContratoByIdAsync(int idContrato)
        {
            return await db.ContratosMembresia
                .Include(c => c.IdSocioNavigation)
                    .ThenInclude(s => s.IdPersonaNavigation)
                .Include(c => c.IdPlanNavigation)
                .Include(c => c.IdSedeNavigation)
                .FirstOrDefaultAsync(c => c.IdContrato == idContrato);
        }

        public async Task<Models.Tablas.ContratosMembresium> CreateContratoAsync(Models.Tablas.ContratosMembresium contrato)
        {
            if (contrato.FechaVencimiento <= contrato.FechaInicio)
            {
                throw new InvalidOperationException("La fecha de vencimiento debe ser posterior a la fecha de inicio.");
            }

            var socioExiste = await db.Socios.AnyAsync(s => s.IdSocio == contrato.IdSocio);
            if (!socioExiste)
            {
                throw new KeyNotFoundException("El socio indicado no existe.");
            }

            var planExiste = await db.Planes.AnyAsync(p => p.IdPlan == contrato.IdPlan);
            if (!planExiste)
            {
                throw new KeyNotFoundException("El plan indicado no existe.");
            }

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == contrato.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            var existeActivoSolapado = await db.ContratosMembresia.AnyAsync(c =>
                c.IdSocio == contrato.IdSocio &&
                c.IdContrato != contrato.IdContrato &&
                c.EstadoContrato == "ACTIVO" &&
                c.FechaVencimiento >= contrato.FechaInicio);
            if (existeActivoSolapado)
            {
                throw new InvalidOperationException("El socio ya tiene un contrato activo que se solapa con el período solicitado.");
            }

            contrato.EstadoContrato = string.IsNullOrWhiteSpace(contrato.EstadoContrato) ? "ACTIVO" : contrato.EstadoContrato;
            db.ContratosMembresia.Add(contrato);
            await db.SaveChangesAsync();
            return contrato;
        }

        public async Task<Models.Tablas.ContratosMembresium?> UpdateContratoAsync(int idContrato, Models.Tablas.ContratosMembresium contrato)
        {
            var entidad = await db.ContratosMembresia
                .Include(c => c.IdSocioNavigation)
                    .ThenInclude(s => s.IdPersonaNavigation)
                .Include(c => c.IdPlanNavigation)
                .Include(c => c.IdSedeNavigation)
                .FirstOrDefaultAsync(c => c.IdContrato == idContrato);

            if (entidad == null)
            {
                return null;
            }

            if (contrato.FechaVencimiento <= contrato.FechaInicio)
            {
                throw new InvalidOperationException("La fecha de vencimiento debe ser posterior a la fecha de inicio.");
            }

            var socioExiste = await db.Socios.AnyAsync(s => s.IdSocio == contrato.IdSocio);
            if (!socioExiste)
            {
                throw new KeyNotFoundException("El socio indicado no existe.");
            }

            var planExiste = await db.Planes.AnyAsync(p => p.IdPlan == contrato.IdPlan);
            if (!planExiste)
            {
                throw new KeyNotFoundException("El plan indicado no existe.");
            }

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == contrato.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            var solapa = await db.ContratosMembresia.AnyAsync(c =>
                c.IdSocio == contrato.IdSocio &&
                c.IdContrato != idContrato &&
                c.EstadoContrato == "ACTIVO" &&
                c.FechaVencimiento >= contrato.FechaInicio);
            if (solapa)
            {
                throw new InvalidOperationException("No se puede solapar otro contrato activo para el mismo socio en ese período.");
            }

            entidad.IdSocio = contrato.IdSocio;
            entidad.IdPlan = contrato.IdPlan;
            entidad.IdSede = contrato.IdSede;
            entidad.FechaInicio = contrato.FechaInicio;
            entidad.FechaVencimiento = contrato.FechaVencimiento;
            entidad.EstadoContrato = string.IsNullOrWhiteSpace(contrato.EstadoContrato) ? "ACTIVO" : contrato.EstadoContrato;

            await db.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> DeleteContratoAsync(int idContrato)
        {
            var contrato = await db.ContratosMembresia
                .Include(c => c.CuotasCobros)
                .FirstOrDefaultAsync(c => c.IdContrato == idContrato);

            if (contrato == null)
            {
                return false;
            }

            var tieneCuotasPagas = contrato.CuotasCobros.Any(c => c.FechaPago.HasValue);
            var tieneCuotasVigentes = contrato.CuotasCobros.Any(c => c.FechaVencimiento >= DateOnly.FromDateTime(DateTime.Today));
            if (tieneCuotasPagas || tieneCuotasVigentes)
            {
                throw new InvalidOperationException("No se puede eliminar un contrato con cuotas cobradas o vigentes asociadas.");
            }

            db.ContratosMembresia.Remove(contrato);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Models.Tablas.ContratosMembresium> RenovarContratoAsync(int idContrato, Models.Tablas.ContratosMembresium nuevoContrato, int idFormaPago = 1)
        {
            if (idContrato <= 0)
            {
                throw new ArgumentException("El contrato origen es inválido.");
            }

            var contratoActual = await db.ContratosMembresia
                .Include(c => c.CuotasCobros)
                .FirstOrDefaultAsync(c => c.IdContrato == idContrato)
                ?? throw new KeyNotFoundException("El contrato a renovar no existe.");

            if (idFormaPago <= 0)
            {
                throw new ArgumentException("La forma de pago es obligatoria.");
            }

            var formaPagoExiste = await db.FormasPagos.AnyAsync(f => f.IdFormaPago == idFormaPago);
            if (!formaPagoExiste)
            {
                throw new KeyNotFoundException("La forma de pago indicada no existe.");
            }

            if (nuevoContrato.FechaVencimiento <= nuevoContrato.FechaInicio)
            {
                throw new InvalidOperationException("La fecha de vencimiento debe ser posterior a la fecha de inicio.");
            }

            var solapamiento = await db.ContratosMembresia.AnyAsync(c =>
                c.IdSocio == contratoActual.IdSocio &&
                c.IdContrato != idContrato &&
                c.EstadoContrato == "ACTIVO" &&
                c.FechaInicio <= nuevoContrato.FechaVencimiento &&
                nuevoContrato.FechaInicio <= c.FechaVencimiento);

            if (solapamiento)
            {
                throw new InvalidOperationException("No se puede renovar un contrato si genera solapamiento con otro activo del mismo socio.");
            }

            var plan = await db.Planes.FirstOrDefaultAsync(p => p.IdPlan == nuevoContrato.IdPlan)
                ?? throw new KeyNotFoundException("El plan indicado no existe.");

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == nuevoContrato.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                contratoActual.EstadoContrato = "RENOVADO";

                nuevoContrato.IdSocio = contratoActual.IdSocio;
                nuevoContrato.EstadoContrato = string.IsNullOrWhiteSpace(nuevoContrato.EstadoContrato) ? "ACTIVO" : nuevoContrato.EstadoContrato;
                db.ContratosMembresia.Add(nuevoContrato);
                await db.SaveChangesAsync();

                var fechaCuota = nuevoContrato.FechaInicio;
                var mesesContrato = Math.Max(1, (nuevoContrato.FechaVencimiento.Year - nuevoContrato.FechaInicio.Year) * 12 + (nuevoContrato.FechaVencimiento.Month - nuevoContrato.FechaInicio.Month));
                var montoPorCuota = plan.PrecioActual / mesesContrato;

                var cuotas = new List<Models.Tablas.CuotasCobro>();
                for (var i = 1; i <= mesesContrato; i++)
                {
                    cuotas.Add(new Models.Tablas.CuotasCobro
                    {
                        IdContrato = nuevoContrato.IdContrato,
                        IdFormaPago = idFormaPago,
                        NumeroCuota = i,
                        FechaVencimiento = DateOnly.FromDateTime(fechaCuota.AddMonths(1).ToDateTime(TimeOnly.MinValue)),
                        Monto = montoPorCuota,
                        FechaPago = null
                    });
                    fechaCuota = fechaCuota.AddMonths(1);
                }

                db.CuotasCobros.AddRange(cuotas);
                await db.SaveChangesAsync();
                await transaction.CommitAsync();
                return nuevoContrato;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Models.Tablas.ConveniosEmpresa>> GetConveniosAsync()
        {
            return await db.ConveniosEmpresas
                .AsNoTracking()
                .OrderBy(c => c.RazonSocial)
                .ToListAsync();
        }

        public async Task<Models.Tablas.ConveniosEmpresa?> GetConvenioByIdAsync(int idConvenio)
        {
            return await db.ConveniosEmpresas
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdConvenio == idConvenio);
        }

        public async Task<Models.Tablas.ConveniosEmpresa> CreateConvenioAsync(Models.Tablas.ConveniosEmpresa convenio)
        {
            if (string.IsNullOrWhiteSpace(convenio.RazonSocial))
            {
                throw new ArgumentException("La razón social es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(convenio.Cuit))
            {
                throw new ArgumentException("El CUIT es obligatorio.");
            }

            var existeCuit = await db.ConveniosEmpresas.AnyAsync(c => c.Cuit == convenio.Cuit.Trim());
            if (existeCuit)
            {
                throw new InvalidOperationException("Ya existe un convenio con ese CUIT.");
            }

            db.ConveniosEmpresas.Add(convenio);
            await db.SaveChangesAsync();
            return convenio;
        }

        public async Task<Models.Tablas.ConveniosEmpresa?> UpdateConvenioAsync(int idConvenio, Models.Tablas.ConveniosEmpresa convenio)
        {
            var entidad = await db.ConveniosEmpresas.FirstOrDefaultAsync(c => c.IdConvenio == idConvenio);
            if (entidad == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(convenio.RazonSocial) || string.IsNullOrWhiteSpace(convenio.Cuit))
            {
                throw new ArgumentException("La razón social y el CUIT son obligatorios.");
            }

            var cuitDuplicado = await db.ConveniosEmpresas.AnyAsync(c => c.IdConvenio != idConvenio && c.Cuit == convenio.Cuit.Trim());
            if (cuitDuplicado)
            {
                throw new InvalidOperationException("Ya existe otro convenio con ese CUIT.");
            }

            entidad.RazonSocial = convenio.RazonSocial.Trim();
            entidad.Cuit = convenio.Cuit.Trim();
            entidad.PorcentajeDescuento = convenio.PorcentajeDescuento;
            entidad.DiaCierreFacturacion = convenio.DiaCierreFacturacion;
            entidad.Telefono = convenio.Telefono;
            entidad.Email = convenio.Email;

            await db.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> DeleteConvenioAsync(int idConvenio)
        {
            var convenio = await db.ConveniosEmpresas
                .Include(c => c.Socios)
                .FirstOrDefaultAsync(c => c.IdConvenio == idConvenio);

            if (convenio == null)
            {
                return false;
            }

            if (convenio.Socios.Any())
            {
                throw new InvalidOperationException("No se puede eliminar un convenio que tiene socios asociados.");
            }

            db.ConveniosEmpresas.Remove(convenio);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Models.Tablas.Proveedore>> GetProveedoresAsync()
        {
            return await db.Proveedores
                .AsNoTracking()
                .OrderBy(p => p.RazonSocial)
                .ToListAsync();
        }

        public async Task<Models.Tablas.Proveedore> CreateProveedorAsync(Models.Tablas.Proveedore proveedor)
        {
            if (string.IsNullOrWhiteSpace(proveedor.RazonSocial))
            {
                throw new ArgumentException("La razón social es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(proveedor.Cuit))
            {
                throw new ArgumentException("El CUIT es obligatorio.");
            }

            db.Proveedores.Add(proveedor);
            await db.SaveChangesAsync();
            return proveedor;
        }

        public async Task<List<Models.Tablas.ComprasStock>> GetComprasAsync(int? idSede = null)
        {
            var query = db.ComprasStocks
                .AsNoTracking()
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.IdSedeNavigation)
                .Include(c => c.DetalleComprasStocks)
                    .ThenInclude(d => d.IdProductoNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(c => c.IdSede == idSede.Value);
            }

            return await query.OrderByDescending(c => c.FechaCompra).ToListAsync();
        }

        public async Task<Models.Tablas.ComprasStock> CreateCompraAsync(Models.Tablas.ComprasStock compra)
        {
            if (compra.DetalleComprasStocks == null || !compra.DetalleComprasStocks.Any())
            {
                throw new InvalidOperationException("La compra debe incluir al menos un producto.");
            }

            var proveedorExiste = await db.Proveedores.AnyAsync(p => p.IdProveedor == compra.IdProveedor);
            if (!proveedorExiste)
            {
                throw new KeyNotFoundException("El proveedor indicado no existe.");
            }

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == compra.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            compra.FechaCompra = compra.FechaCompra == default ? DateTime.Now : compra.FechaCompra;

            foreach (var detalle in compra.DetalleComprasStocks)
            {
                var productoExiste = await db.Productos.AnyAsync(p => p.IdProducto == detalle.IdProducto);
                if (!productoExiste)
                {
                    throw new KeyNotFoundException($"El producto {detalle.IdProducto} no existe.");
                }

                var stock = await db.StockSedes.FirstOrDefaultAsync(s => s.IdSede == compra.IdSede && s.IdProducto == detalle.IdProducto);
                if (stock == null)
                {
                    db.StockSedes.Add(new Models.Tablas.StockSede
                    {
                        IdSede = compra.IdSede,
                        IdProducto = detalle.IdProducto,
                        CantidadActual = detalle.Cantidad
                    });
                }
                else
                {
                    stock.CantidadActual += detalle.Cantidad;
                }
            }

            db.ComprasStocks.Add(compra);
            await db.SaveChangesAsync();
            return compra;
        }

        public async Task<List<Models.Tablas.Caja>> GetCajasAsync(int? idSede = null)
        {
            var query = db.Cajas
                .AsNoTracking()
                .Include(c => c.IdSedeNavigation)
                .Include(c => c.IdEmpleadoNavigation)
                    .ThenInclude(e => e.IdPersonaNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(c => c.IdSede == idSede.Value);
            }

            return await query.OrderByDescending(c => c.FechaApertura).ToListAsync();
        }

        public async Task<Models.Tablas.Caja> CreateCajaAsync(Models.Tablas.Caja caja)
        {
            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == caja.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            var empleadoExiste = await db.Empleados.AnyAsync(e => e.IdEmpleado == caja.IdEmpleado);
            if (!empleadoExiste)
            {
                throw new KeyNotFoundException("El empleado indicado no existe.");
            }

            caja.Estado = string.IsNullOrWhiteSpace(caja.Estado) ? "ABIERTA" : caja.Estado;
            caja.FechaApertura = caja.FechaApertura == default ? DateTime.Now : caja.FechaApertura;

            db.Cajas.Add(caja);
            await db.SaveChangesAsync();
            return caja;
        }

        public async Task<List<Models.Tablas.TransferenciasStock>> GetTransferenciasAsync(int? idSede = null)
        {
            var query = db.TransferenciasStocks
                .AsNoTracking()
                .Include(t => t.IdSedeOrigenNavigation)
                .Include(t => t.IdSedeDestinoNavigation)
                .Include(t => t.DetalleTransferencia)
                    .ThenInclude(d => d.IdProductoNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(t => t.IdSedeOrigen == idSede.Value || t.IdSedeDestino == idSede.Value);
            }

            return await query.OrderByDescending(t => t.FechaSolicitud).ToListAsync();
        }

        public async Task<Models.Tablas.TransferenciasStock?> GetTransferenciaByIdAsync(int idTransferencia)
        {
            return await db.TransferenciasStocks
                .Include(t => t.IdSedeOrigenNavigation)
                .Include(t => t.IdSedeDestinoNavigation)
                .Include(t => t.DetalleTransferencia)
                    .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(t => t.IdTransferencia == idTransferencia);
        }

        public async Task<Models.Tablas.TransferenciasStock> CreateTransferenciaAsync(Models.Tablas.TransferenciasStock transferencia)
        {
            if (transferencia.IdSedeOrigen == transferencia.IdSedeDestino)
            {
                throw new InvalidOperationException("La sede origen y destino deben ser distintas.");
            }

            if (transferencia.DetalleTransferencia == null || !transferencia.DetalleTransferencia.Any())
            {
                throw new InvalidOperationException("La transferencia debe incluir al menos un producto.");
            }

            var sedeOrigenExiste = await db.Sedes.AnyAsync(s => s.IdSede == transferencia.IdSedeOrigen);
            if (!sedeOrigenExiste)
            {
                throw new KeyNotFoundException("La sede de origen no existe.");
            }

            var sedeDestinoExiste = await db.Sedes.AnyAsync(s => s.IdSede == transferencia.IdSedeDestino);
            if (!sedeDestinoExiste)
            {
                throw new KeyNotFoundException("La sede de destino no existe.");
            }

            foreach (var detalle in transferencia.DetalleTransferencia)
            {
                var stockOrigen = await db.StockSedes
                    .FirstOrDefaultAsync(s => s.IdSede == transferencia.IdSedeOrigen && s.IdProducto == detalle.IdProducto)
                    ?? throw new InvalidOperationException($"No hay stock del producto {detalle.IdProducto} en la sede de origen.");

                if (stockOrigen.CantidadActual < detalle.Cantidad)
                {
                    throw new InvalidOperationException($"Stock insuficiente para transferir el producto {detalle.IdProducto}.");
                }

                stockOrigen.CantidadActual -= detalle.Cantidad;

                var stockDestino = await db.StockSedes.FirstOrDefaultAsync(s => s.IdSede == transferencia.IdSedeDestino && s.IdProducto == detalle.IdProducto);
                if (stockDestino == null)
                {
                    db.StockSedes.Add(new Models.Tablas.StockSede
                    {
                        IdSede = transferencia.IdSedeDestino,
                        IdProducto = detalle.IdProducto,
                        CantidadActual = detalle.Cantidad
                    });
                }
                else
                {
                    stockDestino.CantidadActual += detalle.Cantidad;
                }
            }

            transferencia.FechaSolicitud = transferencia.FechaSolicitud == default ? DateTime.Now : transferencia.FechaSolicitud;
            transferencia.Estado = string.IsNullOrWhiteSpace(transferencia.Estado) ? "PENDIENTE" : transferencia.Estado;
            db.TransferenciasStocks.Add(transferencia);
            await db.SaveChangesAsync();
            return transferencia;
        }

        public async Task<Models.Tablas.TransferenciasStock?> UpdateTransferenciaAsync(int idTransferencia, Models.Tablas.TransferenciasStock transferencia)
        {
            var entidad = await db.TransferenciasStocks
                .Include(t => t.DetalleTransferencia)
                .FirstOrDefaultAsync(t => t.IdTransferencia == idTransferencia);

            if (entidad == null)
            {
                return null;
            }

            if (transferencia.IdSedeOrigen == transferencia.IdSedeDestino)
            {
                throw new InvalidOperationException("La sede origen y destino deben ser distintas.");
            }

            if (transferencia.DetalleTransferencia == null || !transferencia.DetalleTransferencia.Any())
            {
                throw new InvalidOperationException("La transferencia debe incluir al menos un producto.");
            }

            foreach (var detalleActual in entidad.DetalleTransferencia)
            {
                var stockOrigen = await db.StockSedes.FirstOrDefaultAsync(s => s.IdSede == entidad.IdSedeOrigen && s.IdProducto == detalleActual.IdProducto);
                if (stockOrigen != null)
                {
                    stockOrigen.CantidadActual += detalleActual.Cantidad;
                }

                var stockDestino = await db.StockSedes.FirstOrDefaultAsync(s => s.IdSede == entidad.IdSedeDestino && s.IdProducto == detalleActual.IdProducto);
                if (stockDestino != null)
                {
                    stockDestino.CantidadActual -= detalleActual.Cantidad;
                }
            }

            db.DetalleTransferencias.RemoveRange(entidad.DetalleTransferencia);

            foreach (var detalle in transferencia.DetalleTransferencia)
            {
                var stockOrigen = await db.StockSedes.FirstOrDefaultAsync(s => s.IdSede == transferencia.IdSedeOrigen && s.IdProducto == detalle.IdProducto)
                    ?? throw new InvalidOperationException($"No existe stock del producto {detalle.IdProducto} en la sede origen.");

                if (stockOrigen.CantidadActual < detalle.Cantidad)
                {
                    throw new InvalidOperationException($"Stock insuficiente para el producto {detalle.IdProducto}.");
                }

                stockOrigen.CantidadActual -= detalle.Cantidad;

                var stockDestino = await db.StockSedes.FirstOrDefaultAsync(s => s.IdSede == transferencia.IdSedeDestino && s.IdProducto == detalle.IdProducto);
                if (stockDestino == null)
                {
                    db.StockSedes.Add(new Models.Tablas.StockSede
                    {
                        IdSede = transferencia.IdSedeDestino,
                        IdProducto = detalle.IdProducto,
                        CantidadActual = detalle.Cantidad
                    });
                }
                else
                {
                    stockDestino.CantidadActual += detalle.Cantidad;
                }
            }

            entidad.IdSedeOrigen = transferencia.IdSedeOrigen;
            entidad.IdSedeDestino = transferencia.IdSedeDestino;
            entidad.Estado = string.IsNullOrWhiteSpace(transferencia.Estado) ? "PENDIENTE" : transferencia.Estado;
            entidad.UsuarioSistema = transferencia.UsuarioSistema;
            entidad.FechaSolicitud = transferencia.FechaSolicitud == default ? DateTime.Now : transferencia.FechaSolicitud;

            foreach (var detalle in transferencia.DetalleTransferencia)
            {
                entidad.DetalleTransferencia.Add(new Models.Tablas.DetalleTransferencia
                {
                    IdProducto = detalle.IdProducto,
                    Cantidad = detalle.Cantidad
                });
            }

            await db.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> DeleteTransferenciaAsync(int idTransferencia)
        {
            var transferencia = await db.TransferenciasStocks
                .Include(t => t.DetalleTransferencia)
                .FirstOrDefaultAsync(t => t.IdTransferencia == idTransferencia);

            if (transferencia == null)
            {
                return false;
            }

            if (transferencia.Estado == "COMPLETADA" || transferencia.Estado == "APROBADA")
            {
                foreach (var detalle in transferencia.DetalleTransferencia)
                {
                    var stockOrigen = await db.StockSedes.FirstOrDefaultAsync(s => s.IdSede == transferencia.IdSedeOrigen && s.IdProducto == detalle.IdProducto);
                    if (stockOrigen != null)
                    {
                        stockOrigen.CantidadActual += detalle.Cantidad;
                    }

                    var stockDestino = await db.StockSedes.FirstOrDefaultAsync(s => s.IdSede == transferencia.IdSedeDestino && s.IdProducto == detalle.IdProducto);
                    if (stockDestino != null)
                    {
                        stockDestino.CantidadActual -= detalle.Cantidad;
                    }
                }
            }

            db.TransferenciasStocks.Remove(transferencia);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Models.Tablas.DevolucionesVentum>> GetDevolucionesAsync(int? idVenta = null)
        {
            var query = db.DevolucionesVenta
                .AsNoTracking()
                .Include(d => d.IdEmpleadoNavigation)
                    .ThenInclude(e => e.IdPersonaNavigation)
                .Include(d => d.IdVentaNavigation)
                .Include(d => d.DetalleDevolucionesVenta)
                    .ThenInclude(dd => dd.IdProductoNavigation)
                .AsQueryable();

            if (idVenta.HasValue)
            {
                query = query.Where(d => d.IdVenta == idVenta.Value);
            }

            return await query.OrderByDescending(d => d.FechaDevolucion).ToListAsync();
        }

        public async Task<Models.Tablas.DevolucionesVentum> CreateDevolucionAsync(Models.Tablas.DevolucionesVentum devolucion)
        {
            if (devolucion.DetalleDevolucionesVenta == null || !devolucion.DetalleDevolucionesVenta.Any())
            {
                throw new InvalidOperationException("La devolución debe incluir al menos un producto.");
            }

            var ventaExiste = await db.Ventas.AnyAsync(v => v.IdVenta == devolucion.IdVenta);
            if (!ventaExiste)
            {
                throw new KeyNotFoundException("La venta indicada no existe.");
            }

            var empleadoExiste = await db.Empleados.AnyAsync(e => e.IdEmpleado == devolucion.IdEmpleado);
            if (!empleadoExiste)
            {
                throw new KeyNotFoundException("El empleado indicado no existe.");
            }

            if (string.IsNullOrWhiteSpace(devolucion.Motivo))
            {
                throw new ArgumentException("El motivo de la devolución es obligatorio.");
            }

            devolucion.FechaDevolucion = devolucion.FechaDevolucion == default ? DateTime.Now : devolucion.FechaDevolucion;
            var venta = await db.Ventas
                .Include(v => v.IdSedeNavigation)
                .FirstOrDefaultAsync(v => v.IdVenta == devolucion.IdVenta)
                ?? throw new KeyNotFoundException("La venta indicada no existe.");

            foreach (var detalle in devolucion.DetalleDevolucionesVenta)
            {
                var stock = await db.StockSedes.FirstOrDefaultAsync(s => s.IdSede == venta.IdSede && s.IdProducto == detalle.IdProducto);
                if (stock != null)
                {
                    stock.CantidadActual += detalle.Cantidad;
                }
            }

            db.DevolucionesVenta.Add(devolucion);
            await db.SaveChangesAsync();
            return devolucion;
        }

        public async Task<List<Models.Tablas.MovimientosStock>> GetMovimientosStockAsync(int? idSede = null, int? idProducto = null)
        {
            var query = db.MovimientosStocks
                .AsNoTracking()
                .Include(m => m.IdSedeNavigation)
                .Include(m => m.IdProductoNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(m => m.IdSede == idSede.Value);
            }

            if (idProducto.HasValue)
            {
                query = query.Where(m => m.IdProducto == idProducto.Value);
            }

            return await query.OrderByDescending(m => m.FechaMovimiento).ToListAsync();
        }

        public async Task<Models.Tablas.MovimientosStock> CreateMovimientoStockAsync(Models.Tablas.MovimientosStock movimiento)
        {
            if (movimiento.Cantidad <= 0)
            {
                throw new ArgumentException("La cantidad del movimiento debe ser mayor a cero.");
            }

            if (string.IsNullOrWhiteSpace(movimiento.TipoMovimiento))
            {
                throw new ArgumentException("El tipo de movimiento es obligatorio.");
            }

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == movimiento.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            var productoExiste = await db.Productos.AnyAsync(p => p.IdProducto == movimiento.IdProducto);
            if (!productoExiste)
            {
                throw new KeyNotFoundException("El producto indicado no existe.");
            }

            movimiento.FechaMovimiento = movimiento.FechaMovimiento == default ? DateTime.Now : movimiento.FechaMovimiento;
            movimiento.TipoMovimiento = movimiento.TipoMovimiento.Trim().ToUpperInvariant();

            if (movimiento.TipoMovimiento == "EGRESO" || movimiento.TipoMovimiento == "SALIDA")
            {
                var stockActual = await db.StockSedes
                    .FirstOrDefaultAsync(s => s.IdSede == movimiento.IdSede && s.IdProducto == movimiento.IdProducto);

                if (stockActual == null || stockActual.CantidadActual < movimiento.Cantidad)
                {
                    throw new InvalidOperationException("No hay stock suficiente para registrar esta salida.");
                }

                stockActual.CantidadActual -= movimiento.Cantidad;
            }
            else if (movimiento.TipoMovimiento == "INGRESO" || movimiento.TipoMovimiento == "AJUSTE" || movimiento.TipoMovimiento == "DEVOLUCION")
            {
                var stockActual = await db.StockSedes
                    .FirstOrDefaultAsync(s => s.IdSede == movimiento.IdSede && s.IdProducto == movimiento.IdProducto);

                if (stockActual == null)
                {
                    db.StockSedes.Add(new Models.Tablas.StockSede
                    {
                        IdSede = movimiento.IdSede,
                        IdProducto = movimiento.IdProducto,
                        CantidadActual = movimiento.Cantidad
                    });
                }
                else
                {
                    stockActual.CantidadActual += movimiento.Cantidad;
                }
            }

            db.MovimientosStocks.Add(movimiento);
            await db.SaveChangesAsync();
            return movimiento;
        }

        public async Task<List<Models.Tablas.CierresCaja>> GetCierresCajaAsync(int? idCaja = null)
        {
            var query = db.CierresCajas
                .AsNoTracking()
                .Include(c => c.IdCajaNavigation)
                    .ThenInclude(c => c.IdSedeNavigation)
                .Include(c => c.IdCajaNavigation)
                    .ThenInclude(c => c.IdEmpleadoNavigation)
                        .ThenInclude(e => e.IdPersonaNavigation)
                .AsQueryable();

            if (idCaja.HasValue)
            {
                query = query.Where(c => c.IdCaja == idCaja.Value);
            }

            return await query.OrderByDescending(c => c.FechaCierre).ToListAsync();
        }

        public async Task<Models.Tablas.CierresCaja?> GetCierreCajaByIdAsync(int idCierre)
        {
            return await db.CierresCajas
                .Include(c => c.IdCajaNavigation)
                    .ThenInclude(c => c.IdSedeNavigation)
                .Include(c => c.IdCajaNavigation)
                    .ThenInclude(c => c.IdEmpleadoNavigation)
                        .ThenInclude(e => e.IdPersonaNavigation)
                .FirstOrDefaultAsync(c => c.IdCierre == idCierre);
        }

        public async Task<Models.Tablas.CierresCaja> CreateCierreCajaAsync(Models.Tablas.CierresCaja cierreCaja)
        {
            if (cierreCaja.IdCaja <= 0)
            {
                throw new ArgumentException("El cierre requiere una caja válida.");
            }

            if (cierreCaja.SaldoReal < 0)
            {
                throw new InvalidOperationException("El saldo real no puede ser negativo.");
            }

            var caja = await db.Cajas
                .Include(c => c.IdSedeNavigation)
                .Include(c => c.IdEmpleadoNavigation)
                    .ThenInclude(e => e.IdPersonaNavigation)
                .FirstOrDefaultAsync(c => c.IdCaja == cierreCaja.IdCaja)
                ?? throw new KeyNotFoundException("La caja indicada no existe.");

            if (caja.Estado == "CERRADA")
            {
                throw new InvalidOperationException("La caja ya está cerrada.");
            }

            var fechaCierre = cierreCaja.FechaCierre == default ? DateTime.Now : cierreCaja.FechaCierre;
            var cierreExistente = await db.CierresCajas.AnyAsync(c => c.IdCaja == cierreCaja.IdCaja && c.FechaCierre.Date == fechaCierre.Date);
            if (cierreExistente)
            {
                throw new InvalidOperationException("Ya existe un cierre para esa caja en la fecha indicada.");
            }

            var totalVentas = await db.Ventas
                .Include(v => v.DetalleVenta)
                .Where(v => v.IdVendedor == caja.IdEmpleado && v.FechaHoraVenta.Date == fechaCierre.Date)
                .SelectMany(v => v.DetalleVenta)
                .SumAsync(d => (decimal?)d.Cantidad * d.PrecioUnitarioHistorico) ?? 0m;

            cierreCaja.FechaCierre = fechaCierre;
            cierreCaja.TotalVentas = totalVentas;
            cierreCaja.SaldoEsperado = caja.SaldoInicial + totalVentas;
            cierreCaja.Diferencia = cierreCaja.SaldoReal - cierreCaja.SaldoEsperado;

            const decimal margenAceptado = 5m;
            if (Math.Abs(cierreCaja.Diferencia) > margenAceptado)
            {
                throw new InvalidOperationException($"La diferencia de cierre ({cierreCaja.Diferencia}) supera el margen aceptado de {margenAceptado}.");
            }

            caja.FechaCierre = fechaCierre;
            caja.Estado = "CERRADA";

            db.CierresCajas.Add(cierreCaja);
            await db.SaveChangesAsync();
            return cierreCaja;
        }

        public async Task<Models.Tablas.CierresCaja?> UpdateCierreCajaAsync(int idCierre, Models.Tablas.CierresCaja cierreCaja)
        {
            var entidad = await db.CierresCajas.FirstOrDefaultAsync(c => c.IdCierre == idCierre);
            if (entidad == null)
            {
                return null;
            }

            if (cierreCaja.SaldoReal < 0)
            {
                throw new InvalidOperationException("El saldo real no puede ser negativo.");
            }

            entidad.FechaCierre = cierreCaja.FechaCierre == default ? DateTime.Now : cierreCaja.FechaCierre;
            entidad.SaldoReal = cierreCaja.SaldoReal;
            entidad.Observaciones = cierreCaja.Observaciones;
            entidad.Diferencia = cierreCaja.SaldoReal - entidad.SaldoEsperado;

            await db.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> DeleteCierreCajaAsync(int idCierre)
        {
            var cierre = await db.CierresCajas.FirstOrDefaultAsync(c => c.IdCierre == idCierre);
            if (cierre == null)
            {
                return false;
            }

            var caja = await db.Cajas.FirstOrDefaultAsync(c => c.IdCaja == cierre.IdCaja);
            if (caja != null)
            {
                caja.Estado = "ABIERTA";
                caja.FechaCierre = null;
            }

            db.CierresCajas.Remove(cierre);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Models.Tablas.Empleado>> GetEmpleadosAsync(int? idSede = null, int? idRol = null)
        {
            var query = db.Empleados
                .AsNoTracking()
                .Include(e => e.IdPersonaNavigation)
                .Include(e => e.IdSedeNavigation)
                .Include(e => e.IdRolNavigation)
                .AsQueryable();

            if (idSede.HasValue)
            {
                query = query.Where(e => e.IdSede == idSede.Value);
            }

            if (idRol.HasValue)
            {
                query = query.Where(e => e.IdRol == idRol.Value);
            }

            return await query.OrderBy(e => e.IdPersonaNavigation.Apellido).ThenBy(e => e.IdPersonaNavigation.Nombre).ToListAsync();
        }

        public async Task<Models.Tablas.Empleado> CreateEmpleadoAsync(Models.Tablas.Empleado empleado)
        {
            if (string.IsNullOrWhiteSpace(empleado.Legajo))
            {
                throw new ArgumentException("El legajo es obligatorio.");
            }

            if (empleado.SueldoBase <= 0)
            {
                throw new ArgumentException("El sueldo base debe ser mayor a cero.");
            }

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == empleado.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            var rolExiste = await db.Roles.AnyAsync(r => r.IdRol == empleado.IdRol);
            if (!rolExiste)
            {
                throw new KeyNotFoundException("El rol indicado no existe.");
            }

            db.Empleados.Add(empleado);
            await db.SaveChangesAsync();
            return empleado;
        }

        public async Task<List<Models.Tablas.Rutina>> GetRutinasAsync(int? idSocio = null, int? idEntrenador = null)
        {
            var query = db.Rutinas
                .AsNoTracking()
                .Include(r => r.IdSocioNavigation)
                    .ThenInclude(s => s.IdPersonaNavigation)
                .Include(r => r.IdEntrenadorNavigation)
                    .ThenInclude(e => e.IdPersonaNavigation)
                .Include(r => r.DetalleRutinas)
                .AsQueryable();

            if (idSocio.HasValue)
            {
                query = query.Where(r => r.IdSocio == idSocio.Value);
            }

            if (idEntrenador.HasValue)
            {
                query = query.Where(r => r.IdEntrenador == idEntrenador.Value);
            }

            return await query.OrderByDescending(r => r.FechaCreacion).ToListAsync();
        }

        public async Task<Models.Tablas.Rutina> CreateRutinaAsync(Models.Tablas.Rutina rutina)
        {
            if (rutina.DetalleRutinas == null || !rutina.DetalleRutinas.Any())
            {
                throw new InvalidOperationException("La rutina debe incluir al menos un ejercicio.");
            }

            var socioExiste = await db.Socios.AnyAsync(s => s.IdSocio == rutina.IdSocio);
            if (!socioExiste)
            {
                throw new KeyNotFoundException("El socio indicado no existe.");
            }

            var entrenadorExiste = await db.Empleados.AnyAsync(e => e.IdEmpleado == rutina.IdEntrenador);
            if (!entrenadorExiste)
            {
                throw new KeyNotFoundException("El entrenador indicado no existe.");
            }

            foreach (var detalle in rutina.DetalleRutinas)
            {
                var ejercicioExiste = await db.Ejercicios.AnyAsync(e => e.IdEjercicio == detalle.IdEjercicio);
                if (!ejercicioExiste)
                {
                    throw new KeyNotFoundException($"El ejercicio {detalle.IdEjercicio} no existe.");
                }
            }

            rutina.FechaCreacion = rutina.FechaCreacion == default ? DateOnly.FromDateTime(DateTime.Today) : rutina.FechaCreacion;
            db.Rutinas.Add(rutina);
            await db.SaveChangesAsync();
            return rutina;
        }

        public async Task<List<Models.Tablas.Turno>> GetTurnosAsync(int? idSocio = null, int? idProfesional = null)
        {
            var query = db.Turnos
                .AsNoTracking()
                .Include(t => t.IdSocioNavigation)
                    .ThenInclude(s => s.IdPersonaNavigation)
                .Include(t => t.IdProfesionalNavigation)
                    .ThenInclude(e => e.IdPersonaNavigation)
                .Include(t => t.IdSedeNavigation)
                .Include(t => t.IdEstadoTurnoNavigation)
                .AsQueryable();

            if (idSocio.HasValue)
            {
                query = query.Where(t => t.IdSocio == idSocio.Value);
            }

            if (idProfesional.HasValue)
            {
                query = query.Where(t => t.IdProfesional == idProfesional.Value);
            }

            return await query.OrderBy(t => t.FechaTurno).ThenBy(t => t.HoraTurno).ToListAsync();
        }

        public async Task<Models.Tablas.Turno?> GetTurnoByIdAsync(int idTurno)
        {
            return await db.Turnos
                .Include(t => t.IdSocioNavigation)
                    .ThenInclude(s => s.IdPersonaNavigation)
                .Include(t => t.IdProfesionalNavigation)
                    .ThenInclude(e => e.IdPersonaNavigation)
                .Include(t => t.IdSedeNavigation)
                .Include(t => t.IdEstadoTurnoNavigation)
                .FirstOrDefaultAsync(t => t.IdTurno == idTurno);
        }


        public async Task<Models.Tablas.Turno> CreateTurnoAsync(Models.Tablas.Turno turno)
        {
            if (turno.IdSocio <= 0 || turno.IdProfesional <= 0 || turno.IdSede <= 0)
            {
                throw new ArgumentException("Socio, profesional y sede son obligatorios.");
            }

            var socioExiste = await db.Socios.AnyAsync(s => s.IdSocio == turno.IdSocio);
            if (!socioExiste)
            {
                throw new KeyNotFoundException("El socio indicado no existe.");
            }

            var profesionalExiste = await db.Empleados.AnyAsync(e => e.IdEmpleado == turno.IdProfesional);
            if (!profesionalExiste)
            {
                throw new KeyNotFoundException("El profesional indicado no existe.");
            }

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == turno.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            var estadoExiste = await db.EstadosTurnos.AnyAsync(e => e.IdEstadoTurno == turno.IdEstadoTurno);
            if (!estadoExiste)
            {
                throw new KeyNotFoundException("El estado del turno no existe.");
            }

            turno.FechaTurno = turno.FechaTurno == default ? DateOnly.FromDateTime(DateTime.Today) : turno.FechaTurno;

            var turnosProfesionalFecha = await db.Turnos
                .Where(t => t.IdProfesional == turno.IdProfesional && t.FechaTurno == turno.FechaTurno && t.IdTurno != turno.IdTurno)
                .ToListAsync();

            var existeSolapamientoProfesional = turnosProfesionalFecha.Any(t =>
                turno.HoraTurno < t.HoraTurno.AddMinutes(60) &&
                t.HoraTurno < turno.HoraTurno.AddMinutes(60));

            if (existeSolapamientoProfesional)
            {
                throw new InvalidOperationException("El profesional ya tiene un turno solapado en ese horario y fecha.");
            }

            var turnosSocioFecha = await db.Turnos
                .Where(t => t.IdSocio == turno.IdSocio && t.FechaTurno == turno.FechaTurno && t.IdTurno != turno.IdTurno)
                .ToListAsync();

            var existeSolapamientoSocio = turnosSocioFecha.Any(t =>
                turno.HoraTurno < t.HoraTurno.AddMinutes(60) &&
                t.HoraTurno < turno.HoraTurno.AddMinutes(60));

            if (existeSolapamientoSocio)
            {
                throw new InvalidOperationException("El socio ya tiene otro turno programado solapado en ese horario.");
            }

            db.Turnos.Add(turno);
            await db.SaveChangesAsync();
            return turno;
        }

        public async Task<Models.Tablas.Turno?> UpdateTurnoAsync(int idTurno, Models.Tablas.Turno turno)
        {
            var entidad = await db.Turnos.FirstOrDefaultAsync(t => t.IdTurno == idTurno);
            if (entidad == null)
            {
                return null;
            }

            if (turno.IdSocio <= 0 || turno.IdProfesional <= 0 || turno.IdSede <= 0)
            {
                throw new ArgumentException("Socio, profesional y sede son obligatorios.");
            }

            var socioExiste = await db.Socios.AnyAsync(s => s.IdSocio == turno.IdSocio);
            if (!socioExiste)
            {
                throw new KeyNotFoundException("El socio indicado no existe.");
            }

            var profesionalExiste = await db.Empleados.AnyAsync(e => e.IdEmpleado == turno.IdProfesional);
            if (!profesionalExiste)
            {
                throw new KeyNotFoundException("El profesional indicado no existe.");
            }

            var sedeExiste = await db.Sedes.AnyAsync(s => s.IdSede == turno.IdSede);
            if (!sedeExiste)
            {
                throw new KeyNotFoundException("La sede indicada no existe.");
            }

            var estadoExiste = await db.EstadosTurnos.AnyAsync(e => e.IdEstadoTurno == turno.IdEstadoTurno);
            if (!estadoExiste)
            {
                throw new KeyNotFoundException("El estado del turno no existe.");
            }

            var fechaTurno = turno.FechaTurno == default ? DateOnly.FromDateTime(DateTime.Today) : turno.FechaTurno;

            var turnosProfesionalFecha = await db.Turnos
                .Where(t => t.IdProfesional == turno.IdProfesional && t.FechaTurno == fechaTurno && t.IdTurno != idTurno)
                .ToListAsync();

            var existeSolapamientoProfesional = turnosProfesionalFecha.Any(t =>
                turno.HoraTurno < t.HoraTurno.AddMinutes(60) &&
                t.HoraTurno < turno.HoraTurno.AddMinutes(60));

            if (existeSolapamientoProfesional)
            {
                throw new InvalidOperationException("El profesional ya tiene un turno solapado en ese horario y fecha.");
            }

            var turnosSocioFecha = await db.Turnos
                .Where(t => t.IdSocio == turno.IdSocio && t.FechaTurno == fechaTurno && t.IdTurno != idTurno)
                .ToListAsync();

            var existeSolapamientoSocio = turnosSocioFecha.Any(t =>
                turno.HoraTurno < t.HoraTurno.AddMinutes(60) &&
                t.HoraTurno < turno.HoraTurno.AddMinutes(60));

            if (existeSolapamientoSocio)
            {
                throw new InvalidOperationException("El socio ya tiene otro turno programado solapado en ese horario.");
            }

            entidad.IdSocio = turno.IdSocio;
            entidad.IdProfesional = turno.IdProfesional;
            entidad.IdSede = turno.IdSede;
            entidad.IdEstadoTurno = turno.IdEstadoTurno;
            entidad.FechaTurno = fechaTurno;
            entidad.HoraTurno = turno.HoraTurno;

            await db.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> DeleteTurnoAsync(int idTurno)
        {
            var turno = await db.Turnos.FirstOrDefaultAsync(t => t.IdTurno == idTurno);
            if (turno == null)
            {
                return false;
            }

            db.Turnos.Remove(turno);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Models.Tablas.MantenimientoEquipamiento>> GetMantenimientosEquipamientoAsync(int? idEquipo = null)
        {
            var query = db.MantenimientoEquipamientos
                .AsNoTracking()
                .Include(m => m.IdEquipoNavigation)
                .Include(m => m.IdProveedorNavigation)
                .AsQueryable();

            if (idEquipo.HasValue)
            {
                query = query.Where(m => m.IdEquipo == idEquipo.Value);
            }

            return await query.OrderByDescending(m => m.FechaMantenimiento).ToListAsync();
        }

        public async Task<Models.Tablas.MantenimientoEquipamiento?> GetMantenimientoEquipamientoByIdAsync(int idMantenimiento)
        {
            return await db.MantenimientoEquipamientos
                .Include(m => m.IdEquipoNavigation)
                .Include(m => m.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdMantenimiento == idMantenimiento);
        }

        public async Task<Models.Tablas.MantenimientoEquipamiento> CreateMantenimientoEquipamientoAsync(Models.Tablas.MantenimientoEquipamiento mantenimiento)
        {
            if (string.IsNullOrWhiteSpace(mantenimiento.Descripcion))
            {
                throw new ArgumentException("La descripción del mantenimiento es obligatoria.");
            }

            var equipoExiste = await db.Equipamientos.AnyAsync(e => e.IdEquipo == mantenimiento.IdEquipo);
            if (!equipoExiste)
            {
                throw new KeyNotFoundException("El equipo indicado no existe.");
            }

            if (mantenimiento.IdProveedor.HasValue)
            {
                var proveedorExiste = await db.Proveedores.AnyAsync(p => p.IdProveedor == mantenimiento.IdProveedor.Value);
                if (!proveedorExiste)
                {
                    throw new KeyNotFoundException("El proveedor indicado no existe.");
                }
            }

            mantenimiento.FechaMantenimiento = mantenimiento.FechaMantenimiento == default ? DateOnly.FromDateTime(DateTime.Today) : mantenimiento.FechaMantenimiento;
            db.MantenimientoEquipamientos.Add(mantenimiento);
            await db.SaveChangesAsync();
            return mantenimiento;
        }

        public async Task<Models.Tablas.MantenimientoEquipamiento?> UpdateMantenimientoEquipamientoAsync(int idMantenimiento, Models.Tablas.MantenimientoEquipamiento mantenimiento)
        {
            var entidad = await db.MantenimientoEquipamientos
                .FirstOrDefaultAsync(m => m.IdMantenimiento == idMantenimiento);

            if (entidad == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(mantenimiento.Descripcion))
            {
                throw new ArgumentException("La descripción del mantenimiento es obligatoria.");
            }

            if (mantenimiento.Costo < 0)
            {
                throw new ArgumentException("El costo no puede ser negativo.");
            }

            var equipoExiste = await db.Equipamientos.AnyAsync(e => e.IdEquipo == mantenimiento.IdEquipo);
            if (!equipoExiste)
            {
                throw new KeyNotFoundException("El equipo indicado no existe.");
            }

            if (mantenimiento.IdProveedor.HasValue)
            {
                var proveedorExiste = await db.Proveedores.AnyAsync(p => p.IdProveedor == mantenimiento.IdProveedor.Value);
                if (!proveedorExiste)
                {
                    throw new KeyNotFoundException("El proveedor indicado no existe.");
                }
            }

            entidad.IdEquipo = mantenimiento.IdEquipo;
            entidad.IdProveedor = mantenimiento.IdProveedor;
            entidad.FechaMantenimiento = mantenimiento.FechaMantenimiento;
            entidad.Descripcion = mantenimiento.Descripcion.Trim();
            entidad.Costo = mantenimiento.Costo;

            await db.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> DeleteMantenimientoEquipamientoAsync(int idMantenimiento)
        {
            var mantenimiento = await db.MantenimientoEquipamientos
                .FirstOrDefaultAsync(m => m.IdMantenimiento == idMantenimiento);

            if (mantenimiento == null)
            {
                return false;
            }

            db.MantenimientoEquipamientos.Remove(mantenimiento);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Models.Tablas.Persona>> GetPersonasAsync()
        {
            return await db.Personas
                .AsNoTracking()
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<Models.Tablas.Persona?> GetPersonaByIdAsync(int idPersona)
        {
            return await db.Personas
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdPersona == idPersona);
        }

        public async Task<Models.Tablas.Persona> CreatePersonaAsync(Models.Tablas.Persona persona)
        {
            if (string.IsNullOrWhiteSpace(persona.Dni))
            {
                throw new ArgumentException("El DNI es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(persona.Nombre) || string.IsNullOrWhiteSpace(persona.Apellido))
            {
                throw new ArgumentException("El nombre y el apellido son obligatorios.");
            }

            var existePersona = await db.Personas.AnyAsync(p => p.Dni == persona.Dni);
            if (existePersona)
            {
                throw new InvalidOperationException("Ya existe una persona con ese DNI.");
            }

            persona.EstadoCivil = string.IsNullOrWhiteSpace(persona.EstadoCivil) ? "SOLTERO" : persona.EstadoCivil.Trim();
            persona.Ocupacion = string.IsNullOrWhiteSpace(persona.Ocupacion) ? "SIN_DATO" : persona.Ocupacion.Trim();

            db.Personas.Add(persona);
            await db.SaveChangesAsync();
            return persona;
        }

        public async Task<List<Models.Tablas.Sede>> GetSedesAsync()
        {
            return await db.Sedes
                .AsNoTracking()
                .Include(s => s.IdCalleNavigation)
                .OrderBy(s => s.NombreSede)
                .ToListAsync();
        }

        public async Task<Models.Tablas.Sede?> GetSedeByIdAsync(int idSede)
        {
            return await db.Sedes
                .AsNoTracking()
                .Include(s => s.IdCalleNavigation)
                .FirstOrDefaultAsync(s => s.IdSede == idSede);
        }

        public async Task<Models.Tablas.Sede> CreateSedeAsync(Models.Tablas.Sede sede)
        {
            if (string.IsNullOrWhiteSpace(sede.NombreSede))
            {
                throw new ArgumentException("El nombre de la sede es obligatorio.");
            }

            var calleExiste = await db.Calles.AnyAsync(c => c.IdCalle == sede.IdCalle);
            if (!calleExiste)
            {
                throw new KeyNotFoundException("La calle indicada no existe.");
            }

            var nombreDuplicado = await db.Sedes.AnyAsync(s => s.NombreSede == sede.NombreSede && s.IdCalle == sede.IdCalle);
            if (nombreDuplicado)
            {
                throw new InvalidOperationException("Ya existe una sede con ese nombre en la calle indicada.");
            }

            db.Sedes.Add(sede);
            await db.SaveChangesAsync();
            return sede;
        }

        public async Task<List<Models.Tablas.Role>> GetRolesAsync()
        {
            return await db.Roles
                .AsNoTracking()
                .OrderBy(r => r.NombreRol)
                .ToListAsync();
        }

        public async Task<Models.Tablas.Role?> GetRoleByIdAsync(int idRol)
        {
            return await db.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdRol == idRol);
        }

        public async Task<Models.Tablas.Role> CreateRoleAsync(Models.Tablas.Role role)
        {
            if (string.IsNullOrWhiteSpace(role.NombreRol))
            {
                throw new ArgumentException("El nombre del rol es obligatorio.");
            }

            var existeRol = await db.Roles.AnyAsync(r => r.NombreRol == role.NombreRol);
            if (existeRol)
            {
                throw new InvalidOperationException("Ya existe un rol con ese nombre.");
            }

            db.Roles.Add(role);
            await db.SaveChangesAsync();
            return role;
        }

        public async Task<List<FITLIFENETWORKPROYECTO.Models.Views_Dashboard.VwEstadoCuentaSocio>> GetEstadoCuentaSociosAsync()
        {
            return await db.VwEstadoCuentaSocios
                .AsNoTracking()
                .OrderByDescending(v => v.SaldoPendiente)
                .ToListAsync();
        }

        public async Task<List<FITLIFENETWORKPROYECTO.Models.Views_Dashboard.VwMembresiasVigente>> GetMembresiasVigentesAsync()
        {
            return await db.VwMembresiasVigentes
                .AsNoTracking()
                .OrderBy(v => v.FechaVencimiento)
                .ToListAsync();
        }

        public async Task<List<FITLIFENETWORKPROYECTO.Models.Views_Dashboard.VwOcupacionClasesGrupale>> GetOcupacionClasesAsync()
        {
            return await db.VwOcupacionClasesGrupales
                .AsNoTracking()
                .OrderBy(v => v.FechaClase)
                .ThenBy(v => v.HoraInicio)
                .ToListAsync();
        }

        public async Task<List<FITLIFENETWORKPROYECTO.Models.Views_Dashboard.VwCostosMantenimientoEquipamiento>> GetCostosMantenimientoEquipamientoAsync()
        {
            return await db.VwCostosMantenimientoEquipamientos
                .AsNoTracking()
                .OrderByDescending(v => v.CostoTotal)
                .ToListAsync();
        }
    }
}