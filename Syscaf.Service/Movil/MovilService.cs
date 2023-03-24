using AutoMapper;
using Newtonsoft.Json;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.MOVIL;
using Syscaf.Data;
using Syscaf.Data.Helpers.Movil;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Syscaf.Common.Helpers.Enums;

namespace Syscaf.Service.Portal
{
    public class MovilService : IMovilService
    {
        private readonly SyscafCoreConn _conn;
     
        private readonly IMapper _mapper;
        private readonly ILogService _log;
        private readonly IListaDetalleService _listas;
        private readonly INotificacionService _notificacionService;
        private readonly SyscafBDCore _ctx;
        public MovilService(SyscafCoreConn conn, ILogService _log, IMapper _mapper, IListaDetalleService _listas, INotificacionService _notificacionService, SyscafBDCore _ctx)
        {
            _conn = conn;            
            this._log = _log;
            this._mapper = _mapper;
            this._listas = _listas;
            this._notificacionService = _notificacionService;
            this._ctx = _ctx;
        }


        public async Task<ResultObject> SetRespuestasPreoperacional(String Encabezado)
        {
            var r = new ResultObject();
            try
            {
                var Preoperacional = JsonConvert.DeserializeObject<Preoperacional>(Encabezado);
                var JSONSTRING = JsonConvert.SerializeObject(Preoperacional.Respuestas);
                var JSONENCA = JsonConvert.SerializeObject(Preoperacional.Encabezado);
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("JSONSTRING", JSONSTRING);
                parametros.Add("JSONENCA", JSONENCA);

                
                try
                {
                     await  sendNotification(Preoperacional);                   
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.Insert<ResultObject>(MovilQueryHelper._Insert, parametros, commandType: CommandType.StoredProcedure));
                    // CREAMOS AL CONDUCTOR QUE SU ENCUESTA FUE LLENADA SATISFACTORIAMENTE
                    await sendNotificationUsuario(Preoperacional);
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

        private async Task<bool> sendNotification(Preoperacional p) 
        {
            if (p.Encabezado.EstadoPreoperacional == 1) {
            
                var plantilla =  await _notificacionService.GetPlantillaBySigla(PlantillaCorreo.MOV_PREOP.ToString());
                if (plantilla != null) {
                    string asunto = plantilla.Asunto.Replace("{placa}", p.Encabezado.Vehiculo).
                        Replace("{fecha}",  Constants.GetFechaServidorFecha(p.Encabezado.FechaHora));

                    string textodinamico = p.Respuestas.Where(
                        w =>  w.Respuesta != null &&  w.Respuesta.Trim().Equals(w.ValorRedLine?.Trim(), StringComparison.CurrentCultureIgnoreCase)
                        ).Select(s => plantilla.DynamicText.Replace("{pregunta}", s.Pregunta).Replace("{respuesta}", s.Respuesta))
                        .Aggregate((i, j) => i + j);
                    //INGRESAR EL CONDUCTOR ASOCIADO AL USUARIO 

                    // buscamos el nombre o la informacion basica del cusuarii

                    string nombre = _ctx.Users.Find(p.Encabezado.UserId)?.Nombres;
                    string cuerpo = plantilla.Cuerpo.Replace("{rows}", textodinamico).
                        Replace("{placa}", p.Encabezado.Vehiculo).Replace("{conductor}", nombre);

                    var noti =    await _notificacionService.MOVCrearNotificacionPreoperacional(Enums.TipoNotificacion.Sistem, asunto, cuerpo, 4);

                    var parametros =  await _listas.getDetalleListas(Enums.ListasParametros.CORREO.ToString());
                    // CREAMOS LA NOTIFICACION PARA QUE POSTERIOREMENTE EL GESTOR HAGA SEGUIMIENTO A LA ACTIVIDAD
                    await _notificacionService.EnviarCorreosMovilNotificacion((List<DetalleListaDTO>) parametros.Data);
                  
                    return noti.Exitoso;
                
                }               
            }
            return false;
        }
        private async Task<bool> sendNotificationUsuario(Preoperacional p)
        {
            if (p.Encabezado.email != null)
            {

                var plantilla = await _notificacionService.GetPlantillaBySigla(PlantillaCorreo.MOV_PREOPC.ToString());
                if (plantilla != null)
                {
                    string apto =  (p.Encabezado.EstadoPreoperacional == 1) ? "No apto, una notificación será enviada a su gestor." : "Apto.";
                    string asunto = plantilla.Asunto.Replace("{placa}", p.Encabezado.Vehiculo).Replace("{fecha}", Constants.GetFechaServidorFecha(p.Encabezado.FechaHora));
                   
                    string cuerpo = plantilla.Cuerpo.Replace("{apto}", apto).Replace("{placa}", p.Encabezado.Vehiculo);                 
                    var parametros = await _listas.getDetalleListas(Enums.ListasParametros.CORREO.ToString());
                    ResultObject r = _notificacionService.EnviarNotificacion((List<DetalleListaDTO>)parametros.Data, p.Encabezado.email, asunto, cuerpo);


                    return r.Exitoso;

                }
            }
            return false;
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
                    var result = await Task.FromResult(_conn.GetAll<getRespuestasVM>(MovilQueryHelper._GetRespuestas, parametros, commandType: CommandType.StoredProcedure));
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
        public async Task<ResultObject> GetPreguntasPreoperacional(string UsuarioId, string NombrePlantilla, string TipoPregunta, long? ClienteId)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("UsuarioId", UsuarioId);
                parametros.Add("NombrePlantilla", NombrePlantilla);
                parametros.Add("TipoPregunta", TipoPregunta);
                parametros.Add("ClienteId", ClienteId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.GetAll<getPreguntasVM>(MovilQueryHelper._GetPreguntas, parametros, commandType: CommandType.StoredProcedure));
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
        Task<ResultObject> SetRespuestasPreoperacional(String Respuestas);
        Task<ResultObject> GetRespuestasPreoperacional(string Fecha, string UsuarioId, Int64? ClienteId);
        Task<ResultObject> GetPreguntasPreoperacional(string UsuarioId, string NombrePlantilla, string TipoPregunta, long? ClienteId);
    }
}
