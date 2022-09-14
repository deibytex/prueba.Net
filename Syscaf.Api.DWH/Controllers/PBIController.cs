using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using MiX.Integrate.Shared.Entities.Drivers;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Integrate.PORTAL;
using Syscaf.PBIConn.Services;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;
using Syscaf.Service.Portal.Models.RAG;
using Syscaf.Service.RAG;

namespace Syscaf.Api.DWH.Controllers
{
    [Route("api/[controller]")]
    public class PBIController : ControllerBase
    {

        private readonly IRagService _MixService;
        private readonly IPortalMService _portalService;
        private readonly INotificacionService _notificacionService;

        public PBIController(IRagService _MixService, INotificacionService _notificacionService, IPortalMService _portalService)
        {
            this._MixService = _MixService;
            this._notificacionService = _notificacionService;

            this._portalService = _portalService;
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
                var infomePBI = informeEficiencia.Select(s =>
                   new
                   {

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
                       s.ExcesoVelocidad_30_EventOccurrences,
                       s.mes,
                       s.anio
                   }
                    );
                var pbiResult = await EmbedService.SetDataDataSet(pbiClient, ConfigValidatorService.WorkspaceId, DatasetId, infomePBI.ToList<object>(), "Safety");


                if (!pbiResult.Exitoso)
                    await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, "Error al cargar Safety 914", Enums.ListaDistribucion.LSSISTEMA);
                else
                {
                    string TripsIds = informeEficiencia.Select(s => s.tripId.ToString()).Aggregate((i, j) => i + "," + j);
                    int filasAffectadas = await _MixService.setEsProcesadoTablaRAG(914, "Trip", TripsIds);

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
                if (datosSafetyEventos.Count > 0)
                {
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
                            s.mes,
                            s.anio
                        };
                    }).ToList<object>();
                    // enviamos los datos a PowerBI
                    var pbiResult = await EmbedService.SetDataDataSet(pbiClient, ConfigValidatorService.WorkspaceId, DatasetId, datosSafetyEventosObject, "SafetyEventos");

                    if (!pbiResult.Exitoso)
                        await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, "Error al cargar datos Safety Eventos 914", Enums.ListaDistribucion.LSSISTEMA);
                    else // enviamos los ids para que se marquen como procesado
                        await _MixService.setEsProcesadoTablaSafety(914, "Safety", datosSafetyEventos.Select(s => s.SafetyId.ToString()).Aggregate((i, j) => i + "," + j));
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
        [HttpGet("RellenoSafety")]
        public async Task<ActionResult<string>> RellenoSafety()
        {

            try
            {

         
            DateTime FechaServidor = DateTime.Now;
            DateTime FechaInicial = FechaServidor.AddDays(-1).Date;
            var datosSafetyEventos = (await _MixService.RellenoTripsEventScoring(914, FechaInicial, FechaServidor.Date));
                return $"FI ={FechaInicial.ToString()} FF={FechaServidor.Date.ToString()}";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }

         

        }

        /// <summary>
        /// Se consulta el servicio en mix y se guarda en la tabla creada.
        /// </summary>
        /// <param name="Clienteids"></param>
        /// <returns></returns>
        [HttpGet("portal/RellenoReporteViajesSemanal")]
        public async Task<ActionResult<int>> RellenoReporteViajesSemanal(int ClienteIds)
        {

            DateTime FechaFinal = DateTime.Now.Date;
            DateTime FechaInicial = FechaFinal.AddDays(-1).Date;
            var datosSafetyEventos = (await _portalService.Portal_RellenoInfomesViajesEventos(ClienteIds, FechaInicial, FechaFinal));

            return datosSafetyEventos;

        }
        [HttpGet("portal/CargarReporteViajesSemanal")]
        public async Task<ResultObject> CargarReporteViajesSemanal(string? DatasetId, DateTime? Fecha)
        {
            DatasetId = DatasetId ?? "99b5cce2-854b-456a-ab42-0933273abaf1";

            using (var pbiClient = await EmbedService.GetPowerBiClient())
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("Fecha", Fecha);                

                var informe = (await _portalService.getDynamicValueDWH("MovQueryHelper", "getReporteViajes", parametros));
                var infomePBI = informe.Select(s => {                 
                    return new
                    {
                        TripId = s.TripId.ToString(),
                        s.CIUDAD,
                        s.GERENTE,
                        s.MOVIL,
                        s.CONDUCTOR,
                        s.CEDULA,
                        FECHA = s.FECHA.Date,
                        s.FECHAHORAINICIAL,
                        s.FECHAHORAFINAL,
                        s.DURACION,
                        DURACIONHORA = (double?)s.DURACIONHORA,
                        DISTANCIA = (double?)s.DISTANCIA,
                        VELOCIDAD = (double?)s.VELOCIDAD ,
                        s.RALENTI,
                        COMBUSTIBLE = (double?)s.COMBUSTIBLE ,
                        s.TIPOLOGIA,
                        s.TIPOASSET,
                        s.TIPODIA,
                        s.SEMANAMES,
                        s.MES 
                    };
                   }
                    ).ToList();

                var pbiResult = await EmbedService.SetDataDataSet(pbiClient, ConfigValidatorService.WorkspaceId, DatasetId, infomePBI.ToList<object>(), "InformeViajes");


                if (!pbiResult.Exitoso)
                    await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, "Error al cargar CargarReporteViajesSemanal", Enums.ListaDistribucion.LSSISTEMA);


                var informeViajes = (await _portalService.getDynamicValueDWH("MovQueryHelper", "getReporteEvento", parametros));
                var infomeViajesPBI = informeViajes.Select(s =>
                {                   
                    return new
                    {
                        EventId = s.EventId.ToString(),
                        s.CIUDAD,
                        s.GERENTE,
                        s.DESCRIPCION,
                        s.PLACA,
                        s.TIPOLOGIA,
                        s.TIPOASSET,
                        s.CONDUCTOR,
                        s.CEDULA,
                        s.EVENTO,
                        FECHAINICIAL = s.FECHAINICIAL.Date,
                        FECHAFINAL = s.FECHAFINAL.Date,
                        HORAINICIAL = s.HORAINICIAL?.ToString(@"h\:mm\:ss"),
                        HORAFINAL = s.HORAFINAL?.ToString(@"h\:mm\:ss"),
                        s.FECHAHORAINICIAL,
                        s.FECHAHORAFINAL,
                        VALOR = (double?)s.VALOR,
                        DURACION = s.DURACION?.ToString(@"h\:mm\:ss"),
                        DURACIONHORA = (double?)s.DURACIONHORA,
                        s.DURACIONSEGUNDOS,
                        LATITUD = s.LATITUD.ToString(),
                        LONGITUD = s.LONGITUD.ToString(),
                        s.TIPODIA,
                        s.SEMANAMES,
                        s.MES 
                    };
                  }
                    ).ToList();

                var pbiResultv = await EmbedService.SetDataDataSet(pbiClient, ConfigValidatorService.WorkspaceId, DatasetId, infomeViajesPBI.ToList<object>(), "InformeEventos");


                if (!pbiResultv.Exitoso)
                    await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, "Error al cargar CargarReporteEventosSemanal", Enums.ListaDistribucion.LSSISTEMA);



            }

            return new ResultObject() { Exitoso = true };

        }

        #region CRUD Datasets

        //Creacion DataSets
        [HttpGet("portal/SetDataSet")]
        public async Task<ResultObject> SetDataSet(string nombreDataSet)
        {

            using (var pbiClient = await EmbedService.GetPowerBiClient())
            {

                //tablas a integrar al dataset

                var InformeEventos = new Table()
                {
                    Name = "InformeEventos",
                    Columns = new List<Column>()
                                {
                                    new Column("EventId", "String"),
                                    new Column("CIUDAD", "String"),
                                    new Column("GERENTE", "String"),
                                    new Column("DESCRIPCION", "String"),
                                    new Column("PLACA", "String"),
                                    new Column("TIPOLOGIA", "String"),
                                    new Column("TIPOASSET", "String"),
                                    new Column("CONDUCTOR", "String"),
                                    new Column("CEDULA", "String"),
                                    new Column("EVENTO", "String"),
                                    new Column("FECHAINICIAL", "Datetime", "dd/MM/yy"),
                                    new Column("FECHAFINAL", "Datetime", "dd/MM/yy"),
                                    new Column("HORAINICIAL", "Datetime","h:mm:ss"),
                                    new Column("HORAFINAL", "Datetime","h:mm:ss"),
                                    new Column("FECHAHORAINICIAL", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("FECHAHORAFINAL", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("VALOR", "Double"),
                                    new Column("DURACION", "Datetime","h:mm:ss"),
                                    new Column("DURACIONHORA", "Double","0.####"),
                                    new Column("DURACIONSEGUNDOS", "Int64"),
                                    new Column("LATITUD", "String"),
                                    new Column("LONGITUD", "String"),
                                    new Column("TIPODIA", "String"),
                                    new Column("MES", "Int64"),
                                    new Column("SEMANAMES", "String")
                                }
                };

                var InformeViajes = new Table()
                {
                    Name = "InformeViajes",
                    Columns = new List<Column>()
                                {
                                    new Column("TripId", "String"),
                                    new Column("CIUDAD", "String"),
                                    new Column("GERENTE", "String"),
                                    new Column("MOVIL", "String"),
                                    new Column("CONDUCTOR", "String"),
                                    new Column("CEDULA", "String"),
                                    new Column("FECHA", "Datetime", "dd/mm/yy"),
                                    new Column("FECHAHORAINICIAL", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("FECHAHORAFINAL", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("DURACION", "Int64"),
                                    new Column("DURACIONHORA", "Double","0.####"),
                                    new Column("DISTANCIA", "Double"),
                                    new Column("VELOCIDAD", "Double"),
                                    new Column("RALENTI", "Int64"),
                                    new Column("COMBUSTIBLE", "Double"),
                                    new Column("TIPOLOGIA", "String"),
                                    new Column("TIPOASSET", "String"),
                                    new Column("TIPODIA", "String"),
                                    new Column("MES", "Int64"),
                                    new Column("SEMANAMES", "String")
                        }
                };

                //Tablas Dummy para posibles nuevos usos
                var tableDummy_1 = new Table()
                {
                    Name = "tableDummy_1",
                    Columns = new List<Column>()
                                {
                                    new Column("FechaId", "Int64"),
                                    new Column("Fecha", "Datetime", "dd/mm/yy")
                                }
                };

                var tableDummy_2 = new Table()
                {
                    Name = "tableDummy_2",
                    Columns = new List<Column>()
                                {
                                    new Column("FechaId", "Int64"),
                                    new Column("Fecha", "String")
                                }
                };

                var tableDummy_3 = new Table()
                {
                    Name = "tableDummy_3",
                    Columns = new List<Column>()
                                {
                                    new Column("FechaId", "Int64"),
                                    new Column("Fecha", "String")
                                }
                };

                var tableDummy_4 = new Table()
                {
                    Name = "tableDummy_4",
                    Columns = new List<Column>()
                                {
                                    new Column("FechaId", "Int64"),
                                    new Column("Fecha", "String")
                                }
                };


                // Creación de datasets 
                var dataset = await pbiClient.Datasets.PostDatasetAsync(ConfigValidatorService.WorkspaceId, new CreateDatasetRequest()
                {
                    Name = nombreDataSet,
                    DefaultMode = "Push",
                    //integrar dentro de los corchetes {} las tablas a incluir.
                    Tables = new List<Table>() { InformeViajes, InformeEventos, tableDummy_1, tableDummy_2, tableDummy_3, tableDummy_4 }
                });
            }

            return new ResultObject() { Exitoso = true };

        }

        // Eliminar datos de las tablas dentro de los datasets
        [HttpGet("portal/deleteDataDataset")]
        public async Task<ResultObject> deleteDataDataset(string? DatasetId, string tableName)
        {
            DatasetId = DatasetId ?? "584140c7-ba24-4ad3-94ea-c7a16e5cab7d";

            var pbiResultv = await EmbedService.DeleteDataDataSet(ConfigValidatorService.WorkspaceId, DatasetId, tableName);

            return new ResultObject() { Exitoso = true };

        }

        //Modificar columnas tablas PBI de los datasets, tipo de dato eliminar o crear.
        [HttpGet("portal/setNewColumnDataset")]
        public async Task<ResultObject> setNewColumnDataset(string? DatasetId, string? tableName)
        {
            DatasetId = DatasetId ?? "584140c7-ba24-4ad3-94ea-c7a16e5cab7d";

            using (var pbiClient = await EmbedService.GetPowerBiClient())
            {


                var Eficiencia = new Table()
                {
                    Name = "Eficiencia",
                    Columns = new List<Column>()
                               {
                                   new Column("Movil", "String"),
                                   new Column("Operador", "String"),
                                   new Column("Fecha", "Datetime", "dd/mm/yy"),
                                   new Column("Inicio", "Datetime", "dd/MM/yy HH:mm:ss"),
                                   new Column("Fin", "Datetime", "dd/MM/yy HH:mm:ss"),
                                   new Column("Carga", "Double"),
                                   new Column("Descarga", "Double"),
                                   new Column("Distancia", "Double"),
                                   new Column("Duracion", "Datetime","h:mm:ss"),
                                   new Column("DuracionHora", "Double","0.#"),
                                   new Column("TotalConsumo", "Double"),
                                   new Column("MaxSOC", "Int64"),
                                   new Column("MinSOC", "Int64"),
                                   new Column("DSOC", "Int64"),
                                   new Column("RutaCodigo", "String"),
                                   new Column("RutaNombre", "String"),
                                   new Column("StartOdometer", "Double"),
                                   new Column("EndOdometer", "Double"),
                                   new Column("anio", "Int64"),
                                   new Column("mes", "Int64")
                               }
                };



                var pbiResultv = await EmbedService.SetNewColumn(pbiClient, ConfigValidatorService.WorkspaceId,DatasetId, tableName, Eficiencia);



            }

            return new ResultObject() { Exitoso = true };

        }


        #endregion


    }
}
