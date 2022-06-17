using Microsoft.AspNetCore.Mvc;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.PORTAL;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;
using SyscafWebApi.Service;
using System.ComponentModel.DataAnnotations;

namespace Syscaf.Api.DWH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortalController : ControllerBase
    {
       
        private readonly IPortalMService _portalService;
        private readonly IProcesoGeneracionService _procesoGeneracionService;
        private readonly INotificacionService _notificacionService;
        private readonly IMixIntegrateService _MixService;
        public PortalController(IPortalMService _portalService, IProcesoGeneracionService _procesoGeneracionService, INotificacionService _notificacionService, IMixIntegrateService _MixService)
        {
            this._portalService = _portalService;
            this._procesoGeneracionService = _procesoGeneracionService;
            this._notificacionService = _notificacionService;
            this._MixService = _MixService;
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

        [HttpGet("GetPosiciones")]
        public async Task<ActionResult<ResultObject>> GetPosicionesMixByGroup()
        {
            ResultObject result = null;

            var procesoGeneracion = await _procesoGeneracionService.GetFechasGeneracionByServicios((int)Enums.Servicios.Posiciones, 1);

            foreach (var fecha in procesoGeneracion)
            {
                result = await _portalService.Get_PositionsByClient(null,fecha.ProcesoGeneracionDatosId);

                SetEstadoProcesoGeneracionDatos(fecha.ProcesoGeneracionDatosId, result.Exitoso, " Posiciones -  Error al sincronizar " + result.Mensaje);
            }

            return (result);
        }
        [HttpGet("GetPosicionesSinValidar")]
        public async Task<ActionResult<ResultObject>> GetPosicionesMixByGroupSinValidar()
        {
      
            return await _portalService.Get_PositionsByClient(null, 0);
        }

        private void SetEstadoProcesoGeneracionDatos(int ProcesoGeneracionDatosId, bool exitoso, string mensaje)
        {
            int status = (!exitoso) ? (int)Enums.EstadoProcesoGeneracionDatos.SW_ERROR : (int)Enums.EstadoProcesoGeneracionDatos.SW_EXEC;
            _procesoGeneracionService.SetEstadoProcesoGeneracion(ProcesoGeneracionDatosId, status);

            if (!exitoso)
            {
                _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, mensaje, Enums.ListaDistribucion.LSSISTEMA);
                _procesoGeneracionService.SetLogDetalleProcesoGeneracionDatos(ProcesoGeneracionDatosId, mensaje, null, status);
            }

        }
        /// <summary>
        /// Se obtienen los detalles lista por lista Id
        /// </summary>
        /// <param name="ListaId"></param>
        /// <param name="Sigla"></param>
        /// <returns></returns>
        [HttpGet("GetDetallesLista")]
        public async Task<ResultObject> GetDetallesLista(int? ListaId, string? Sigla)
        {

            return await _portalService.GetDetallesListas(ListaId, Sigla);
        }
        /// <summary>
        /// Para consultar los score desde MIX
        /// </summary>
        /// <param name="EncScoringFlexDriver"></param>
        /// <returns></returns>
        [HttpPost("GetScoreFlexible")]
        public async Task<ResultObject> GetScoreFlexible([FromBody] EncScoringFlexDriverVM EncScoringFlexDriver)
        {
            var result = (await _portalService.GetDriverxCliente(EncScoringFlexDriver.ClienteIds));
            //var Cliente = (await _portalService.GetCliente(ClienteIds));
            var final = new ResultObject();
            final.Data = await _MixService.GetFlexibleRAGScoreReportAsync(result, EncScoringFlexDriver.from, EncScoringFlexDriver.to, EncScoringFlexDriver.aggregationPeriod, EncScoringFlexDriver.ClienteIds, EncScoringFlexDriver.ClienteId);
            final.Exitoso = true;
            final.Mensaje = "Operación éxitosa";
            return final;
        }
        /// <summary>
        /// Se consulta el servicio en mix y se guarda en la tabla creada.
        /// </summary>
        /// <param name="EncScoringFlexDriver"></param>
        /// <returns></returns>
        [HttpPost("GuardarEncScoringDetalleScoringFlexDriver")]
        public async Task<ResultObject> GuardarEncScoringDetalleScoringFlexDriver([FromBody] EncScoringFlexDriverVM EncScoringFlexDriver)
        {
            return await _portalService.GuardarEncScoringDetalleScoringFlexDriver(EncScoringFlexDriver);
        }


    }
}
