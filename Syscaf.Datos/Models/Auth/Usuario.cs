using System;
using System.Collections.Generic;

namespace Syscaf.Data.Models.Auth
{
    public class Usuario
    {
        public int usuarioIdS { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public bool notificacion { get; set; }
        public string documento { get; set; }
        public string correo { get; set; }
        public byte[] key { get; set; }
        public byte[] IV { get; set; }
        public string usuario { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaUltimaActualizacion { get; set; }
        public DateTime? fechaUltimoIngreso { get; set; }
        public int perfilIdS { get; set; }
        public int estadoUsuarioIdS { get; set; }
        public byte[] contrasena { get; set; }
        public Guid? TokenRecuperacion { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public int? Intentos { get; set; }
        public byte[] imagen { get; set; }
       // public virtual ICollection<RolUsuario> Roles { get; set; }
    }
}
