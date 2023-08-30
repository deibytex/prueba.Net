using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Syscaf.Data;
using Syscaf.Data.Helpers.Auth;

using Syscaf.Data.Models.Auth;
using Syscaf.Service.FreshDesk;
using Syscaf.Service.Helpers;
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
        private readonly SyscafCoreConn _conn;
        private readonly IFreshDeskService _FreshDesk;
        public TxController(SyscafCoreConn conn, IFreshDeskService freshDesk)
        {
            _conn = conn;
            _FreshDesk = freshDesk;
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
        /// <summary>
        /// Se obtienen todos los tickets desde Freshdesk
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTicketsFreshDesk")]
        public async Task<ResultObject> GetTicketsFreshDesk()
        {
            return await _FreshDesk.GetTickets();
        }

        [HttpGet("GetCamposTicketsFreshDesk")]
        public async Task<ResultObject> GetCamposTicketsFreshDesk()
        {
            return await _FreshDesk.GetTicketsCampos();
        }

        [HttpGet("GetAgentes")]
        public async Task<ResultObject> GetAgentes()
        {
            return await _FreshDesk.GetListAgentes();
        }


      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <returns></returns>

        [HttpGet("GetTicketsFreshDeskSemana")]
        public async Task<ResultObject> GetTicketsFreshDeskSemana(DateTime FechaInicial, DateTime FechaFinal)
        {
            return await _FreshDesk.GetListaTicketsSemana(FechaInicial, FechaFinal);
        }
        /// <summary>
        /// Para setear los tickets.
        /// </summary>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <returns></returns>
        [HttpGet("SetTickets")]
        public async Task<ResultObject> SetTickets(DateTime FechaInicial, DateTime FechaFinal)
        {
            return await _FreshDesk.SetListaTickets(FechaInicial, FechaFinal);
        }

        
    }
}
