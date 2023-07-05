using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syscaf.Common.Utils
{
    public class GlobalVariables
    {

        public string NameZone { get; set; }
        public string VersionJs { get; set; }
        public string CallsMix { get; set; }

        public string timeCache { get; set; }
        public string Dominio { get; set; }

        public string CallsMixHour { get; set; }
    }

    public class PegVariablesConn {

        public string UrlToken { get; set; }
        public string UrlRequest { get; set; }
        public string PwsPeg { get; set; }
        public string UserPeg { get; set; }
    }

    public class FreshdeskVariablesConn
    {
        public string Key { get; set; }
        public string Clave { get; set; }
        public string Dominio { get; set; }
    }
}
