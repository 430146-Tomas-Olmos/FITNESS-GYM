using FITLIFENETWORKPROYECTO.DTO;

namespace FITLIFENETWORKPROYECTO.Repositorys.Interfaces
{
    public interface IRepository
    {
        public Task<List<ReporteRecaudacionSede>> GetRecaudacionSede(int? año, int? mes);

        public Task<List<AuditoriaAptoMedico>> GetAccesoDenegadoApto();

        public Task<List<PremiosCanjeados>> GetPremiosCanjeados(string estadoCanje, int minMeses);

        public Task<List<Top3Productos>> GetTop3Productos(int? año);

        public Task<List<ReporteReservaTT>> GetReporteReserva();

        public Task<List<EvaluacionEstadistica>> GetEvaluacionesEstadistica();

        public Task<List<ControlAptoMedico>> GetControlAptosMedicos();

        public Task<List<RentabilidadConvenio>> GetRentabilidadConvenios(string? Estado);
    }
}
