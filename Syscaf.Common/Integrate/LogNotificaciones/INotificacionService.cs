using Syscaf.Common.Helpers;
using Syscaf.Data;
using Syscaf.Data.Models;
using Syscaf.Service.Helpers;
using Syscaf.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Helper = Syscaf.Common.Helpers.Helpers;

namespace Syscaf.Service
{
    public interface INotificacionService
    {


        ResultObject EnviarCorreosSistemaNotificacion();
        ListaDistribucion GetListadoDistribucionBySigla(string sigla);

        // trae la plantilla segun sigla dada
        PlantillaCorreo GetPlantillaBySigla(string sigla);

        // trae todas las notificaciones sin notificar
        List<NotificacionesCorreo> GetNotificacionesCorreo();        // actualiza el estado de las notificaciones a verdadero

        ResultObject SetEsNotificadoCorreos(List<long> ids);
        ResultObject CrearLogNotificacion(Enums.TipoNotificacion tipo, string descripcion, Enums.ListaDistribucion lista);
     



    }
}
