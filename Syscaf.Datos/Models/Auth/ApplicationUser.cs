using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public int PerfilId { get; set; }
        public string Nombres { get; set; }
        public long ClienteId { get; set; }
        public int? usuarioIdS { get; set; }
        public bool? esMigrado { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
