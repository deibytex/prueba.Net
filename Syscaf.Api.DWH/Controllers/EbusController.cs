using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Api.DWH.Controllers;
using Syscaf.Common.eBus.Models;
using Syscaf.Common.Models;
using Syscaf.Data;
using Syscaf.Service.eBus;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;
using System.Net;

namespace Syscaf.ApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EbusController : BaseController
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
        /// <summary>
        /// Trae los clientes que pertenecen a ESOMOS
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetClientesUsuarios")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ResultObject> GetClientesUsuarios()
        {
            string Usuario = this.UserId;
            return await _ebusService.GetClientesUsuarios(null, Usuario);
            
        }
        /// <summary>
        /// trae los viajes por cliente en un periodo determinado
        /// </summary>
        /// <param name="clienteids"></param>
        /// <param name="period"></param>
        /// <param name="command"></param>
        /// <returns></returns>

        [HttpPost("GetEventosActivosByClienteIds")]
        public List<ListaEventosViajeVM> GetEventosActivosByClienteIds(int clienteids, string period, string command)
        {
            return   _ebusService.getEventosActivosViaje<ListaEventosViajeVM>(clienteids, period, command);
        }
        /// <summary>
        /// Trae los eventos de las recargas
        /// </summary>
        /// <param name="clienteids"></param>
        /// <param name="period"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("GetEventosActivosRecargaByClienteIds")]
        public List<ListaEventosRecargaVM> GetEventosActivosRecargaByClienteIds(int clienteids, string period, string command)
        {
            return _ebusService.getEventosActivosViaje<ListaEventosRecargaVM>(clienteids, period, command);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ClienteId"></param>
        /// <returns></returns>

        [HttpPost("GetTiempoActualizacion")]
        public async Task<List<ParametrizacionVM>> ConsultarTiempoActualizacion(int ClienteId)
        {
            return await _ebusService.ConsultarTiempoActualizacion(ClienteId);

        }
        /// <summary>
        /// Consulta la posicion de los vehiculos
        /// </summary>
        /// <param name="ClienteIds"></param>
        /// <param name="Periodo"></param>
        /// <returns></returns>
        [HttpPost("GetUltimaPosicionVehiculos")]
        public async Task<List<ParqueoInteligenteVM>> GetUltimaPosicionVehiculos(int ClienteIds, string Periodo)
        {
            return await _ebusService.GetUltimaPosicionVehiculos(ClienteIds, Periodo);

        }

        
    }
}
