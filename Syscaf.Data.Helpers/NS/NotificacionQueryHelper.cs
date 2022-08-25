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


        public static string _getNotificacionesSinEnviar = @"
                            SELECT TNC.NotificacionCorreoId,
                                   TNC.TipoNotificacionId,
                                   TNC.Descripcion,
                                   TNC.ListaDistribucionId
                                  FROM NS.TB_NotificacionesCorreo AS TNC
                            WHERE TNC.EsNotificado = 0
                ";
    }
}
