using Microsoft.AspNetCore.Mvc;

using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;

namespace Syscaf.Api.DWH.Controllers
{
    public class PortalController : ControllerBase
    {
       
        private readonly IPortalMService _portalService;
        public PortalController(IPortalMService _portalService)
        {
            this._portalService = _portalService;
        }

        [HttpGet("ObtenerViajesMetricas")]
        public async Task<ActionResult<ResultObject>> GetViajesMetricasMix()
        {
            return await _portalService.Get_ViajesMetricasPorClientes(null, null, null); ;
        }

        [HttpGet("ObtenerViajesMetricasHistorico")]
        public async Task<ActionResult<ResultObject>> GetViajesMetricasMixHistorico(int ClienteIds , DateTime FechaInicial, DateTime FechaFinal)
        {
            return await _portalService.Get_ViajesMetricasPorClientes(ClienteIds, FechaInicial, FechaFinal); 
        }
        [HttpGet("ObtenerEventos")]
        public async Task<ActionResult<ResultObject>> GetEventosMix()
        {
            return await _portalService.Get_EventosPorClientes(null, null, null); ;
        }

        [HttpGet("ObtenerEventosHistorico")]
        public async Task<ActionResult<ResultObject>> GetEventosMixHistorico(int ClienteIds, DateTime FechaInicial, DateTime FechaFinal)
        {
            return await _portalService.Get_EventosPorClientes(ClienteIds, FechaInicial, FechaFinal);
        }
    }
}
