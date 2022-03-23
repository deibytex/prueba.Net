using Syscaf.Common.Helpers;
using Syscaf.Data;
using Syscaf.Data.Models;
using Syscaf.Service.Helpers;
using Syscaf.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Helper = Syscaf.Common.Helpers.Helpers;

namespace Syscaf.Service
{
    public  class NotificacionService: INotificacionService
    {
        readonly ILogService LogService;
        readonly ICommonService CommonService;
        
        // nos trae la infomación de los assets junto a sus clientesreadonly I_LogService __LogService;
        public NotificacionService(ILogService LogService, ICommonService CommonService) {
            this.LogService = LogService;
            this.CommonService = CommonService;
        }

        public  ResultObject EnviarCorreosSistemaNotificacion()
        {
            ResultObject r = new ResultObject();
            Task task = Task.Run(() => {
            try
            {
                // se coloca un foreach ay que los servicios de automatizacion de azure no permiten ejecucion minuto a minuto 
                // por el plan de azure

               
                                        // trae el listado de notificaciones
                    List<NotificacionesCorreo> notif = GetNotificacionesCorreo();

                    if (notif.Count > 0)
                    {
                        // trae la plantilla para notificar
                        PlantillaCorreo plantilla = GetPlantillaBySigla(Enums.PlantillaCorreo.P_SISTEMA.ToString());


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


        public  ListaDistribucion GetListadoDistribucionBySigla(string sigla)

        {
            ListaDistribucion lista = null;
            try
            {

                using (SyscafBD ctx = new SyscafBD())
                {
                    lista = ctx.ListaDistribucionCorreo
                        .Include(i => i.DetalleListaCorreo)
                        .Where(w => w.Sigla.Equals(sigla, Constants.comparer)).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {

                LogService.SetLog(ex.ToString(), "", "NotificacionService - GetListadoDistribucionBySigla ");
            }

            return lista;
        }

        // trae la plantilla segun sigla dada

        public  PlantillaCorreo GetPlantillaBySigla(string sigla)

        {
            PlantillaCorreo plantilla = null;
            try
            {
                using (SyscafBD ctx = new SyscafBD())
                {
                    plantilla = ctx.PlantillaCorreo.Where(w => w.Sigla.Equals(sigla, Constants.comparer)).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                LogService.SetLog(ex.ToString(), "", "NotificacionService - GetPlantillaBySigla ");
            }


            return plantilla ?? new PlantillaCorreo();


        }



        // trae todas las notificaciones sin notificar
        public  List<NotificacionesCorreo> GetNotificacionesCorreo()
        {
            List<NotificacionesCorreo> notificaciones = null;
            try
            {
                using (SyscafBD ctx = new SyscafBD())
                {

                    notificaciones = ctx.NotificacionesCorreo
                        .Where(w => !w.EsNotificado && w.EsActivo)
                        .Include(x => x.ListaDistribucion)
                        .Include(x => x.ListaDistribucion.DetalleListaCorreo)
                        .Include(x => x.ListaDistribucion.DetalleListaCorreo.Select(s => s.TipoEnvio))
                        .Include(x => x.TipoNotificacion).ToList();

                }

            }
            catch (Exception ex)
            {
                LogService.SetLog(ex.ToString(), "GetNotificacionesCorreo", "NotificacionService - GetPlantillaBySigla ");
            }

            return notificaciones;

        }


        // actualiza el estado de las notificaciones a verdadero

        public  ResultObject SetEsNotificadoCorreos(List<long> ids)
        {
            ResultObject result = new ResultObject();
            try
            {
                using (SyscafBD ctx = new SyscafBD())
                {

                    foreach (int id in ids)
                    {

                        var item = ctx.NotificacionesCorreo.Find(id);
                        if (item != null)
                        {
                            var entry = ctx.Entry(item);
                            item.EsNotificado = true;
                            entry.Property(p => p.EsNotificado).IsModified = true;

                        }
                    }

                    ctx.SaveChanges();

                }
                result.Exitoso = true;
            }
            catch (Exception ex)
            {
                result.Exitoso = false;
                result.Mensaje = "Error al actualizar las notificaciones, por favor revise el log.";
                LogService.SetLog(ex.ToString(), "", "NotificacionService - SetEsNotificadoCorreos");
            }

            return result;

        }




        public  ResultObject CrearLogNotificacion(Enums.TipoNotificacion tipo, string descripcion, Enums.ListaDistribucion lista)
        {
            ResultObject result = new ResultObject();
            try
            {
                using (SyscafBD ctx = new SyscafBD())
                {

                    ctx.NotificacionesCorreo.Add(
                        new NotificacionesCorreo()
                        {
                            TipoNotificacionId = CommonService.GetDetalleListaBySigla(tipo.ToString()).DetalleListaId,
                            Descripcion = descripcion,
                            ListaDistribucionId = GetListadoDistribucionBySigla(lista.ToString()).ListaDistribucionId,
                            EsNotificado = false,
                            FechaSistema = Helper.GetFechaServidor()
                        }
                        );

                    ctx.SaveChanges();
                }
                result.Exitoso = true;
            }
            catch (Exception ex)
            {
                result.Exitoso = false;
                result.Mensaje = "Error al crear las notificaciones, por favor revise el log.";
                LogService.SetLog(ex.ToString(), "", "NotificacionService - CrearLogNotificacion");
            }

            return result;
        }



    }
}
