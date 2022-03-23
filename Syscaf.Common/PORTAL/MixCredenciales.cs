using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.PORTAL
{
    public class MixCredenciales
    {
        public string IdentityServerBaseAddress { get; set; }
        public string IdentityServerClientId { get; set; }
        public string IdentityServerClientSecret { get; set; }
        public string IdentityServerScopes { get; set; }
        public string IdentityServerUserName { get; set; }
        public string IdentityServerPassword { get; set; }
        public string ApiUrl { get; set; }
    }
    
}
