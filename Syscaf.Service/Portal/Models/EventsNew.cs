using Syscaf.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Portal.Models
{
    public class EventsNew
    {
        public long EventId { get; set; }
        public long AssetId { get; set; }
        public long DriverId { get; set; }
        public long EventTypeId { get; set; }
        public int TotalTimeSeconds { get; set; }
        public int TotalOccurances { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public decimal? FuelUsedLitres { get; set; }
        public decimal? Value { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? StartOdometerKilometres { get; set; }
        public decimal? EndOdometerKilometres { get; set; }
        public int? AltitudMeters { get; set; }
        public int? ClienteIds { get; set; }
        public bool isebus { get; set; }     
        public DateTime FechaSistema
        {
            get
            {
                return Constants.GetFechaServidor();
            }
        }
        public string MediaUrls { get; set; }
        public int? SpeedKilometresPerHour { get; set; }

    }
}
