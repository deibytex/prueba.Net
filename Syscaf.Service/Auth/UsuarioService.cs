using Syscaf.Data;
using Syscaf.Service.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Auth
{
    public class UsuarioService : IUsuarioService
    {
        private readonly SyscafCoreConn _conn;
        public UsuarioService(SyscafCoreConn _conn)
        {
            this._conn = _conn;
        }
        // --UsuarioIdentity

        public async Task<List<dynamic>> GetUsuarios(string? UserId, int? PerfilId)
        {

            return await _conn.GetAll("ADM.GetAspnetUsers", new { UserId  , PerfilId });
        }
        public async Task<List<MenuDesagregadoDTO>> GetMenuUsuario(string UserId)
        {

            return await _conn.GetAll<MenuDesagregadoDTO>("ADM.GetMenuUsuario", new { UserId });
        }
    }

    public interface IUsuarioService
    {
        Task<List<dynamic>> GetUsuarios(string? UserId, int? PerfilId);
         Task<List<MenuDesagregadoDTO>> GetMenuUsuario(string UserId);
    }
}
