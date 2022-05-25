using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.TRANSMISION;
using Syscaf.Data;
using Syscaf.Data.Helpers.TX;
using Syscaf.Data.Models.WS;
using Syscaf.Service.Helpers;

namespace Syscaf.Service.Portal
{
    public class ProcesoGeneracionService : IProcesoGeneracionService
    {

        private readonly ISyscafConn __conn;

        private readonly ILogService _log;
        public ProcesoGeneracionService(ILogService _log, ISyscafConn __conn)
        {

            this._log = _log;

            this.__conn = __conn;
        }

        public async Task<List<ProcesoGeneracionDatos>> GetFechasGeneracionByServicios(int ServicioId, int rows)
        {

            var fechaServidor = Constants.GetFechaServidor();

            try
            {                           //// debe validr que la tabla a la que va a isnertar el mensaje exista            
                return await __conn.GetAll<ProcesoGeneracionDatos>(ProcesoGeneracionQueryHelper._Get, new
                {
                    ServicioId,
                    rows,
                    EstadoGeneracionId = (int)Enums.EstadoProcesoGeneracionDatos.SW_NOEXEC,
                    FechaServidor = fechaServidor
                }, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _log.SetLogError(0, "ProcesoGeneracionService - GetFechasGeneracionByServicios", ex.ToString());
                return null;
            }
        }
        public async void SetEstadoProcesoGeneracion(int ProcesoGeneracionDatosId, int estado)
        {

            var fechaServidor = Constants.GetFechaServidor();

            try
            {                           //// debe validr que la tabla a la que va a isnertar el mensaje exista            
                 await __conn.Execute(ProcesoGeneracionQueryHelper._UpdateEstado, new
                {
                    ProcesoGeneracionDatosId,
                    estado
                }, commandType: CommandType.Text);

            }
            catch (Exception ex)
            {
                _log.SetLogError(0, "ProcesoGeneracionService - SetEstadoProcesoGeneracion", ex.ToString());
               
            }
        }
        public async void SetLogDetalleProcesoGeneracionDatos(int ProcesoGeneracionDatosId, string Descripcion, int? ClienteId, int EstadoDetallenId)
        {          

            try
            {                           //// debe validr que la tabla a la que va a isnertar el mensaje exista            
                await __conn.Execute(ProcesoGeneracionQueryHelper._InsertDetalleGeneracion, new
                {
                    ProcesoGeneracionDatosId,
                    Descripcion,
                    EstadoDetallenId,
                    FechaSistema = Constants.GetFechaServidor(),
                    ClienteId
                }, commandType: CommandType.Text);

            }
            catch (Exception ex)
            {
                _log.SetLogError(0, "ProcesoGeneracionService - SetEstadoProcesoGeneracion", ex.ToString());

            }
        }
    }

}
public interface IProcesoGeneracionService
{
    Task<List<ProcesoGeneracionDatos>> GetFechasGeneracionByServicios(int ServicioId, int rows);
    void SetEstadoProcesoGeneracion(int ProcesoGeneracionDatosId, int estado);
    void SetLogDetalleProcesoGeneracionDatos(int ProcesoGeneracionDatosId, string Descripcion, int? ClienteId, int estado);

}
