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
        SELECT TC.clienteIdS,
           TC.clienteNombre,   
           TC.fechaIngreso,
           TC.estadoClienteIdS,
           TC.clienteId,
           TC.notificacion,
           TC.GeneraIMG,
           TC.Trips,
           TC.Metrics,
           TC.Event,
           TC.Position,
           TC.ActiveEvent 
        FROM dbo.TB_Cliente AS TC
        WHERE ( @Estado = -1 OR   TC.estadoClienteIdS = @Estado  )  
       ";
    }
}
