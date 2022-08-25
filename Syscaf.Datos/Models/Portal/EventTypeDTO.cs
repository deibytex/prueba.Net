using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    public class EventTypeDTO
    {
        public long EventTypeId { get; set; }
        public long ClienteId { get; set; }
        public string Description { get; set; }        

        public string EventType { get; set; }

        public string DisplayUnits { get; set; }

        public string FormatType { get; set; }
        public string ValueName { get; set; }
        public DateTime FechaSistema { get; set; }

    }
}
