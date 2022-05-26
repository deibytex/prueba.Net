using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.Portal
{
    public class DriverQueryHelper
    {
        public static string _Insert = "PORTAL.AddDrivers ";


        public static string _getByClientId = @"SELECT D.DriverId,
                                                   D.fmDriverId,
                                                   D.extendedDriverIdType,
                                                   D.employeeNumber,
                                                   D.name,
                                                   D.FechaSistema,
                                                   D.aditionalFields,
                                                   D.SiteId,
                                                   D.ClienteId FROM PORTAL.Drivers AS D
                                            WHERE(@Clienteid IS NULL OR  D.ClienteId = @Clienteid)
                                            AND(@EsActivo IS NULL OR D.EsActivo = @EsActivo)";

        public static string _getByClientIds = @"SELECT D.DriverId,
                                                           D.fmDriverId,
                                                           D.extendedDriverIdType,
                                                           D.employeeNumber,
                                                           D.name,
                                                           D.FechaSistema,
                                                           D.aditionalFields,
                                                           D.SiteId,
                                                           D.ClienteId FROM PORTAL.Drivers AS D
	                                                       INNER JOIN PORTAL.Cliente AS C ON C.ClienteId = D.ClienteId
                                                    WHERE (@Clienteids IS NULL OR  C.ClienteIds = @Clienteids)
                                                    AND (@EsActivo IS NULL OR D.EsActivo = @EsActivo)
                                                    ";
    }
}
