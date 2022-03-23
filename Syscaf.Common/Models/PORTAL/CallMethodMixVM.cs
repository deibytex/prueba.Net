using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.ViewModels.PORTAL
{
   public  class CallMethodMixVM
    {
        public int ClienteCredencialId { get; set; }

        public int TotalCallsHour { get; set; }
        public int TotalCalls { get; set; }

        public DateTime? DateCall { get; set; }
        public DateTime? dateHour { get; set; }
        public int HourCall { get; set; }
        public int MinuteCall { get; set; }

        public int SecondMinute { get; set; }

        public int SecondHour { get; set; }

    }
}
