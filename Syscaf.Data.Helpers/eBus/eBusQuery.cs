using System;
using System.Collections.Generic;
using System.Text;

namespace Syscaf.Data.Helpers.eBus.Gcp
{
    // trae los querys necesarios para la base de datos ebus 
    // modulo gcp
    public static class eBusQuery
    {
        public static string newParametrizacion = @"insert into ebus.Parametrizacion (ClienteIds, EsActivo, TipoParametroId, UltimaActualizacion, UsuarioId, Userid, Valor, FechaSistema )
                                                    values (@ClienteIds, @EsActivo,@TipoParametroId,@UltimaActualizacion, @UsuarioId, @Userid, @Valor, @FechaSistema )";
        public static string updateParametrizacion = @"UPDATE EBUS.Parametrizacion
                                                       SET ClienteIds = @ClienteIds
                                                          ,Valor = @Valor    
                                                          ,UltimaActualizacion =@UltimaActualizacion     
                                                          ,EsActivo = @EsActivo     
                                                     WHERE  ParametrizacionId = @ParametrizacionId";
        public static string updateEstadoParametrizacion = @"UPDATE EBUS.Parametrizacion
                                                       SET EsActivo = @EsActivo     
                                                     WHERE  ParametrizacionId = @ParametrizacionId";
    }
}
