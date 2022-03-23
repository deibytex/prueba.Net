using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syscaf.Data.Helpers.Auth
{
    public static class QueryHelper
    {
        public static string _QUsuarios = "SELECT TU.usuarioIdS,  TU.nombre,   TU.apellido,       TU.telefono,       TU.documento, " +
       "TU.correo,       TU.[key],       TU.IV,       TU.usuario,       TU.fechaCreacion,       TU.fechaUltimaActualizacion," +
       "TU.fechaUltimoIngreso,       TU.perfilIdS,       TU.estadoUsuarioIdS,       TU.contrasena,       TU.notificacion," +
       "TU.imagen,        TU.TokenRecuperacion,       TU.FechaExpiracion,       TU.Intentos FROM dbo.TB_Usuarios AS TU " +
       "WHERE TU.usuarioIdS = @UsuarioId";

        public static string _QUsuariosAll = "SELECT TU.usuarioIdS,  TU.nombre,   TU.apellido,       TU.telefono,       TU.documento, " +
     "TU.correo,       TU.[key],       TU.IV,       TU.usuario,       TU.fechaCreacion,       TU.fechaUltimaActualizacion," +
     "TU.fechaUltimoIngreso,       TU.perfilIdS,       TU.estadoUsuarioIdS,       TU.contrasena,       TU.notificacion," +
     "TU.imagen,        TU.TokenRecuperacion,       TU.FechaExpiracion,       TU.Intentos FROM dbo.TB_Usuarios AS TU ";
        public static string _QEventType = new string[] {
        "SELECT TET.eventTypeIdS,",
        "TET.eventTypeId,",
        "TET.clienteIdS,",
        "TET.descriptionEvent ",
        " FROM dbo.TB_EventType AS TET"
        }.ToList().Aggregate((i, j) => i + j);


    }
}
