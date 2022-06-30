using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Portal.Models.RAG
{
    public class SafetyVM
    {
        public string Driver { get; set; }
        public string Site { get; set; }
        public double TripsMaxSpeed { get; set; }
        public double TripsDrivingTime { get; set; }
        public double TripsDuration { get; set; }
        public double TripsDistance { get; set; }
        public int TripsCount { get; set; }
        public DateTime Period { get; set; }
        public double AceleracionBrusca_8_EventDuration { get; set; }
        public double AceleracionBrusca_8_EventMaxValue { get; set; }
        public double AceleracionBrusca_8_EventMinValue { get; set; }
        public double AceleracionBrusca_8_EventOccurrences { get; set; }
        public double FrenadaBrusca_10_EventDuration { get; set; }
        public double FrenadaBrusca_10_EventMaxValue { get; set; }
        public double FrenadaBrusca_10_EventMinValue { get; set; }
        public double FrenadaBrusca_10_EventOccurrences { get; set; }
        public double ExcesoVelocidad_50_EventDuration { get; set; }
        public double ExcesoVelocidad_50_EventMaxValue { get; set; }
        public double ExcesoVelocidad_50_EventMinValue { get; set; }
        public double ExcesoVelocidad_50_EventOccurrences { get; set; }
        public double GiroBrusco_EventDuration { get; set; }
        public double GiroBrusco_EventMaxValue { get; set; }
        public double GiroBrusco_EventMinValue { get; set; }
        public double GiroBrusco_EventOccurrences { get; set; }
        public double ExcesoVelocidad_30_EventDuration { get; set; }
        public double ExcesoVelocidad_30_EventMaxValue { get; set; }
        public double ExcesoVelocidad_30_EventMinValue { get; set; }
        public double ExcesoVelocidad_30_EventOccurrences { get; set; }
    }
}
