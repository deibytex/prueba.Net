using System;
using Syscaf.Common.Helpers;

namespace Syscaf.Service.Portal.Models
{
    public class EventsActivesNew
    {
        public long EventId { get; set; }
        public long AssetId { get; set; }
        public long DriverId { get; set; }
        public long EventTypeId { get; set; }
        public int Priority { get; set; }
        public bool Armed { get; set; }
        public DateTime EventDateTime { get; set; }
        public DateTime? ReceivedDateTime { get; set; }
        public decimal? OdometerKilometres { get; set; }
        public double? Value { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public string? ValueType { get; set; }
        public string? ValueUnits { get; set; }
        public float? SpeedLimit { get; set; }
        public DateTime FechaSistema
        {
            get
            {
                return Constants.GetFechaServidor();
            }
        }
        public bool EsActivo { get; set; }
        public bool EsProcesado { get; set; }
    }
}

