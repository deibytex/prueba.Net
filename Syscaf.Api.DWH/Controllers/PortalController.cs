using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.PORTAL;
using Syscaf.Common.Services;
using Syscaf.Common.Utils;
using Syscaf.Service.Helpers;
using Syscaf.Service.Peg;
using Syscaf.Service.Portal;
using SyscafWebApi.Service;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

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
        private readonly IClientService _clientService;
        private readonly IPegasoService _pegasoService;
     
        readonly DateTime _fechaservidor = Constants.GetFechaServidor();
        public PortalController(IPortalMService _portalService, IProcesoGeneracionService _procesoGeneracionService, INotificacionService _notificacionService
            , IMixIntegrateService _MixService, IClientService _clientService, IPegasoService _pegasoService)
        {
            this._portalService = _portalService;
            this._procesoGeneracionService = _procesoGeneracionService;
            this._notificacionService = _notificacionService;
            this._MixService = _MixService;
            this._clientService = _clientService;
           this._pegasoService = _pegasoService;
          
        }
        [HttpGet]
        public async Task<ResultObject> EjecucionesSimultaneasPortal()
        {

            // extrae informacion de trace cada minuto esto por la cantidad de datos que extrae
            var result = await _portalService.Get_EventosPorClientes(909, null,  null);
            if (!result.Exitoso)
                await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, $"ObtenerDatosPortal_eventos TRACE {result.Mensaje}", Enums.ListaDistribucion.LSSISTEMA);

            result = await _portalService.Get_EventosPorClientes(914, null, null);
            if (!result.Exitoso)
                await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, $"ObtenerDatosPortal_eventos ESOMOS F {result.Mensaje}", Enums.ListaDistribucion.LSSISTEMA);


            //// revisa ejecucioin que es a cada minuto
            //result = await _transmisionClass.PruebaSimCard();
            //if (!result.Exitoso)
            //    _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, $"PruebaSimCard  {result.Mensaje}", Enums.ListaDistribucion.LSSISTEMA);

            // obtiene las posiciones de los que tengan marcado como posiciones en la tabla clientes
            result = await _portalService.Get_PositionsByClientPositionsActive();
            if (!result.Exitoso)
                await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, $"ObtenerDatosPortal_Posiciones  {result.Mensaje}", Enums.ListaDistribucion.LSSISTEMA);

            //// estrae la informacion del proyecto esomos
            //string period = $"{_fechaservidor.Month}{_fechaservidor.Year}";
            //List<int> clientesIds = new List<int>() { 914 };
            //foreach (int clienteid in clientesIds)
            //{
            //    result = await _eBusClass.SetEventosActivos(period, clienteid);
            //    if (!result.Exitoso)
            //        await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, $"Error al cargar eventos Activos {result.Mensaje}", Enums.ListaDistribucion.LSSISTEMA);

            //}

            //// cada 5 minutos envia notificacion
            //if (_fechaservidor.Minute % 5 == 0)
            //{
            //    await _notificacionService.EnviarCorreosSistemaNotificacion();

            //}


            // cada minuto 10 ejecuta la extraccion
            if (_fechaservidor.Minute % 10 == 0)
            {

                await _pegasoService.SendData();
            }
            return null;
        }

        [HttpGet("ObtenerViajesMetricas")]
        public async Task<ActionResult<ResultObject>> GetViajesMetricasMix()
        {
            return await _portalService.Get_ViajesMetricasPorClientes(null, null, null); ;
        }

        [HttpGet("ObtenerViajesMetricasHistorico")]
        public async Task<ActionResult<ResultObject>> GetViajesMetricasMixHistorico(int ClienteIds, DateTime FechaInicial, DateTime FechaFinal)
        {
            return await _portalService.Get_ViajesMetricasPorClientes(ClienteIds, FechaInicial, FechaFinal);
        }
        [HttpGet("ObtenerEventos")]
        public async Task<ActionResult<ResultObject>> GetEventosMix(int? ClienteIds)
        {
            return await _portalService.Get_EventosPorClientes(ClienteIds, null, null);
        }

        [HttpGet("ObtenerEventosHistorico")]
        public async Task<ActionResult<ResultObject>> GetEventosMixHistorico(int ClienteIds, DateTime FechaInicial, DateTime FechaFinal)
        {
            return await _portalService.Get_EventosPorClientes(ClienteIds, FechaInicial, FechaFinal);
        }

        [HttpGet("ObtenerEventosActivos")]
        public async Task<ActionResult<ResultObject>> GetEventosActivos(int? ClienteIds, DateTime? FechaInicial, DateTime? FechaFinal)
        {
            return await _portalService.Get_EventosActivosPorClientes(ClienteIds, FechaInicial, FechaFinal);
        }

        [HttpGet("GetPosiciones")]
        public async Task<ActionResult<ResultObject>> GetPosicionesMixByGroup()
        {
            ResultObject result = null;

            var procesoGeneracion = await _procesoGeneracionService.GetFechasGeneracionByServicios((int)Enums.Servicios.Posiciones, 1);

            foreach (var fecha in procesoGeneracion)
            {
                result = await _portalService.Get_PositionsByClient(null, fecha.ProcesoGeneracionDatosId);

                SetEstadoProcesoGeneracionDatos(fecha.ProcesoGeneracionDatosId, result.Exitoso, " Posiciones -  Error al sincronizar " + result.Mensaje);
            }

            return (result);
        }
        [HttpGet("GetPosicionesSinValidar")]
        public async Task<ActionResult<ResultObject>> GetPosicionesMixByGroupSinValidar(int? ClienteIds)
        {

            return await _portalService.Get_PositionsByClient(ClienteIds, 0);
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
        /// <param name="ClienteIds"></param>
        /// /// <param name="FechaInicial"></param>
        /// /// <param name="FechaFinal"></param>
        /// <returns></returns>
        [HttpGet("GuardarEncScoringDetalleScoringFlexDriver")]
        public async Task<ResultObject> GuardarEncScoringDetalleScoringFlexDriver(int? ClienteIds, DateTime? FechaInicial, DateTime? FechaFinal)
        {

            return await _portalService.GuardarEncScoringDetalleScoringFlexDriver(ClienteIds, FechaInicial, FechaFinal);
        }

        /// <summary>
        /// Permite ejecutar de manera manual la extraccion de las pruebas simCard
        /// </summary>

        /// <returns></returns>
        [HttpGet("ExecPruebaSimCard")]
        public async Task<ResultObject> ExecPruebaSimCard()
        {

            return await _portalService.PruebaSimCard();
        }

      


        [HttpPost("GetConsultasDinamicas")]
        public async Task<List<dynamic>> GetConsultasDinamicas( [FromBody]  Dictionary<string, string> parametros , [FromQuery]  string Clase, [FromQuery]  string NombreConsulta)
        {


            var dynamic = new Dapper.DynamicParameters() { };
            foreach (var kvp in parametros)
            {
                dynamic.Add(  kvp.Key, kvp.Value);
            }


            return await _portalService.getDynamicValueDWH(Clase, NombreConsulta, dynamic);
        }

        [HttpPost("GetConsultasDinamicasProced")]
        public async Task<List<dynamic>> GetConsultasDinamicasProcedure([FromBody] Dictionary<string, string>? parametros, [FromQuery] string Clase, [FromQuery] string NombreConsulta)
        {


            var dynamic = new Dapper.DynamicParameters();
            if(parametros !=  null)
            foreach (var kvp in parametros)
            {
                dynamic.Add(kvp.Key, kvp.Value);
            }


            return await _portalService.getDynamicValueProcedureDWH(Clase, NombreConsulta, dynamic);
        }

        [HttpPost("GetConsultasDinamicasTablaDinamica")]
        // permite relaizar consultas dinamicas colocando una tabla dinamica
        // la consulta en la tabla consultas por tipos debe contener la variable {tabla} la cual sera
        // reemplazada, eje, tabla = _ebus_914  select 1 from tabla{tabla}=> resultado select 1 from tabla_ebus_914
        public async Task<List<dynamic>> GetConsultasDinamicasTablasDinamica([FromBody] Dictionary<string, string>? parametros,
            [FromQuery] string Clase, [FromQuery] string NombreConsulta, [FromQuery] string tabla)
        {


            var dynamic = new Dapper.DynamicParameters();
            if (parametros != null)
                foreach (var kvp in parametros)
                {
                    dynamic.Add(kvp.Key, kvp.Value);
                }


            return await _portalService.getDynamicValueProcedureDWHTabla(Clase, NombreConsulta, dynamic, tabla);
        }



    }
}
