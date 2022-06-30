using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
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
            return await _conProd.GetAll<SafetyVM>("RAG.GetDataPowerBiByCliente", new { ClienteIds, Fecha }, commandType: CommandType.StoredProcedure);
        }
    }

    public interface IRagService
    {
        Task<List<SafetyVM>> getInformacionSafetyByClient(int ClienteIds, DateTime? Fecha);
    }
}
