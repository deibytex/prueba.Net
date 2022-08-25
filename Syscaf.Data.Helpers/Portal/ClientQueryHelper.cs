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
           TC.clienteNombre,
           TC.fechaIngreso,
           TC.estadoClienteId,
           TC.notificacion,
           TC.GeneraIMG,
           TC.Trips,
           TC.Metrics,
           TC.Event,
           TC.Position,
           TC.ActiveEvent
        FROM  PORTAL.Cliente AS TC
        WHERE ( @clienteIdS is null OR   TC.clienteIdS = @clienteIdS  ) and ( @Estado = -1 OR   TC.estadoClienteId = @Estado  ) 
        and ( @clienteId is null  OR   TC.clienteId = @clienteId )
       ";

        public static string _Insert = @"PORTAL.AddClientes    ";

        
    }
}
