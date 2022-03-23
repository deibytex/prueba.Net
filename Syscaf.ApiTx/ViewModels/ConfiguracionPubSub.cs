using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syscaf.ApiTx.ViewModels
{
    public class ConfiguracionPubSub
    {
        public string TOKEN { get; set; }
        public string AUD { get; set; }
        public string AZP { get; set; }
        public string ISSUER { get; set; }
        public string EMAIL { get; set; }
        public bool VERIFICA_TOKEN { get; set; }
        public bool VERIFICA_AUTHORIZATION { get; set; }
        
    }
}
