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

        
        public static Func<string,string> _guardaTablasPortalString = (tabla) => $"PORTAL.Insert{tabla}";
        public static Func<string, string> _guardaTablasPortal = (tabla) => $"PORTAL.Insert{tabla}ByPeriodAndClient";


        public static string _insertaPosiciones = "TX.InsertPosiciones";

        public static string _insertaPosicionesCliente = "PORTAL.InsertPosicionesByClient";

        public static string _listaDetalle = "PORTAL.GetDetalleLista";
        public static string DriverxCliente = "RAG.GetDriversxCliente";
        public static string OrganizacionMix = "RAG.GetClienteMix";
        public static string EncScoringDetalleScoringFlexDriver = "RAG.SetEncScoring&DetalleScoringFlexDriver";

        //Cargar eventos Activos
        public static string ActiveEvent = "PORTAL.SetActiveEvent";

        public static string getAssetsProgramacion = @"SELECT ProcesoGeneracionDatosId
                                                      ,a.AssetId
	                                                  ,pa.clienteid
                                                      ,pa.Description
                                                  FROM dbo.TB_AssetProgramacion a 
                                                  inner join portal.Assets pa on pa.AssetId =  a.AssetId
                                                  where a.EsActivo  = 1";

        public static string insertPruebasSimCard = @" INSERT INTO dbo.TB_PruebaSimCard
                                                       (Placa
                                                       , UltimoAvl
                                                       , FechaSistema
                                                       , ProcesoGeneracionDatosId
                                                       , Latitud
                                                       , Longitud
                                                       , Velocidad)
                                                 VALUES
                                                       (@Placa
                                                       , @UltimoAvl
                                                       , @FechaSistema
                                                       , @ProcesoGeneracionDatosId
                                                       , @Latitud
                                                       , @Longitud
                                                       , @Velocidad)";

        public static string getConsultasByClaseyNombre = @"  SELECT Consulta , Tipo    
                                                              FROM PORTAL.ConsultasPortalPorTipo
                                                              where Clase = @Clase and NombreConsulta = @NombreConsulta";
        public static string getTokenPorTipo = @"  select Token,ExpirationDate  from PORTAL.Tokens
                                                        where ( is null or  Tipo = @Tipo)";
        public static string setTokenPorTipo = @" update portal.token set Token = @Token, ExpirationDate = @ExpirationDate
                                                    where Tipo = @Tipo";
        public static string newTokenPorTipo = @"  insert into portal.token (Tipo,Token, ExpirationDate)
                                                    values(@Tipo,@Token,@ExpirationDate )";
    }
}
