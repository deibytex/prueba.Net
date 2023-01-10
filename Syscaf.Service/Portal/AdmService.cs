using Dapper;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Portal
{
    public class AdmService : IAdmService
    {
        private readonly SyscafCoreConn _connCore;
        private readonly ISyscafConn _conDWH;       
        private readonly ILogService _log;

        public AdmService(SyscafCoreConn conn, ILogService _log,  ISyscafConn __conn)
        {
            _connCore = conn;
            this._log = _log;         
            this._conDWH = __conn;
        }

        public async Task<List<dynamic>> getDynamicValueCore(string Clase, string NombreConsulta, DynamicParameters lstparams)
        {
            try
            {
                dynamic consulta = await _connCore.GetAsync<dynamic>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase, NombreConsulta }, commandType: CommandType.Text);

                if (consulta != null)
                {
                    //Se ejecuta el procedimiento almacenado.
                    return await Task.FromResult(_connCore.GetAll<dynamic>(consulta.Consulta, lstparams, commandType: (consulta.Tipo == 2) ? CommandType.Text : CommandType.StoredProcedure));
                }


                else
                    throw new Exception("La consulta no se ha encontrado");

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<int> setDynamicValueCore(string Clase, string NombreConsulta, DynamicParameters lstparams)
        {
            try
            {
                dynamic consulta = await _connCore.GetAsync<dynamic>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase, NombreConsulta }, commandType: CommandType.Text);

                if (consulta != null)
                    //Se ejecuta el procedimiento almacenado.
                    return await Task.FromResult(_connCore.Execute(consulta.Consulta, lstparams, commandType: (consulta.Tipo == 2) ? CommandType.Text : CommandType.StoredProcedure));

                else
                    throw new Exception("La consulta no se ha encontrado");

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }

    public interface IAdmService
    {
        Task<List<dynamic>> getDynamicValueCore(string Clase, string NombreConsulta, DynamicParameters lstparams);
        Task<int> setDynamicValueCore(string Clase, string NombreConsulta, DynamicParameters lstparams);
    }
}
