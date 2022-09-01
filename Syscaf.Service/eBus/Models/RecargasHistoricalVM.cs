using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.eBus.Models
{
    public class RecargasHistoricalVM
    {
        public int RecargasHistoricalId { get; set; }
        public long assetId { get; set; }
        public string Movil { get; set; }
        public DateTime FechaHoraInicioCarga { get; set; }
        public DateTime FechaInicioCarga { get; set; }
        public TimeSpan HoraInicioCarga { get; set; }
        public int HoraEnteroInicioCarga { get; set; }
        public DateTime FechaHoraFinCarga { get; set; }
        public DateTime FechaFinCarga { get; set; }
        public TimeSpan HoraFinCarga { get; set; }
        public int HoraEnteroFinCarga { get; set; }
        public DateTime FechaCorte { get; set; }
        public int? SOCInicial { get; set; }
        public int? SOC { get; set; }
        public double Energia { get; set; }
        public double PotenciaPromedio { get; set; }
        public TimeSpan Duracion { get; set; }
        public double DuracionDecimal { get; set; }
        public double Odometro { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public string Ubicacion { get; set; }
        public DateTime fechaSistema { get; set; }
        public bool EsProcesado { get; set; }
        public long EventId { get; set; }
    }
}
