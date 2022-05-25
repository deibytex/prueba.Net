using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.Portal
{

    public static class AssetsQueryHelper
    {
        public static string _Insert = "PORTAL.AddAssets ";
        public static string _get = "PORTAL.getAssets";
        public static string _setEstado = "PORTAL.setEstadoAssets";


        public static string _getByEstado = @"SELECT A.AssetId,
                                               A.AssetTypeId,
                                               A.Description,
                                               A.RegistrationNumber,
                                               A.SiteId,
                                               A.ClienteId,    
                                               A.UserState
                                           FROM PORTAL.Assets AS A
                                   WHERE  (@ClienteId IS NULL OR A.ClienteId = @ClienteId ) AND(@userstate IS NULL OR  A.UserState = @userstate) 
            ";
    }
}
