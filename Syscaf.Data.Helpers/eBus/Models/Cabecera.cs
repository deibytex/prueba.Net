using System;
using System.Collections.Generic;
using System.Text;

namespace Syscaf.Data.Helpers.eBus.Models
{
    public class Cabecera
    {
        public string versionTrama { get; set; }
        public int idRegistro { get; set; }
        public string idOperador { get; set; }
        public string idVehiculo { get; set; }
        public string idRuta { get; set; }
        public string idConductor { get; set; }
        public DateTime fechaHoraLecturaDato { get; set; }
        public DateTime fechaHoraEnvioDato { get; set; }
        public string tipoBus { get; set; }
        public Localizacion localizacionVehiculo { get; set; }
        public int tipoTrama { get; set; }
        public string codigoPeriodica { get; set; }
        public int tecnologiaMotor { get; set; }
        public bool tramaRetransmitida { get; set; }
        public int tipoFreno { get; set; }

       

     
     
    }

    public class Localizacion
    {
        public double latitud { get; set; }
        public double longitud { get; set; }
    }

    public class P20 {
               
        public double velocidadVehiculo { get; set; }
        public double aceleracionVehiculo { get; set; }
    }

    public class P60
    {
        //P60
        public double temperaturaMotor { get; set; }
        /*"temperaturaMotor",
            "presionAceiteMotor",
            "velocidadVehiculo",
            "aceleracionVehiculo",
            "revolucionesMotor",
            "estadoDesgasteFrenos",
            "kilometrosOdometro",
            "consumoCombustible",
            "nivelTanqueCombustible",
            "consumoEnergia",
            "regeneracionEnergia",
            "nivelRestanteEnergia",
            "porcentajeEnergiaGenerada",
            "temperaturaSts",
            "usoCpuSts",
            "memRamSts",
            "memDiscoSts",
            "temperaturaBaterias",
            "sentidoMarcha"*/
        // para alarmas
    }

    public class Eventos { 
    
    }


    public class Alarma
    {
        public string codigoAlarma { get; set; }
        public string nivelAlarma { get; set; }

        //ALA9
        public bool estadoInfoEntretenimiento { get; set; }

    }


}
