using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.Auth.DTOs
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Nombres { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PerfilId { get; set; }
        public long ClienteId { get; set; }
        public int? usuarioIdS { get; set; }
        public bool? esMigrado { get; set; }

        public bool lockoutEnabled { get; set; }
    }

    public class ResetPassWord
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }
        public string token { get; set; }
        public bool EmailConfirm { get; set; }
    }
}
