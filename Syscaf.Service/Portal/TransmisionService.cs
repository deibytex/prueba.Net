using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.TRANSMISION;
using Syscaf.Data;
using Syscaf.Data.Helpers.TX;

using Syscaf.Service.Helpers;

namespace Syscaf.Service.Portal
{
    public class TransmisionService : ITransmisionService
    {
        private readonly SyscafCoreConn _conn;
        private readonly IMapper _mapper;
        private readonly ILogService _log;
        private readonly ISyscafConn _conDWH;
        public TransmisionService(SyscafCoreConn conn, ILogService _log, IMapper _mapper, ISyscafConn _conDWH)
        {
            _conn = conn;
            this._log = _log;
            this._mapper = _mapper;
            this._conDWH = _conDWH;
        }

        public async Task<ResultObject> GetReporteTransmision(int Usuario, long? ClienteId)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("usuarioIdS", Usuario);
                parametros.Add("ClienteId",ClienteId);

                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conDWH.GetAll<ReporteTxVM>(InformeTransmisionQueryHelper._GetInforme, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result.ToList();
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa.";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Usuario " + Usuario);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> GetSnapShotTransmision(int Usuario, DateTime Fecha, long? ClienteId)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("usuarioIdS", Usuario);
                parametros.Add("Fecha", Fecha);
                parametros.Add("ClienteId", ClienteId);

                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conDWH.GetAll<SnapShotTransmisionVM>(GetSnapShotTransmisionQueryHelper._Get, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result.ToList();
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
        public async Task<ResultObject> GetSnapshotUnidadesActivas(string Usuario, DateTime Fecha, long? ClienteId)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("usuarioIdS", Usuario);
                parametros.Add("Fecha", Fecha);
                parametros.Add("ClienteId", ClienteId);

                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.GetAll<SnapShotUnidadesActivasVM>(GetSnapshotUnidadesActivasQueryHelper._Get, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result.ToList();
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
        public async Task<ResultObject> SetSnapShotTransmision()
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();

                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conDWH.Insert<int>(SetSnapShotTransmisionQueryHelper._Set, parametros, commandType: CommandType.StoredProcedure));
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
        public async Task<ResultObject> SetSnapShotUnidadesActivas()
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();

                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.Insert<int>(SetSnapShotUnidadesActivasQueryHelper._Set, parametros, commandType: CommandType.StoredProcedure));
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
   
}
public interface ITransmisionService 
{
    Task<ResultObject> GetReporteTransmision(int Usuario, long? clienteId);
    Task<ResultObject> GetSnapShotTransmision(int Usuario, DateTime Fecha, long? ClienteId);
    Task<ResultObject> GetSnapshotUnidadesActivas(string Usuario, DateTime Fecha, long? ClienteId);
    Task<ResultObject> SetSnapShotTransmision();
    Task<ResultObject> SetSnapShotUnidadesActivas();
}
