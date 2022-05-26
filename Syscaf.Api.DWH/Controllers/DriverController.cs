using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;
using System.ComponentModel.DataAnnotations;

namespace Syscaf.Api.DWH.Controllers
{
    /// <summary>
    /// Controlador de transmisión
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class DriverController : ControllerBase
    {

        private readonly IDriverService _driverService;
        /// <summary>
        /// Controlador de transmisión
        /// </summary>
        /// <param name="_Transmision">aaa</param>
        public DriverController(IDriverService _driverService)
        {
            this._driverService = _driverService;
        }
        /// <summary>
        /// Obtiene el  listado de Conductores segun filtros
        /// </summary>
        /// <param name="Estado"></param>
        /// <param name="ClienteId"></param>
        
        /// <returns></returns>
        [HttpGet("GetDrivers/ClienteId")]
        public async Task<List<dynamic>> GetByClienteId(int? Estado, long? ClienteId)
        {
            return await _driverService.GetByClienteId(ClienteId, Estado);
        }

        /// <summary>
        /// Obtiene el  listado de Conductores segun filtros
        /// </summary>
        /// <param name="Estado"></param>
        /// <param name="ClienteIds"></param>

        /// <returns></returns>
        [HttpGet("GetDrivers/ClienteIds")]
        public async Task<List<dynamic>> GetByClienteId(int? Estado, int? ClienteIds)
        {
            return await _driverService.GetByClienteIds(ClienteIds, Estado);
        }
    }
}
