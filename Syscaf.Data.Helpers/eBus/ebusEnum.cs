using System;
using System.Collections.Generic;
using System.Text;

namespace Syscaf.Data.Helpers.eBus
{
    public class ebusEnum
    {
        public enum TipoBus { 
         A, // articulado
         B, // BiArticulado
         M, // MicroBus
         P, // Padron
         T  // Buseton
        }
        public enum TipoTrama { 
        Periodica = 1,
        Evento = 2,
        Alarma = 3,
        Configuracion = 4
        }


        public enum TecnologiaMotor { 
            Diesel = 1,
            GNV = 2,
            Electrico = 3,
            Hibrido = 4
        }

        public enum TipoFreno { 
        
            Freno_Bandas  = 1,
            Freno_Pastillas= 2
        }
        public enum TipoParametro
        {
            Tiempos_Actualizacion = 67,
            Soc_MAx = 71,
            Pistola_Conectada = 72,
            Pistola_Desconectada = 73
        }
    }
}
