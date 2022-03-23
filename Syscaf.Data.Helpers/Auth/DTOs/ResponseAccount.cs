using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.Auth.DTOs
{
    public class ResponseAccount
    {
       
            public string Token { get; set; }
            public DateTime Expiracion { get; set; }
     
    }
}
