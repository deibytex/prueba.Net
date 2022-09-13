using Syscaf.Common.Helpers;
using Syscaf.Data;
using Syscaf.Data.Models;
using Syscaf.Data.Models.NS;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.Helpers;
using Syscaf.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Helper = Syscaf.Common.Helpers.Helpers;

namespace Syscaf.Common.Integrate.LogNotificaciones
{
    public interface INotificacionService
    {


        //ResultObject EnviarCorreosSistemaNotificacion();
        Task<ListaDistribucionDTO> GetListadoDistribucionBySigla(string sigla, long ClienteId);

        //// trae la plantilla segun sigla dada
        Task<PlantillaDTO> GetPlantillaBySigla(string sigla);

        //// trae todas las notificaciones sin notificar
        Task<List<NotificacionesCorreoDTO>> GetNotificacionesCorreo();        // actualiza el estado de las notificaciones a verdadero

        Task<ResultObject> SetEsNotificadoCorreos(List<long> ids);
        Task<ResultObject> CrearLogNotificacion(Enums.TipoNotificacion tipo, string descripcion, Enums.ListaDistribucion lista);

        Task<ResultObject> MOVCrearNotificacionPreoperacional(Enums.TipoNotificacion tipo, string asunto, string descripcion,
                    int lista);

        Task<ResultObject> EnviarCorreosMovilNotificacion(List<DetalleListaDTO> smtp);
        ResultObject EnviarNotificacion(List<DetalleListaDTO> smtp, string correo, string Asunto, string Cuerpo);


    }
}
