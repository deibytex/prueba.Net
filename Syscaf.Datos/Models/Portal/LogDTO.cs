using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    public  class LogDTO
    {
        public string UserId { get; set; }
        public string Level { get; set; }
        public int? OptionId { get; set; }      
        public string Method { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
