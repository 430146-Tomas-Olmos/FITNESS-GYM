using FITLIFENETWORKPROYECTO.DTO;
using FITLIFENETWORKPROYECTO.Repositorys.Interfaces;
using FITLIFENETWORKPROYECTO.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using FITLIFENETWORKPROYECTO.Models;

namespace FITLIFENETWORKPROYECTO.Services
{
    public class ServicesFIT : IServices
    {
        private readonly IRepository repo;

        public ServicesFIT(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<List<AuditoriaAptoMedico>> GetAccesoDenegadoApto()
        {
            return await repo.GetAccesoDenegadoApto();
        }

        public async Task<List<ControlAptoMedico>> GetControlAptosMedicos()
        {
            return await repo.GetControlAptosMedicos();
        }

        public async Task<List<EvaluacionEstadistica>> GetEvaluacionesEstadistica()
        {
            return await repo.GetEvaluacionesEstadistica();
        }

        public async Task<List<PremiosCanjeados>> GetPremiosCanjeados(string estadoCanje, int minMeses)
        {
            return await repo.GetPremiosCanjeados(estadoCanje, minMeses);
        }

        public async Task<List<ReporteRecaudacionSede>> GetRecaudacionSede(int? año, int? mes)
        {
            if(año == null || mes == null)
            {
                throw new ArgumentNullException();
            }
            if(año > DateTime.Today.Year)
            {
                throw new ArgumentException();
            }
            return await repo.GetRecaudacionSede(año, mes);
        }

        public async Task<List<RentabilidadConvenio>> GetRentabilidadConvenios(string? Estado)
        {
            if (Estado == null)
            {
                throw new ArgumentNullException();
            }
            return await repo.GetRentabilidadConvenios(Estado);
        }

        public async Task<List<ReporteReservaTT>> GetReporteReserva()
        {
            return await repo.GetReporteReserva();
        }

        public async Task<List<Top3Productos>> GetTop3Productos(int? año)
        {
            if(año == null)
            {
                throw new ArgumentNullException();
            }
            if (año > DateTime.Today.Year)
            {
                throw new ArgumentException();
            }
            return await repo.GetTop3Productos(año);
        }
    }
}
