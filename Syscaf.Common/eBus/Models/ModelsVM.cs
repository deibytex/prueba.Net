using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.eBus.Models
{
    public class EventosActivosVM
    {
        public List<EventosActivosViajeVM> EventActiveViaje { get; set; }
        public List<EventosActivosRecargaVM> EventActiveRecarga { get; set; }
    }
    public class EventosActivosRecargaVM
    {
        public long EventId { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaString { get; set; }
        public long EventTypeId { get; set; }
        public double? Consecutivo { get; set; }
        public double? Carga { get; set; }
        public string AssetId { get; set; }
        public string DriverId { get; set; }
        public double? Soc { get; set; }
        public double? Corriente { get; set; }
        public double? Voltaje { get; set; }
        public double Potencia { get; set; }
        public double? Energia { get; set; }
        public double? ETA { get; set; }
        public double? Odometer { get; set; }
        public double VelocidadPromedio { get; set; }
    }
    public class EventosActivosViajeVM
    {

        public long EventId { get; set; }
        public DateTime Fecha { get; set; }
        public long EventTypeId { get; set; }
        public long AssetId { get; set; }
        public long DriverId { get; set; }
        public double? Altitud { get; set; }
        public double? EnergiaRegenerada { get; set; }
        public double? EnergiaDescargada { get; set; }
        public double? Soc { get; set; }
        public double? Energia { get; set; }
        public double? PorRegeneracion { get; set; }
        public double Distancia { get; set; }
        public string Localizacion { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public double Autonomia { get; set; }
        public double VelocidadPromedio { get; set; }
    }


    public class ListaEventosViajeVM
    {

        public DateTime Fecha { get; set; }
        public string FechaString { get; set; }
        public long EventTypeId { get; set; }
        public long AssetId { get; set; }
        public string Driver { get; set; }
        public string Placa { get; set; }
        public decimal? Altitud { get; set; }
        public decimal? EnergiaRegenerada { get; set; }
        public decimal? EnergiaDescargada { get; set; }
        public decimal? Soc { get; set; }
        public decimal? Energia { get; set; }
        public decimal? PorRegeneracion { get; set; }
        public decimal Kms { get; set; }
        public string Localizacion { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public decimal Autonomia { get; set; }
        public decimal VelocidadPromedio { get; set; }
        public decimal? Eficiencia { get; set; }
        public DateTime FechaSistema { get; set; }
        public decimal Odometro { get; set; }
    }



    public class ListaEventosRecargaVM
    {

        public DateTime Fecha { get; set; }
        public string FechaString { get; set; }
        public string TotalTime { get; set; }
        public long EventTypeId { get; set; }
        public int? Consecutivo { get; set; }
        public double? Carga { get; set; }
        public long AssetId { get; set; }
        public long DriverId { get; set; }
        public decimal? Soc { get; set; }
        public decimal? SocInicial { get; set; }
        public decimal? Corriente { get; set; }
        public decimal? Voltaje { get; set; }
        public decimal Potencia { get; set; }
        public decimal? Energia { get; set; }
        public decimal? ETA { get; set; }
        public decimal? Odometer { get; set; }
        public decimal VelocidadPromedio { get; set; }
        public string Placa { get; set; }
        public int IsDisconected { get; set; }
        public decimal? PotenciaProm { get; set; }
    }
}
