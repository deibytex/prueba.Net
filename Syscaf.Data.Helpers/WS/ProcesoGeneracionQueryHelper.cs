using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.TX
{
    public class ProcesoGeneracionQueryHelper
    {
        public static string _Get = "WS.GetListProcesoGeneracionDatos ";
        public static string _UpdateEstado = @"UPDATE WS.TB_ProcesoGeneracionDatos SET EstadoProcesoGeneracionId = @estado
                                        WHERE ProcesoGeneracionDatosId = @ProcesoGeneracionDatosId";
        public static string _InsertDetalleGeneracion = @"INSERT INTO WS.TB_DetalleProcesoGeneracionDatos
                                                            (
                                                                ProcesoGeneracionDatosId,
                                                                Descripcion,
                                                                EstadoDetallenId,
                                                                FechaSistema,
                                                                ClienteIdS
                                                            )
                                                            VALUES
                                                            (   @ProcesoGeneracionDatosId,
                                                               @Descripcion,
                                                                @EstadoDetallenId,
                                                                @FechaSistema,
                                                                @ClienteIdS
                                                                )";



    }
}
