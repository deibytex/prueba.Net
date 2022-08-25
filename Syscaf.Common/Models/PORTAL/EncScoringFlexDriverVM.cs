using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.PORTAL
{
    public class EncScoringFlexDriverVM
    {
        public string from { get; set; } 
        public string to { get; set; }
        public  string aggregationPeriod { get; set; }
        public  int ClienteIds { get; set; }
        public long ClienteId { get; set; }
    }
}
