using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.eBus.Models
{
    public class EficienciaVM
    {
        public int EficienciaId { get; set; }
        public string Movil { get; set; }
        public string Operador { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public decimal? Carga { get; set; }
        public decimal? Descarga { get; set; }
        public decimal? Distancia { get; set; }
        public TimeSpan Duracion { get; set; }
        public decimal? DuracionHora { get; set; }
        public decimal? TotalConsumo { get; set; }
        public int? MaxSOC { get; set; }
        public int? MinSOC { get; set; }
        public int? DSOC { get; set; }
        public string RutaCodigo { get; set; }
        public string RutaNombre { get; set; }
        public DateTime fechasistema { get; set; }
        public bool EsProcesado { get; set; }
        public decimal? StartOdometer { get; set; }
        public decimal? EndOdometer { get; set; }
    }

    public class ZPVM
    {
        public int ZPId { get; set; }
        public string Movil { get; set; }
        public string Operador { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string Descripcion { get; set; }
        public TimeSpan Duracion { get; set; }
        public decimal? DuracionHora { get; set; }
        public string RutaCodigo { get; set; }
        public string RutaNombre { get; set; }
        public DateTime fechasistema { get; set; }
        public bool EsProcesado { get; set; }
    }

    public class Recargas30segVM
    {
        public int Recargas30segId { get; set; }
        public int Muestra { get; set; }
        public int NoCarga { get; set; }
        public long AssetId { get; set; }
        public string Movil { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public DateTime FechaHora { get; set; }
        public int? SOC { get; set; }
        public decimal? Corriente { get; set; }
        public int? Voltaje { get; set; }
        public DateTime fechasistema { get; set; }
        public bool EsProcesado { get; set; }
    }

    public class AlarmasVM
    {
        public int AlarmasId { get; set; }
        public string Movil { get; set; }
        public string Operador { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string Descripcion { get; set; }
        public TimeSpan Duracion { get; set; }
        public decimal? DuracionHora { get; set; }
        public string RutaCodigo { get; set; }
        public string RutaNombre { get; set; }
        public DateTime fechasistema { get; set; }
        public bool EsProcesado { get; set; }
        public decimal? Valor { get; set; }
    }

    public class Viajes1MinVM
    {
        public int Viajes1MinId { get; set; }
        public string Movil { get; set; }
        public string Operador { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public DateTime FechaHora { get; set; }
        public int SOC { get; set; }
        public decimal? CargakWh { get; set; }
        public decimal? DescargakWh { get; set; }
        public decimal? Odometro { get; set; }
        public decimal? Distancia { get; set; }
        public string RutaCodigo { get; set; }
        public string RutaNombre { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public int? Altitud { get; set; }
        public int? DeltaSOC { get; set; }
        public DateTime fechasistema { get; set; }
        public bool EsProcesado { get; set; }
    }
}