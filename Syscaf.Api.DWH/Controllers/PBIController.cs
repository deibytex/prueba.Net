using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.PBIConn.Services;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal.Models.RAG;
using Syscaf.Service.RAG;

namespace Syscaf.Api.DWH.Controllers
{
    [Route("api/[controller]")]
    public class PBIController : ControllerBase
    {
        
        private readonly IRagService _MixService;
        private readonly INotificacionService _notificacionService;
        private readonly IMapper _map;
        public PBIController(IRagService _MixService, INotificacionService _notificacionService, IMapper _map)
        {
            this._MixService = _MixService;
            this._notificacionService = _notificacionService;
            this._map = _map; 
        }
        /// <summary>
        /// Se consulta el servicio en mix y se guarda en la tabla creada.
        /// </summary>
        /// <param name="DatasetId"></param>
        /// /// <param name="Fecha"></param>
        /// <returns></returns>
        [HttpGet("CargarSafetyDataset")]
        public async Task<ResultObject> UploadSafety(string? DatasetId, DateTime? Fecha)
        {

            DatasetId = DatasetId ?? "365a6d57-fb13-45fa-b3cc-57705a5f8faa";
            Fecha = Fecha ?? Constants.GetFechaServidor().AddDays(-1);

            using (var pbiClient = await EmbedService.GetPowerBiClient())
            {
               
                var informeEficiencia = (await _MixService.getInformacionSafetyByClient(914, Fecha));
                var infomePBI = informeEficiencia.Select( s=> 
                    new  {
                       
                        s.Asset,
                        s.Driver,
                        s.Site,
                        s.TripsMaxSpeed,
                        s.TripsDrivingTime,
                        s.TripsDuration,
                        s.TripsDistance,
                        s.TripsCount,
                        s.Period,
                        s.tripStart,
                        s.tripEnd,
                        s.AceleracionBrusca_8_EventDuration,
                        s.AceleracionBrusca_8_EventMaxValue,
                        s.AceleracionBrusca_8_EventOccurrences,
                        s.FrenadaBrusca_10_EventDuration,
                        s.FrenadaBrusca_10_EventMaxValue,
                        s.FrenadaBrusca_10_EventOccurrences,
                        s.ExcesoVelocidad_50_EventDuration,
                        s.ExcesoVelocidad_50_EventMaxValue,
                        s.ExcesoVelocidad_50_EventOccurrences,
                        s.GiroBrusco_EventDuration,
                        s.GiroBrusco_EventMaxValue,
                        s.GiroBrusco_EventOccurrences,
                        s.ExcesoVelocidad_30_EventDuration,
                        s.ExcesoVelocidad_30_EventMaxValue,
                        s.ExcesoVelocidad_30_EventOccurrences
                    }
                    );
                var pbiResult = await EmbedService.SetDataDataSet(pbiClient, ConfigValidatorService.WorkspaceId, DatasetId, infomePBI.ToList<object>(), "Safety");

               
                if (!pbiResult.Exitoso)
                    await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, "Error al cargar Safety 914", Enums.ListaDistribucion.LSSISTEMA);
                else {
                    string TripsIds = informeEficiencia.Select(s => s.tripId.ToString()).Aggregate((i, j) => i + "," + j);
                    int filasAffectadas  = await _MixService.setEsProcesadoTablaRAG(914, "Trip", TripsIds);

                    if (filasAffectadas <= 0)
                        await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, "Error al cargar Safety 914", Enums.ListaDistribucion.LSSISTEMA);
                }

              

           }

            return new ResultObject() { Exitoso = true };
           
        }

        /// <summary>
        /// Se consulta el servicio en mix y se guarda en la tabla creada.
        /// </summary>
        /// <param name="DatasetId"></param>
        /// /// <param name="Fecha"></param>
        /// <returns></returns>
        [HttpGet("CargarSafetyEventosDataset")]
        public async Task<ResultObject> UploadSafetyEventos(string? DatasetId, DateTime? Fecha)
        {

            DatasetId = DatasetId ?? "365a6d57-fb13-45fa-b3cc-57705a5f8faa";


            using (var pbiClient = await EmbedService.GetPowerBiClient())
            {
                var datosSafetyEventos = (await _MixService.getEventosSafety(914, "Safety"));
                var datosSafetyEventosObject = datosSafetyEventos.Select(s =>
                {
                    return new
                    {
                        s.Movil,
                        s.Operador,
                        Fecha = s.Fecha.Date,
                        s.Inicio,
                        s.Fin,
                        s.Descripcion,
                        Duracion = s.Duracion.ToString(@"h\:mm\:ss"),
                        DuracionHora = (double)s.DuracionHora,
                        Valor = (double)(s.Valor ?? decimal.Zero),
                        FechaFin = s.FechaFin?.Date,
                        HoraInicial = s.HoraInicial?.ToString(@"h\:mm\:ss"),
                        HoraFinal = s.HoraFinal?.ToString(@"h\:mm\:ss"),
                        Latitud = s.Latitud.ToString(),
                        Longitud = s.Longitud.ToString(),
                        s.StartOdo,
                    };
                }).ToList<object>();
                // enviamos los datos a PowerBI
                var pbiResult = await EmbedService.SetDataDataSet(pbiClient, ConfigValidatorService.WorkspaceId, DatasetId, datosSafetyEventosObject, "SafetyEventos");

                if (!pbiResult.Exitoso)
                    await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, "Error al cargar datos Safety Eventos 914", Enums.ListaDistribucion.LSSISTEMA);
                else // enviamos los ids para que se marquen como procesado
                    await _MixService.setEsProcesadoTablaSafety(914, "Safety", datosSafetyEventos.Select(s => s.SafetyId.ToString()).Aggregate((i, j) => i + "," + j));

            }

            return new ResultObject() { Exitoso = true };

        }


        /// <summary>
        /// Se consulta el servicio en mix y se guarda en la tabla creada.
        /// </summary>
        /// <param name="DatasetId"></param>
        /// /// <param name="Fecha"></param>
        /// <returns></returns>
        [HttpGet("RellenoSafety")]
        public async Task<ActionResult<int>> RellenoSafety()
        {

            DateTime FechaServidor  = Constants.GetFechaServidor();
            DateTime FechaInicial = FechaServidor.AddDays(-1).Date;
                var datosSafetyEventos = (await _MixService.RellenoTripsEventScoring(914,  FechaInicial, FechaServidor.Date));
             
            return datosSafetyEventos;

        }


    }
}
