using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.Movil
{
    public static class MovilQueryHelper
    {
        public static string _Insert = "MOV.InsertarRespuestas";
        public static string _GetRespuestas = "MOV.SP_RespuestasPreoperacional";
        public static string _GetPreguntas = "MOV.SP_PreguntasPreoperacional";

        public static string _PREOPQueryHelper = "PREOPQueryHelper";
        public static string _MOVQueryHelper = "MOVQueryHelper";

        public static string _getConsolidadoReportesPorTipo = "GetConsolidadoReportesPorTipo";
        public static string _getInformeViajesVsPreoperacional = "GetInformeViajesVsPreopreracional";
    }
}
