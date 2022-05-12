using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    public class SiteDTO
    {
        public long SiteId { get; set; }
        public long ClienteId { get; set; }
        public string SiteName { get; set; }       
        public long? SitePadreId { get; set; }      
        public DateTime FechaSistema { get; set; }

    }
}
