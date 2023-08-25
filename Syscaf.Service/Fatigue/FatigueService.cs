using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.FATIGUE;
using Syscaf.Data;
using Syscaf.Data.Helpers.Fatigue;
using Syscaf.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Fatigue
{
    public class FatigueService : IFatigueService
    {
        private readonly SyscafCoreConn _connCore;
        private readonly ILogService _log;
        private readonly ISyscafConn _conProd;

        public FatigueService(SyscafCoreConn conn, ILogService _log, ISyscafConn _conProd)
        {
            _connCore = conn;
            this._log = _log;
            this._conProd = _conProd;
        }


        //public async  Task<List<dynamic>> GetFatigue(string Periodo, int ClienteIds) { 



        //}

        public async Task<int> RellenoEventosDistancia(int clienteIdS, DateTime FechaInicial, DateTime FechaFinal)
        {
            return await _conProd.ExecuteAsync("FATG.RellenoFatiga", new { PeriodoFecha = FechaInicial, clienteIdS, FechaInicial, FechaFinal }, 240);
        }

        //Marcial
        public async Task<ResultObject> SetConfiguracionAlerta(int? Clave, string? Nombre, int? Tiempo, string? Condicion, string? Columna, string? ClienteId, bool? EsActivo, int? ConfiguracionAlertaId)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("Clave", Clave);
                parametros.Add("Nombre", Nombre);
                parametros.Add("Tiempo", Tiempo);
                parametros.Add("Condicion", Condicion);
                parametros.Add("Columna", Columna);
                parametros.Add("ClienteId", ClienteId);
                parametros.Add("EsActivo", EsActivo);
                parametros.Add("ConfiguracionAlertaId", ConfiguracionAlertaId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conProd.Insert<String>(FatigueQueryHelper._Insert, parametros, commandType: CommandType.StoredProcedure));
                    r.Mensaje = result;
                    r.Exitoso = (result == "Operación Éxitosa") ? true : false;
                }
                catch (Exception ex)
                {
                    r.error(ex.Message);
                }

            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> GetConfiguracionAlerta(string? Nombre, string? ClienteId, bool? EsActivo)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();

                parametros.Add("Nombre", Nombre);
                parametros.Add("ClienteId", ClienteId);
                parametros.Add("EsActivo", EsActivo);

                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conProd.GetAll<FatigueVM>(FatigueQueryHelper.GetDatos, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result;
                    r.Mensaje = "Operación Éxitosa";
                    r.Exitoso = (result.Count() > 0) ? true : false;
                    r.success();
                }
                catch (Exception ex)
                {
                    r.error(ex.Message);
                }

            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
    }

    public interface IFatigueService
    {
        Task<int> RellenoEventosDistancia(int clienteIdS, DateTime FechaInicial, DateTime FechaFinal);
        Task<ResultObject> SetConfiguracionAlerta(int? Clave, string? Nombre, int? Tiempo, string? Condicion, string? Columna, string? ClienteId, bool? EsActivo, int? ConfiguracionAlertaId);
        Task<ResultObject> GetConfiguracionAlerta(string? Nombre, string? ClienteId, bool? EsActivo);
    }
}
