using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Portal.Models.RAG
{
    public class SafetyEventosVM
    {
        public int SafetyId { get; set; }
        public string Movil { get; set; }
        public string Operador { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime? Fin { get; set; }
        public string Descripcion { get; set; }
        public TimeSpan Duracion { get; set; }
        public decimal? DuracionHora { get; set; }
        public string RutaCodigo { get; set; }
        public string RutaNombre { get; set; }
        public DateTime fechasistema { get; set; }
        public bool EsProcesado { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? FechaFin { get; set; }
        public TimeSpan? HoraInicial { get; set; }
        public TimeSpan? HoraFinal { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public decimal? StartOdo { get; set; }
        public int? mes { get; set; }
        public int? anio { get; set; }
    }
    public class SafetyVM 
    {
        public long tripId { get; set; }
        public string Asset { get; set; }
        public string Driver { get; set; }
        public string Site { get; set; }
        public double TripsMaxSpeed { get; set; }
        public double TripsDrivingTime { get; set; }
        public double TripsDuration { get; set; }
        public double TripsDistance { get; set; }
        public int TripsCount { get; set; }
        public DateTime Period { get; set; }
        public DateTime tripStart { get; set; }
        public DateTime tripEnd { get; set; }
        public double AceleracionBrusca_8_EventDuration { get; set; }
        public double AceleracionBrusca_8_EventMaxValue { get; set; }
        public double AceleracionBrusca_8_EventOccurrences { get; set; }
        public double FrenadaBrusca_10_EventDuration { get; set; }
        public double FrenadaBrusca_10_EventMaxValue { get; set; }
        public double FrenadaBrusca_10_EventOccurrences { get; set; }
        public double ExcesoVelocidad_50_EventDuration { get; set; }
        public double ExcesoVelocidad_50_EventMaxValue { get; set; }
        public double ExcesoVelocidad_50_EventOccurrences { get; set; }
        public double GiroBrusco_EventDuration { get; set; }
        public double GiroBrusco_EventMaxValue { get; set; }
        public double GiroBrusco_EventOccurrences { get; set; }
        public double ExcesoVelocidad_30_EventDuration { get; set; }
        public double ExcesoVelocidad_30_EventMaxValue { get; set; }
        public double ExcesoVelocidad_30_EventOccurrences { get; set; }
        public int? mes { get; set; }
        public int? anio { get; set; }
    }
   
}
