using Syscaf.Common.Helpers;
using Syscaf.Data;
using Syscaf.Data.Models;
using Syscaf.Service.Helpers;
using Syscaf.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;
using Helper = Syscaf.Common.Helpers.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data.Helpers.Portal;

using Syscaf.Data.Models.NS;
using System.Data;
using Syscaf.Data.Models.Portal;
using static Syscaf.Common.Helpers.Enums;

namespace Syscaf.Common.Integrate.LogNotificaciones
{
    public class NotificacionService : INotificacionService
    {
        readonly ILogService LogService;
        readonly ISyscafConn _con;
        readonly SyscafCoreConn _conCore;

      

        // nos trae la infomación de los assets junto a sus clientesreadonly I_LogService __LogService;
        public NotificacionService(ILogService LogService, ISyscafConn _con, SyscafCoreConn _conCore)
        {
            this.LogService = LogService;
            this._con = _con;
            this._conCore = _conCore;
        }

        public async Task<ResultObject> EnviarCorreosMovilNotificacion(List<DetalleListaDTO> smtp)
        {
            ResultObject r = new ResultObject();

            try
            {
                // se coloca un foreach ay que los servicios de automatizacion de azure no permiten ejecucion minuto a minuto 
                // por el plan de azure

                // trae el listado de notificaciones
                var notif = (await GetNotificacionesCorreoMovil()).Select(
                    s => new { s.ListaDistribucionId, s.Asunto, s.Descripcion, s.NotificacionId }
                    ).ToList();

                if (notif.Count > 0)
                {
                    // trae la plantilla para notificar
                    PlantillaDTO plantilla = await GetPlantillaBySigla(Enums.PlantillaCorreo.MOV_PREOP.ToString());

                    // configuracion desde el correo a trabajar
                    string usuarioSmtp = smtp.Find(f => f.Sigla.Equals("USER"))?.Valor;
                    string contraseniaSmtp = smtp.Find(f => f.Sigla.Equals("PSWD"))?.Valor;
                    string host = smtp.Find(f => f.Sigla.Equals("SMTP"))?.Valor;
                    MailNotification _mail = new MailNotification(usuarioSmtp, contraseniaSmtp, host);
                    // agrupo las notificaciones por lista de distribucion
                    foreach (var notificaciones in notif)
                    {
                        // traemos los correos por lista de distribucion
                        List<ListaDistribucionDTO> correos = await GetListadoDistribucionById(notificaciones.ListaDistribucionId);
                        // se adicionan las personas a notificar 
                        _mail.AddRemitente(correos.Select(
                                s => new ListaCorreoVM()
                                {
                                    Correo = s.Correo,
                                    TipoEnvio = s.TipoEnvio
                                }
                                ).ToList());
                        usuarioSmtp = "novedad.preop@syscaf.com.co";
                        var result = _mail.SendEmail(usuarioSmtp, notificaciones.Asunto, notificaciones.Descripcion, "", LogService);

                        if (result.Exitoso)
                            SetEsNotificadoCorreos(notificaciones.NotificacionId);

                    }
                }
                r.success("Enviado Exitosamente");
            }
            catch (Exception exp)
            {
                r.error(exp.Message);
            }

            return r;
        }

        public ResultObject EnviarNotificacion(List<DetalleListaDTO> smtp, string correo, string Asunto, string Cuerpo)
        {
            ResultObject r = new ResultObject();

            try
            {
                // se coloca un foreach ay que los servicios de automatizacion de azure no permiten ejecucion minuto a minuto 
                // por el plan de azure


                // configuracion desde el correo a trabajar
                string usuarioSmtp = smtp.Find(f => f.Sigla.Equals("USER"))?.Valor;
                string contraseniaSmtp = smtp.Find(f => f.Sigla.Equals("PSWD"))?.Valor;
                string host = smtp.Find(f => f.Sigla.Equals("SMTP"))?.Valor;
                MailNotification _mail = new MailNotification(usuarioSmtp, contraseniaSmtp, host);
                // agrupo las notificaciones por lista de distribucion

                _mail.AddRemitente(correo, Enums.TipoEnvio.TO);
                var result = _mail.SendEmail(usuarioSmtp, Asunto, Cuerpo, "", LogService);

                r.success("Enviado Exitosamente");
            }
            catch (Exception exp)
            {
                r.error(exp.Message);
            }

            return r;
        }
        private async Task SetEsNotificadoCorreos(int Id)
        {
            try
            {                //Se ejecuta el procedimiento almacenado.
                await _conCore.GetAllAsync(NotificacionQueryHelper._UpdateEstadoMovi, new { Id }, commandType: CommandType.Text);

            }
            catch (Exception ex)
            {
                LogService.SetLogError(0, "NotificacionService - GetNotificacionesCorreo", ex.Message);
            }
        }

        public async Task<List<dynamic>> GetNotificacionesCorreoMovil()
        {
            try
            {                //Se ejecuta el procedimiento almacenado.
                return await _conCore.GetAllAsync(NotificacionQueryHelper._getNotificacionesSinEnviarMovil, null, commandType: CommandType.Text);

            }
            catch (Exception ex)
            {
                LogService.SetLogError(0, "NotificacionService - GetNotificacionesCorreo", ex.Message);
            }


            return null;
        }

        //_GetListaDistribucionCorreoBySigla
        public async Task<ListaDistribucionDTO> GetListadoDistribucionBySigla(string sigla, long ClienteId)

        {
            ListaDistribucionDTO plantilla = null;
            try
            {
                //Se ejecuta el procedimiento almacenado.
                plantilla = await _con.GetAsync<ListaDistribucionDTO>(NotificacionQueryHelper._GetListaDistribucionCorreoBySigla, new
                {
                    sigla,
                    ClienteId
                }, commandType: CommandType.Text);

            }
            catch (Exception ex)
            {
                LogService.SetLogError(0, "NotificacionService - GetListadoDistribucionBySigla", ex.Message);
            }


            return plantilla;
        }
        public async Task<List<ListaDistribucionDTO>> GetListadoDistribucionById(int Listadistribucionid)

        {
            List<ListaDistribucionDTO> plantilla = null;
            try
            {
                //Se ejecuta el procedimiento almacenado.
                plantilla = await _con.GetAllAsync<ListaDistribucionDTO>(NotificacionQueryHelper._GetListaDistribucionCorreoById, new
                {
                    Listadistribucionid
                }, commandType: CommandType.Text);

            }
            catch (Exception ex)
            {
                LogService.SetLogError(0, "NotificacionService - GetListadoDistribucionBySigla", ex.Message);
            }


            return plantilla;
        }

        //// trae la plantilla segun sigla dada

        public async Task<PlantillaDTO> GetPlantillaBySigla(string Sigla)

        {
            PlantillaDTO plantilla = null;
            try
            {
                //Se ejecuta el procedimiento almacenado.
                plantilla = await _conCore.GetAsync<PlantillaDTO>(NotificacionQueryHelper._GetPlantillaBySigla, new
                {
                    Sigla
                }, commandType: CommandType.Text);

            }
            catch (Exception ex)
            {
                LogService.SetLogError(0, "NotificacionService - GetPlantillaBySigla", ex.Message);
            }


            return plantilla;
        }



        // trae todas las notificaciones sin notificar
        public async Task<List<NotificacionesCorreoDTO>> GetNotificacionesCorreo()
        {

            List<NotificacionesCorreoDTO> notificaciones = null;

            try
            {
                //Se ejecuta el procedimiento almacenado.
                notificaciones = await Task.FromResult(_con.GetAll<NotificacionesCorreoDTO>(NotificacionQueryHelper._getNotificacionesSinEnviar, null, commandType: CommandType.Text));

            }
            catch (Exception ex)
            {
                LogService.SetLogError(0, "NotificacionService - GetPlantillaBySigla", ex.Message);
            }


            return notificaciones;

        }


        // actualiza el estado de las notificaciones a verdadero

        public async Task<ResultObject> SetEsNotificadoCorreos(List<long> ids)
        {
            ResultObject result = new ResultObject();
            try
            {
                var resultt = await Task.FromResult(_con.ExecuteAsync(NotificacionQueryHelper._UpdateEstado, new
                {
                    Id = ids
                }));

                result.success();
            }
            catch (Exception ex)
            {
                result.error("Error al actualizar las notificaciones, por favor revise el log.");

                LogService.SetLogError(0, "NotificacionService - SetEsNotificadoCorreos", ex.ToString());
            }

            return result;

        }




        public async Task<ResultObject> CrearLogNotificacion(Enums.TipoNotificacion tipo, string descripcion, Enums.ListaDistribucion lista)
        {
            ResultObject result = new();
            try
            {

                var resultt = await _con.ExecuteAsync(NotificacionQueryHelper._Insert, new
                {
                    TipoNotificacionId = (int)tipo,
                    Descripcion = descripcion,
                    ListaDistribucionId = (int)lista,
                    EsNotificado = false,
                    FechaSistema = Constants.GetFechaServidor()
                }, CommandType.Text);

                result.success();
            }
            catch (Exception ex)
            {
                result.error("Error al crear las notificaciones, por favor revise el log.");
                LogService.SetLogError(0, "NotificacionService - CrearLogNotificacion", ex.ToString());
            }

            return result;
        }

        public async Task<ResultObject> MOVCrearNotificacionPreoperacional(Enums.TipoNotificacion tipo, string asunto, string descripcion,
            int lista)
        {
            ResultObject result = new();
            try
            {

                var resultt = await _conCore.ExecuteAsync(NotificacionQueryHelper._InsertPreoperacional, new
                {
                    Asunto = asunto,
                    Descripcion = descripcion,
                    ListaDistribucionId = lista,
                    EsNotificado = false,
                    FechaSistema = Constants.GetFechaServidor()
                }, CommandType.Text);

                result.success();
            }
            catch (Exception ex)
            {
                result.error("Error al crear las notificaciones, por favor revise el log.");
                LogService.SetLogError(0, "MOVCrearNotificacionPreoperacional - CrearLogNotificacion", ex.ToString());
            }

            return result;
        }
   /*     public async Task<ResultObject> EnviarCorreosSistemaNotificacion()
        {
            ResultObject r = new ResultObject();
            Task task = Task.Run(async () =>
            {
                try
                {
                    // se coloca un foreach ay que los servicios de automatizacion de azure no permiten ejecucion minuto a minuto 
                    // por el plan de azure


                    // trae el listado de notificaciones
                    List<NotificacionesCorreoDTO> notif =await  GetNotificacionesCorreo();

                    if (notif.Count > 0)
                    {
                        // trae la plantilla para notificar
                        PlantillaDTO plantilla = await GetPlantillaBySigla(Enums.PlantillaCorreo.P_SISTEMA.ToString());


                        // configuracion desde el correo a trabajar
                        string usuarioSmtp = CommonService.GetDetalleListaBySigla("USER").Valor;
                        string contraseniaSmtp = CommonService.GetDetalleListaBySigla("PSWD").Valor;

                        // agrupo las notificaciones por lista de distribucion
                        foreach (var notificaciones in notif.GroupBy(g => g.ListaDistribucion))
                        {

                            MailNotification _mail = new MailNotification(usuarioSmtp, contraseniaSmtp, CommonService);

                            // se adicionan las personas a notificar 
                            _mail.AddRemitente(notificaciones.Key.DetalleListaCorreo.Where(w => w.EsActivo).Select(
                                s => new ListaCorreoVM()
                                {
                                    Correo = s.Correo,
                                    TipoEnvio = s.TipoEnvio.Sigla
                                }
                                ).ToList());


                            string body = plantilla.Cuerpo;

                            // agregamos la informacion si hay varios items se mostrara 
                            body = body.Replace("{rows}", notificaciones.Select(s =>
                            plantilla.DynamicText.
                            Replace("{fecha}", s.FechaSistema.ToString(Helper.FormatoFechaHora)).
                            Replace("{descripcion}", s.Descripcion).
                            Replace("{origen}", s.TipoNotificacion.Nombre)
                            ).Aggregate((i, j) => i + j));
                            // fin de body

                            var result = _mail.SendEmail(usuarioSmtp, plantilla.Asunto.Replace("{0}", Helper.GetFechaServidor().ToString(Helper.FormatoFechaHora)), body, "", LogService);

                            if (result.Exitoso)
                                SetEsNotificadoCorreos(notificaciones.Select(s => s.NotificacionCorreoId).ToList());

                        }
                    }
                    r.success("Enviado Exitosamente");
                }
                catch (Exception exp)
                {
                    r.error(exp.Message);
                }
            });
            task.Wait();
            return r;
        }

*/

    }
}
