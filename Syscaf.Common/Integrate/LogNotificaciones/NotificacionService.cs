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

namespace Syscaf.Common.Integrate.LogNotificaciones
{
    public class NotificacionService : INotificacionService
    {
        readonly ILogService LogService;
        readonly ISyscafConn _con;

        // nos trae la infomación de los assets junto a sus clientesreadonly I_LogService __LogService;
        public NotificacionService(ILogService LogService, ICommonService CommonService, ISyscafConn _con)
        {
            this.LogService = LogService;
            this._con = _con;
            // this.CommonService = CommonService;
        }

        //public  ResultObject EnviarCorreosSistemaNotificacion()
        //{
        //    ResultObject r = new ResultObject();
        //    Task task = Task.Run(() => {
        //    try
        //    {
        //        // se coloca un foreach ay que los servicios de automatizacion de azure no permiten ejecucion minuto a minuto 
        //        // por el plan de azure


        //                                // trae el listado de notificaciones
        //            List<NotificacionesCorreo> notif = GetNotificacionesCorreo();

        //            if (notif.Count > 0)
        //            {
        //                // trae la plantilla para notificar
        //                PlantillaCorreo plantilla = GetPlantillaBySigla(Enums.PlantillaCorreo.P_SISTEMA.ToString());


        //                // configuracion desde el correo a trabajar
        //                string usuarioSmtp = CommonService.GetDetalleListaBySigla("USER").Valor;
        //                string contraseniaSmtp = CommonService.GetDetalleListaBySigla("PSWD").Valor;

        //                // agrupo las notificaciones por lista de distribucion
        //                foreach (var notificaciones in notif.GroupBy(g => g.ListaDistribucion))
        //                {

        //                    MailNotification _mail = new MailNotification(usuarioSmtp, contraseniaSmtp, CommonService);

        //                    // se adicionan las personas a notificar 
        //                    _mail.AddRemitente(notificaciones.Key.DetalleListaCorreo.Where(w => w.EsActivo).Select(
        //                        s => new ListaCorreoVM()
        //                        {
        //                            Correo = s.Correo,
        //                            TipoEnvio = s.TipoEnvio.Sigla
        //                        }
        //                        ).ToList());


        //                    string body = plantilla.Cuerpo;

        //                    // agregamos la informacion si hay varios items se mostrara 
        //                    body = body.Replace("{rows}", notificaciones.Select(s =>
        //                    plantilla.DynamicText.
        //                    Replace("{fecha}", s.FechaSistema.ToString(Helper.FormatoFechaHora)).
        //                    Replace("{descripcion}", s.Descripcion).
        //                    Replace("{origen}", s.TipoNotificacion.Nombre)
        //                    ).Aggregate((i, j) => i + j));
        //                    // fin de body

        //                    var result = _mail.SendEmail(usuarioSmtp, plantilla.Asunto.Replace("{0}", Helper.GetFechaServidor().ToString(Helper.FormatoFechaHora)), body, "", LogService);

        //                    if (result.Exitoso)
        //                        SetEsNotificadoCorreos(notificaciones.Select(s => s.NotificacionCorreoId).ToList());

        //                }
        //            }
        //             r.success("Enviado Exitosamente"); 
        //        }
        //        catch (Exception exp)
        //        {
        //            r.error(exp.Message);
        //        }
        //    });
        //    task.Wait();
        //    return r;
        //}


        //public  ListaDistribucion GetListadoDistribucionBySigla(string sigla)

        //{
        //    ListaDistribucion lista = null;
        //    try
        //    {

        //        using (SyscafBD ctx = new SyscafBD())
        //        {
        //            lista = ctx.ListaDistribucionCorreo
        //                .Include(i => i.DetalleListaCorreo)
        //                .Where(w => w.Sigla.Equals(sigla, Constants.comparer)).FirstOrDefault();

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        LogService.SetLog(ex.ToString(), "", "NotificacionService - GetListadoDistribucionBySigla ");
        //    }

        //    return lista;
        //}

        //// trae la plantilla segun sigla dada

        //public  PlantillaCorreo GetPlantillaBySigla(string sigla)

        //{
        //    PlantillaCorreo plantilla = null;
        //    try
        //    {
        //        using (SyscafBD ctx = new SyscafBD())
        //        {
        //            plantilla = ctx.PlantillaCorreo.Where(w => w.Sigla.Equals(sigla, Constants.comparer)).FirstOrDefault();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        LogService.SetLog(ex.ToString(), "", "NotificacionService - GetPlantillaBySigla ");
        //    }


        //    return plantilla ?? new PlantillaCorreo();


        //}



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
                var resultt = await Task.FromResult(_con.Execute(NotificacionQueryHelper._UpdateEstado, new
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

                var resultt = await Task.FromResult(_con.Execute(NotificacionQueryHelper._Insert, new
                {
                    TipoNotificacionId = (int)tipo,
                    Descripcion = descripcion,
                    ListaDistribucionId = (int)lista,
                    EsNotificado = false,
                    FechaSistema = Constants.GetFechaServidor()
                }));

                result.success();
            }
            catch (Exception ex)
            {
                result.error("Error al crear las notificaciones, por favor revise el log.");
                LogService.SetLogError(0, "NotificacionService - CrearLogNotificacion", ex.ToString());
            }

            return result;
        }



    }
}
