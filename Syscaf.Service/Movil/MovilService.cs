using AutoMapper;
using Newtonsoft.Json;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.MOVIL;
using Syscaf.Data;
using Syscaf.Data.Helpers.Movil;
using Syscaf.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Portal
{
    public class MovilService : IMovilService
    {
        private readonly SyscafCoreConn _conn;
        private readonly ISyscafConn __conn;
        private readonly IMapper _mapper;
        private readonly ILogService _log;
        public MovilService(SyscafCoreConn conn, ILogService _log, IMapper _mapper, ISyscafConn __conn)
        {
            _conn = conn;
            this._log = _log;
            this._mapper = _mapper;
            this.__conn = __conn;
        }


        public async Task<ResultObject> SetRespuestasPreoperacional(RespuestasVM Respuestas)
        {
            var r = new ResultObject();
            try
            {
                var jsonconvert = JsonConvert.SerializeObject(Respuestas);
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("JSONSTRING", jsonconvert);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(__conn.Insert<string>(MovilQueryHelper._Insert, parametros, commandType: CommandType.StoredProcedure));
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa.";
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

        public async Task<ResultObject> GetRespuestasPreoperacional(string Fecha, string UsuarioId,Int64? ClienteId)
        {
            var r = new ResultObject();
            try
            {
                DateTime? FechaConvert = null;
                if (!String.IsNullOrEmpty(Fecha))
                    FechaConvert = Convert.ToDateTime(Fecha);
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("Fecha", FechaConvert);
                parametros.Add("UsuarioId", UsuarioId);
                parametros.Add("ClienteId", ClienteId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(__conn.GetAll<getRespuestasVM>(MovilQueryHelper._GetRespuestas, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result;
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa.";
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
    public interface IMovilService
    {
        Task<ResultObject> SetRespuestasPreoperacional(RespuestasVM Respuestas);
        Task<ResultObject> GetRespuestasPreoperacional(string Fecha, string UsuarioId, Int64? ClienteId);
    }
}
