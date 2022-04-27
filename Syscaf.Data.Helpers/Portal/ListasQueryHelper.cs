using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.Portal
{

    public static class ListasQueryHelper
    {
        /*
         Parametros
        @siglasPadre = Sigla de la Lista, puede ser null
        @siglas = Sigla del detalle lista si se quiere filtar por una en especifico                            
         */
        public static string _GetDeatlleListaBySiglas = @"
        SELECT    
          TDL.DetalleListaId,
            TDL.ListaId,
            TDL.Nombre,
            TDL.Valor,
            TDL.Sigla,
            TDL.FechaSistema,
            TDL.EsActivo 
        FROM dbo.TB_Listas AS TL
        INNER JOIN dbo.TB_DetalleListas AS TDL ON TDL.ListaId = TL.ListaId
        WHERE (@siglasPadre IS NULL OR TL.Sigla = @siglasPadre) 
        AND (@siglas IS  NULL or TDL.Sigla like @siglas +'%')
        AND TDL.EsActivo = 1 AND TL.EsActivo = 1
       ";

        /*
         Parametros
      
        @siglas = Sigla de la lista  permite null     
         @ListaId = ListaId de la lista  permite null   
         @EsActivo = EsActivo de la lista  permite null   
         */
        public static string _GetLista = @"
        SELECT    
           TL.ListaId,
           TL.Nombre,
           TL.Descripcion,
           TL.Sigla,
           TL.FechaSistema,
           TL.EsActivo 
        FROM dbo.TB_Listas AS TL	
        WHERE (@siglas IS NULL OR TL.Sigla = @siglas) 
        AND (@ListaId IS  NULL or TL.ListaId  = @ListaId)
        AND (@EsActivo is null or TL.EsActivo  = @EsActivo)
       ";


        /*
       Parametros

      @siglas = Sigla del detallelista  permite null     
       @ListaId = ListaId de la lista  permite null   
       @EsActivo = EsActivo del detallelista  permite null   
       */
        public static string _GetDetalleLista = @"
        SELECT 
            TDL.DetalleListaId,
            TDL.ListaId,
            TDL.Nombre,
            TDL.Valor,
            TDL.Sigla,
            TDL.FechaSistema,
            TDL.EsActivo 
        FROM dbo.TB_DetalleListas AS TDL	
        WHERE (@siglas IS NULL OR TDL.Sigla like @siglas + '%') 
        AND (@ListaId IS  NULL or TDL.ListaId  = @ListaId)
        AND (@EsActivo is null or TDL.EsActivo  = @EsActivo)
       ";
    }
}
