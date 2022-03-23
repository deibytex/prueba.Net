using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Syscaf.Data.Helpers.Auth;
using Syscaf.Data.Interface;
using Syscaf.Data.Models.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Syscaf.ApiCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TxController : ControllerBase
    {
        private readonly ISyscafConn _conn;
        public TxController(ISyscafConn conn)
        {
            _conn = conn;
        }
        [HttpGet]
        public  string Get()
        {           
            return "Inicializado Correctamente....";
        }

        [HttpGet(nameof(GetById))]
        public async Task<List<Usuario>> GetById(int Id)
        {            
            var parametros = new Dapper.DynamicParameters();
            parametros.Add("UsuarioId", Id, DbType.Int32);
            var result = await Task.FromResult(_conn.GetAll<Usuario>(QueryHelper._QUsuarios, parametros, commandType: CommandType.Text));
            return result;
        }
        [HttpGet(nameof(GetEventType ))]
        public async Task<List<EventTypeId>> GetEventType()
        {
            var result = await Task.FromResult(_conn.GetAll<EventTypeId>(QueryHelper._QEventType, null, commandType: CommandType.Text));
            return result;
        }

    }
}
