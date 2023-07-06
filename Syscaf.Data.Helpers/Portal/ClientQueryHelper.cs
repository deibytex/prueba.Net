using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.Portal
{

    public static class ClientQueryHelper
    {
        public static string _Get = @"
        SELECT 
           TC.ClienteIdS,
           TC.ClienteId,
           clienteNombre = isnull(TC.NombreNormalizado, TC.clienteNombre),
           TC.fechaIngreso,
           TC.estadoClienteId,
           TC.notificacion,
           TC.GeneraIMG,
           TC.Trips,
           TC.Metrics,
           TC.Event,
           TC.Position,
           TC.ActiveEvent,
           ClienteIdString = CAST(TC.ClienteId AS VARCHAR(100))
        FROM  PORTAL.Cliente AS TC
        WHERE ( @clienteIdS is null OR   TC.clienteIdS = @clienteIdS  ) and ( @Estado = -1 OR   TC.estadoClienteId = @Estado  ) 
        and ( @clienteId is null  OR   TC.clienteId = @clienteId )
        order by isnull(TC.NombreNormalizado, TC.clienteNombre)
       ";

        public static string _Insert = @"PORTAL.AddClientes    ";

        
    }
}
