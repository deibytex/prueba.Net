using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal.Models.RAG;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.RAG
{
    public class RagService : IRagService
    {
        private readonly SyscafCoreConn _conn;
        private readonly ILogService _log;
        private readonly ISyscafConn _conProd;
        public RagService(SyscafCoreConn conn, ILogService _log, ISyscafConn _conProd)
        {
            _conn = conn;
            this._log = _log;
            this._conProd = _conProd;
        }

        public async Task<List<SafetyVM>> getInformacionSafetyByClient(int ClienteIds, DateTime? Fecha)
        {
            return await _conProd.GetAllAsync<SafetyVM>("RAG.GetDataPowerBiByCliente_New", new { ClienteIds, Fecha }, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> setEsProcesadoTablaRAG(int clienteIdS, string Reporte, string ReporteIds)
        {
            return await _conProd.ExecuteAsync("RAG.SETTablesPBI_New", new { clienteIdS, Reporte, ReporteIds }, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<SafetyEventosVM>> getEventosSafety(int clienteIdS, string Reporte)
        {
            return await _conProd.GetAllAsync<SafetyEventosVM>("EBUS.GetTablesPBI", new { clienteIdS, Reporte }, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<SafetyEventosVM>> setEsProcesadoTablaSafety(int clienteIdS, string Reporte, string ReporteIds)
        {
            return await _conProd.GetAllAsync<SafetyEventosVM>("EBUS.SETTablesPBI", new { clienteIdS, Reporte, ReporteIds }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> RellenoTripsEventScoring(int clienteIdS, DateTime FechaInicial, DateTime FechaFinal)
        {
            return  await _conProd.ExecuteAsync("RAG.RellenoTripsEventScoring", new { PeriodoFecha = FechaInicial, clienteIdS,  FechaInicial, FechaFinal }, 240);            
        }
     
    }
    

    public interface IRagService
    {
        Task<List<SafetyVM>> getInformacionSafetyByClient(int ClienteIds, DateTime? Fecha);
        Task<List<SafetyEventosVM>> getEventosSafety(int clienteIdS, string Reporte);
        Task<List<SafetyEventosVM>> setEsProcesadoTablaSafety(int clienteIdS, string Reporte, string ReporteIds);
        Task<int> RellenoTripsEventScoring(int clienteIdS, DateTime FechaInicial, DateTime FechaFinal);
        Task<int> setEsProcesadoTablaRAG(int clienteIdS, string Reporte, string ReporteIds);
    }
}
