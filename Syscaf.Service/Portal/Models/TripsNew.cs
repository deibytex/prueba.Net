using Syscaf.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Portal.Models
{
    public class TripsNew
    {
        public long TripId { get; set; }
        public long AssetId { get; set; }
        public long DriverId { get; set; }
        public string Notes { get; set; }
        public decimal DistanceKilometers { get; set; }
        public decimal? StartOdometerKilometers { get; set; }
        public decimal? EndOdometerKilometers { get; set; }
        public decimal MaxSpeedKilometersPerHour { get; set; }
        public decimal MaxAccelerationKilometersPerHourPerSecond { get; set; }
        public decimal MaxRpm { get; set; }
        public decimal StandingTime { get; set; }
        public float? FuelUsedLitres { get; set; }
        public string StartPositionId { get; set; }
        public string EndPositionId { get; set; }
        public int? StartEngineSeconds { get; set; }
        public int? EndEngineSeconds { get; set; }
        public DateTime TripEnd { get; set; }
        public DateTime TripStart { get; set; }
        public int clienteIdS { get; set; } = 0;
        public int CantSubtrips { get; set; } = 0;
        public DateTime FechaSistema
        {
            get
            {
                return Constants.GetFechaServidor();
            }
        }

        public decimal MaxDecelerationKilometersPerHourPerSecond { get; set; }
        public int Duration { get; set; }
    }
}
