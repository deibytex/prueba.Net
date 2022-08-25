using Syscaf.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Portal.Models
{
    public class MetricsNew
    {
        public long Tripid { get; set; }
        public int NIdleTime { get; set; }
        public int NIdleOccurs { get; set; }
        public DateTime TripStart { get; set; }
        public int ClienteIds { get; set; }
        public DateTime FechaSistema
        {
            get
            {
                return Constants.GetFechaServidor();
            }
        }

    }
}
