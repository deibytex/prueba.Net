using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.Portal
{
    public static class PortalQueryHelper
    {
        public static string _InsertLog = @"  INSERT INTO PORTAL.Log
	   (  Level,	       OptionId,	       Method,	       Description,	       Date   )
	   VALUES
	   (       @Level,  @OptionId, @Method,   @Description,   @Date	       )";

		public static string _SelectPreferenciasDescargas = @"SELECT TPDW.PreferenciasIdS,
                                   TPDW.clienteIdS,
                                   TPDW.eventTypeIdS,
                                   TPDW.usuarioIdS,
                                   TPDW.EsActivo,
                                   TPDW.FechaSistema,
                                   TPDW.TipoPreferencia,
                                   TPDW.EventTypeId,
                                   TPDW.ClientesId,
                                   TPDW.isActive,
                                   TPDW.Parametrizacion FROM dbo.TB_PreferenciasDescargarWS AS TPDW";

        public static string _verificaIdExistentes = "PORTAL.VerifyDataStageByPeriodAndClient";

        
        public static Func<string,string> _guardaTablasPortal = (tabla) => $"PORTAL.Insert{tabla}ByPeriodAndClient";

    }
}
