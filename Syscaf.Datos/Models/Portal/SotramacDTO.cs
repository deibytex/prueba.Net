using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    public class ViajesSotramacDTO
    {
      
        public long tripId { get; set; }
        public long assetId { get; set; }
        public decimal distanceKilometers { get; set; }
        public long driverId { get; set; }
        public decimal? startEngineSeconds { get; set; }
        public int? endEngineSeconds { get; set; }
        public int engineSeconds { get; set; }
        public decimal fuelUsedLitres { get; set; }
        public DateTime tripEnd { get; set; }
        public DateTime tripStart { get; set; }
        public DateTime FechaSistema { get; set; }
        public int ClienteIds { get; set; }
    }
}
