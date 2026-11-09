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

        public async Task<List<ReporteRecaudacionSede>> GetRecaudacionSede(int año, int mes)
        {
            string query = @"
                             DECLARE @Año INT = {0};
                             DECLARE @Mes INT = {1};

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
            return db.ReporteRecaudacionSedes.FromSqlRaw(query, año, mes).ToList();
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
            return db.AuditoriaAptoMedicos.FromSqlRaw(query).ToList();
        }

        public async Task<List<PremiosCanjeados>> GetPremiosCanjeados(string estadoCanje, int minMeses)
        {
            return db.Database.SqlQuery<PremiosCanjeados>(
                    $"EXEC sp_AuditoriaPremiosCanjeados @EstadoCanje = {estadoCanje}, @MinimoMesesAntiguedad = {minMeses}").ToList();
        }

        public async Task<List<Top3Productos>> GetTop3Productos(int año)
        {
            string query = @"
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
                            WHERE anio_venta = {0}
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

            return db.Top3Productos.FromSqlRaw(query, año).ToList();
        }

        public async Task<List<ReporteReservaTT>> GetReporteReserva()
        {
            //Busque como hacer Tablas Temporales y siempre decian que era un bardo, que era mas facil mandan el cuerpo de la tabla y listo, asiq eso hice
            string query = @"
                             SELECT
                                    R.id_reserva AS IdReserva,
                                    P.nombre AS Nombre,
                                    P.apellido AS Apellido,
                                    C.fecha_clase AS FechaClase,
                                    C.hora_inicio AS HoraInicio,
                                    R.estado_reserva AS EstadoReserva
                             FROM RESERVAS R
                             INNER JOIN SOCIOS S ON R.id_socio = S.id_socio
                             INNER JOIN PERSONAS P ON S.id_persona = P.id_persona
                             INNER JOIN CLASES C ON R.id_clase = C.id_clase;";

            return db.ReporteReservaTT.FromSqlRaw(query).ToList();
        }

        public async Task<List<EvaluacionEstadistica>> GetEvaluacionesEstadistica()
        {
            string query = @"
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
                              GROUP BY
                                  SED.id_sede,
                                  SED.nombre_sede,
                                  P_PROF.apellido,
                                  P_PROF.nombre,
                                  R.nombre_rol
                              HAVING COUNT(EF.id_evaluacion) >= 1
                              ORDER BY
                                  SED.nombre_sede ASC,
                                  EvaluacionesRealizadas DESC;";

            return db.EvaluacionEstadisticas.FromSqlRaw(query).ToList();
        }
    }
}

