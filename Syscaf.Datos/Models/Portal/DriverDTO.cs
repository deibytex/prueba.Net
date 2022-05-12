using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    public class DriverDTO
    {
        public long DriverId { get; set; }
        public int fmDriverId { get; set; }
        public long SiteId { get; set; }
        public string name { get; set; }

        public string employeeNumber { get; set; }
        public string extendedDriverIdType { get; set; }

        public string aditionalFields { get; set; }

        public long ClienteId { get; set; }
        public DateTime FechaSistema { get; set; }

    }
}
