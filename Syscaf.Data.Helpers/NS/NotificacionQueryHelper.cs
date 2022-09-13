using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.Portal
{

    public static class NotificacionQueryHelper
    {
        public static string _Insert = @"INSERT INTO NS.TB_NotificacionesCorreo
                                        (
                                            TipoNotificacionId,  Descripcion,   ListaDistribucionId,    FechaSistema
                                        )
                                        VALUES
                                        (   @TipoNotificacionId,  @Descripcion,   @ListaDistribucionId,  @FechaSistema    )";

        public static string _UpdateEstado = @"
                UPDATE NS.TB_NotificacionesCorreo SET EsNotificado = 1
                WHERE NotificacionCorreoId = @Id
                ";

        public static string _InsertPreoperacional = @"INSERT INTO MOV.Notificaciones
                                        (
                                            asunto,  Descripcion,   ListaDistribucionId,    FechaSistema
                                        )
                                        VALUES
                                        (   @asunto,  @Descripcion,   @ListaDistribucionId,  @FechaSistema )";


        public static string _GetPlantillaBySigla = @"select Asunto, Cuerpo, DynamicText from dbo.TB_PlantillaCorreos
                                                            where (@Sigla is null or Sigla = @Sigla)";
        public static string _GetListaDistribucionCorreoBySigla = @"select dt.Nombres, Correo, Tipoenvioid, TipoEnvio = dl.Sigla from NS.TB_ListaDistribucionCorreo ld
                                                    inner join NS.TB_DetalleDistribucionCorreo dt on ld.listadistribucionid = dt.listadistribucionid
                                                    inner join dbo.tb_detallelistas dl on dt.tipoenvioid = dl.detallelistaid
                                                    where (ld.sigla = @sigla) and(@Clienteid is null or ld.clienteid = @Clienteid)
                                                    and dt.EsActivo = 1";

        public static string _GetListaDistribucionCorreoById = @"select dt.Nombres, Correo, Tipoenvioid, TipoEnvio = dl.Sigla from NS.TB_ListaDistribucionCorreo ld
                                                    inner join NS.TB_DetalleDistribucionCorreo dt on ld.listadistribucionid = dt.listadistribucionid
                                                    inner join dbo.tb_detallelistas dl on dt.tipoenvioid = dl.detallelistaid
                                                    where ld.Listadistribucionid = @Listadistribucionid
                                                        and dt.EsActivo = 1";



        public static string _getNotificacionesSinEnviar = @"
                            SELECT TNC.NotificacionCorreoId,
                                   TNC.TipoNotificacionId,
                                   TNC.Descripcion,
                                   TNC.ListaDistribucionId
                                  FROM NS.TB_NotificacionesCorreo AS TNC
                            WHERE TNC.EsNotificado = 0
                ";


        public static string _getNotificacionesSinEnviarMovil = @"
                            SELECT TNC.NotificacionId,
                                   TNC.Asunto,
                                   TNC.Descripcion,
                                   TNC.ListaDistribucionId
                                  FROM MOV.Notificaciones AS TNC
                            WHERE TNC.EsNotificado = 0
                ";


        public static string _UpdateEstadoMovi = @"
                UPDATE MOV.Notificaciones SET EsNotificado = 1
                WHERE NotificacionId = @Id
                ";
    }
}
