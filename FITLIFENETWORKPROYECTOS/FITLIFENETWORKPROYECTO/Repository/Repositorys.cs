using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Models.Context;
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

        public async Task<List<AuditoriaAptoMedico>> GetAccesoDenegadoApto()
        {
            string query = @"
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
                              AND AL.fecha_hora_marcado >= DATEADD(YEAR, -1, GETDATE())
                            GROUP BY
                                S.id_socio,
                                PER.dni,
                                PER.apellido,
                                PER.nombre,
                                SED.nombre_sede,
                                AM.fecha_vencimiento,
                                AM.medico_firmante
                            HAVING COUNT(AL.id_acceso) >= 1
                            ORDER BY IntentosDenegados DESC, UltimoIntentoDenegado DESC;";
            return await db.AuditoriaAptoMedicos.FromSqlRaw(query).ToListAsync();
        }

        public async Task<List<PremiosCanjeados>> GetPremiosCanjeados(string estadoCanje, int minMeses)
        {
            return await db.Database.SqlQuery<PremiosCanjeados>(
                    $"EXEC sp_AuditoriaPremiosCanjeados @EstadoCanje = {estadoCanje}, @MinimoMesesAntiguedad = {minMeses}").ToListAsync();
        }

        public async Task<List<Top3Productos>> GetTop3Productos(int? año)
        {
            string query = @$"
                            WITH TotalesPorProducto AS (
                            SELECT
                            nombre_sede,
                            nombre_categoria,
                            nombre_producto,
                            SUM(cantidad) AS unidades_vendidas,
                            SUM(subtotal_linea) AS facturacion_total,
                            SUM(CASE WHEN imputado_a_cuenta = 1 THEN subtotal_linea ELSE 0 END) AS facturacion_cuenta_corriente,
                            SUM(CASE WHEN imputado_a_cuenta = 0 THEN subtotal_linea ELSE 0 END) AS facturacion_contado
                            FROM vw_VentasDetalladasProductos
                            WHERE anio_venta = {año}
                            GROUP BY nombre_sede, nombre_categoria, nombre_producto
                                                                                    ),
                            RankingProductos AS (
                                                 SELECT
                                                 nombre_sede,
                                                 nombre_categoria,
                                                 nombre_producto,
                                                 unidades_vendidas,
                                                 facturacion_total,
                                                 facturacion_cuenta_corriente,
                                                 facturacion_contado,
                                                 ROUND((facturacion_cuenta_corriente * 100.0) / NULLIF(facturacion_total, 0), 2) AS pct_cuenta_corriente,
                                                 ROW_NUMBER() OVER (PARTITION BY nombre_sede ORDER BY facturacion_total DESC) AS posicion
                                                 FROM TotalesPorProducto
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

            return await db.Top3Productos.FromSqlRaw(query, año).ToListAsync();
        }

        public async Task<List<ReporteReservaTT>> GetReporteReserva(DateTime Fecha1, DateTime Fecha2, string Estado1, string Estado2)
        {
            //Busque como hacer Tablas Temporales y siempre decian que era un bardo, que era mas facil mandan el cuerpo de la tabla y listo, asiq eso hice
            string query = @$"
                                SELECT
                                    R.id_reserva,
                                    P.nombre,
                                    P.apellido,
                                    C.fecha_clase,
                                    C.hora_inicio,
                                    D.nombre_disciplina,
                                    R.estado_reserva
                                FROM RESERVAS R
                                INNER JOIN SOCIOS S
                                    ON R.id_socio = S.id_socio
                                INNER JOIN PERSONAS P
                                    ON S.id_persona = P.id_persona
                                INNER JOIN CLASES C
                                    ON R.id_clase = C.id_clase
                                INNER JOIN DISCIPLINAS D
                                    ON C.id_disciplina = D.id_disciplina
                                WHERE D.nombre_disciplina IN ('{Estado1}', '{Estado2}')
                                AND MONTH(C.fecha_clase) BETWEEN MONTH('{Fecha1}') AND MONTH('{Fecha2}');";

            return await db.ReporteReservaTT.FromSqlRaw(query, Fecha1, Fecha2, Estado1, Estado2).ToListAsync();
        }

        public async Task<List<EvaluacionEstadistica>> GetEvaluacionesEstadistica()
        {
            string query = @$"
                                SELECT 
                                    SED.nombre_sede AS [Sede Consultorio],
                                    CONCAT(P_PROF.apellido, ', ', P_PROF.nombre) AS [Profesional a Cargo],
                                    R.nombre_rol AS [Especialidad],
                                    COUNT(EF.id_evaluacion) AS [Evaluaciones Realizadas],
                                    ROUND(AVG(EF.peso_kg), 2) AS [Peso Promedio (kg)],
                                    ROUND(AVG(EF.porcentaje_grasa), 2) AS [% Grasa Promedio],
                                    ROUND(AVG(EF.porcentaje_masa_muscular), 2) AS [% Masa Muscular Promedio]
                                FROM EVALUACIONES_FISICAS EF
                                INNER JOIN TURNOS T ON EF.id_turno = T.id_turno
                                INNER JOIN SEDES SED ON T.id_sede = SED.id_sede
                                INNER JOIN EMPLEADOS E ON T.id_profesional = E.id_empleado
                                INNER JOIN PERSONAS P_PROF ON E.id_persona = P_PROF.id_persona
                                INNER JOIN ROLES R ON E.id_rol = R.id_rol
                                INNER JOIN ESTADOS_TURNO ET ON T.id_estado_turno = ET.id_estado_turno
                                GROUP BY 
                                    SED.id_sede,
                                    SED.nombre_sede,
                                    P_PROF.apellido,
                                    P_PROF.nombre,
                                    R.nombre_rol
                                HAVING COUNT(EF.id_evaluacion) >= 1
                                ORDER BY 
                                    SED.nombre_sede ASC,
                                    [Evaluaciones Realizadas] DESC";

            return await db.EvaluacionEstadisticas.FromSqlRaw(query).ToListAsync();
        }

        public async Task<List<ControlAptoMedico>> GetControlAptosMedicos()
        {
            string query = @"
            SELECT 
                s.id_socio AS NumeroSocio,
                p.dni AS Dni,
                p.apellido + ', ' + p.nombre AS NombreCompleto,
                p.telefono AS Telefono,
                a.fecha_emision AS FechaEmision,
                a.fecha_vencimiento AS FechaVencimiento,
                CASE
                    WHEN a.fecha_vencimiento < CAST(GETDATE() AS DATE) THEN 'VENCIDO'
                    WHEN a.fecha_vencimiento <= DATEADD(DAY, 30, CAST(GETDATE() AS DATE)) THEN 'PROXIMO A VENCER'
                    ELSE 'VIGENTE'
                END AS EstadoAptoMedico
            FROM APTOS_MEDICOS a
            JOIN SOCIOS s ON a.id_socio = s.id_socio
            JOIN PERSONAS p ON p.id_persona = s.id_persona;";

            return await db.ControlAptoMedicos.FromSqlRaw(query).ToListAsync();
        }

        public async Task<List<RentabilidadConvenio>> GetRentabilidadConvenios(string? Estado)
        {
            string query = @$"
                            SELECT 
                                CE.razon_social AS [Empresa Convenio],
                                CE.cuit AS [CUIT],
                                CE.porcentaje_descuento AS [% Descuento Acordado],
                                COUNT(DISTINCT SOC.id_socio) AS [Socios Adheridos Activos],
                                COUNT(DISTINCT CM.id_contrato) AS [Contratos Generados],
                                ISNULL(SUM(CC.monto), 0) AS [Monto Total Facturado ($)],
                                ISNULL(SUM(CASE WHEN CC.fecha_pago IS NOT NULL THEN CC.monto ELSE 0 END), 0) AS [Monto Cobrado ($)],
                                ISNULL(SUM(CASE WHEN CC.fecha_pago IS NULL AND CC.fecha_vencimiento < GETDATE() THEN CC.monto ELSE 0 END), 0) AS [Monto En Mora ($)]
                            FROM CONVENIOS_EMPRESA CE
                            INNER JOIN SOCIOS SOC ON CE.id_convenio = SOC.id_convenio
                            INNER JOIN CONTRATOS_MEMBRESIA CM ON SOC.id_socio = CM.id_socio
                            INNER JOIN CUOTAS_COBROS CC ON CM.id_contrato = CC.id_contrato
                            WHERE SOC.estado_cuenta IN ('{Estado}')
                            GROUP BY 
                                CE.id_convenio,
                                CE.razon_social,
                                CE.cuit,
                                CE.porcentaje_descuento
                            ORDER BY 
                                [Monto Cobrado ($)] DESC,
                                [Socios Adheridos Activos] DESC;";

            return await db.RentabilidadConvenios.FromSqlRaw(query, Estado).ToListAsync();
        }
    }
}