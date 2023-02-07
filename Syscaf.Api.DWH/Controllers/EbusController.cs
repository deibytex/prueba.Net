using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Common.eBus.Models;
using Syscaf.Data;
using Syscaf.Service.eBus;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;

namespace Syscaf.ApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EbusController : ControllerBase
    {
        private readonly IEBusService _ebusService;
        private readonly SyscafCoreConn _connCore;
        public EbusController(IEBusService ebusService, SyscafCoreConn _connCore)
        {
            this._ebusService = ebusService;
            this._connCore = _connCore;
        }
        /// <summary>
        /// Se setean los campos de la tabla de configuracion de la data table por usuario y tabla
        /// </summary>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        [HttpPost("SetColumnasDatatable")]
        public async Task<ResultObject> SetColumnasDatatable([FromBody] ConfiguracionDatatableVM Modelo)
        {
            return await _ebusService.SetColumnasDatatable(Modelo);
        }
        /// <summary>
        /// Se consultan las columnnas de las tablas guardadas
        /// </summary>
        /// <param name="OpcionId"></param>
        /// <param name="UsuarioIds"></param>
        /// <param name="IdTabla"></param>
        /// <returns></returns>
        [HttpPost("GetColumnasDatatable")]
        public async Task<List<Object>> GetColumnasDatatable(int OpcionId, int UsuarioIds, string IdTabla)
        {
            return await _ebusService.GetColumnasDatatable(OpcionId, UsuarioIds, IdTabla);
        }
    }
}
