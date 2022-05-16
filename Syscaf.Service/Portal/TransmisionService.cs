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
        private readonly ISyscafConn __conn;
        private readonly IMapper _mapper;
        private readonly ILogService _log;
        public TransmisionService(SyscafCoreConn conn, ILogService _log, IMapper _mapper, ISyscafConn __conn)
        {
            _conn = conn;
            this._log = _log;
            this._mapper = _mapper;
            this.__conn = __conn;
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
                    var result = await Task.FromResult(__conn.GetAll<ReporteTxVM>(TransmisionQueryHelper._GetReporteTransmision, parametros, commandType: CommandType.StoredProcedure));
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
        public async Task<ResultObject> GetSnapShotTransmision(int Usuario, DateTime? Fecha, long? ClienteId)
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
                    var result = await Task.FromResult(__conn.GetAll<SnapShotTransmisionVM>(TransmisionQueryHelper._GetSnapShotTransmision, parametros, commandType: CommandType.StoredProcedure));
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
        public async Task<ResultObject> GetSnapshotUnidadesActivas(string Usuario, DateTime? Fecha, long? ClienteId)
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
                    var result = await Task.FromResult(_conn.GetAll<SnapShotUnidadesActivasVM>(TransmisionQueryHelper._GetSnapshotUnidadesActivas, parametros, commandType: CommandType.StoredProcedure));
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
                    var result = await Task.FromResult(__conn.Insert<int>(TransmisionQueryHelper._SetSnapShotTransmision, parametros, commandType: CommandType.StoredProcedure));
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
                    var result = await Task.FromResult(_conn.Insert<int>(TransmisionQueryHelper._SetSnapshotUnidadesActivas, parametros, commandType: CommandType.StoredProcedure));
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
        public async Task<ResultObject> GetAdministradores(string UsuarioId, string Nombres)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("UsuarioId", UsuarioId);
                parametros.Add("Nombres", Nombres);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.GetAll<AdministradoresVM>(TransmisionQueryHelper._GetAdministradores, parametros, commandType: CommandType.StoredProcedure));
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

        public async Task<ResultObject> GetSemanasAnual(int Anio, int Tipo)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("Anio", Anio);
                parametros.Add("Tipo", Tipo);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.GetAll<ListaSemanasAnualVM>(TransmisionQueryHelper._GetSemanasAnual, parametros, commandType: CommandType.StoredProcedure));
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
    }
   
}
public interface ITransmisionService 
{
    Task<ResultObject> GetReporteTransmision(int Usuario, long? clienteId);
    Task<ResultObject> GetSnapShotTransmision(int Usuario, DateTime? Fecha, long? ClienteId);
    Task<ResultObject> GetSnapshotUnidadesActivas(string Usuario, DateTime? Fecha, long? ClienteId);
    Task<ResultObject> SetSnapShotTransmision();
    Task<ResultObject> SetSnapShotUnidadesActivas();
    Task<ResultObject> GetAdministradores(string UsurioId, string Nombre);
    Task<ResultObject> GetSemanasAnual(int Anio, int Tipo);
}
