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

    public class ClienteController : ControllerBase
    {

        private readonly IClientService _clientService;
        /// <summary>
        /// Controlador de transmisión
        /// </summary>
        /// <param name="_Transmision">aaa</param>
        public ClienteController(IClientService _Transmision)
        {
            this._clientService = _Transmision;
        }
        /// <summary>
        /// Obtiene el  listado de cliente segun filtros
        /// </summary>
        /// <param name="Estado"></param>
        /// <param name="ClienteId"></param>
        /// /// <param name="ClienteIds"></param>
        /// <returns></returns>
        [HttpGet("GetClientes")]
        public async Task<List<ClienteDTO>> Get([Required] int Estado, long? ClienteId, int? ClienteIds)
        {
            return await _clientService.GetAsync(Estado, ClienteIds, ClienteId);
        }


    }
}
