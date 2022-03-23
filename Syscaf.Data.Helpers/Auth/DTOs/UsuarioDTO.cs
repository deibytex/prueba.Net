using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Helpers.Auth.DTOs
{
    public class UsuarioDTO
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int tipoPerfil { get; set; }

        public int ClienteId { get; set; }


    }
}
