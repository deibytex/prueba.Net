using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.ViewModels.PORTAL
{
    public class CredencialesMixVM
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }

        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string clientesId { get; set; }
    }
}
