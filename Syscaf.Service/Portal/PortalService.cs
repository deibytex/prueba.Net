
using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;
using MiX.Integrate.Shared.Entities.Events;
using MiX.Integrate.Shared.Entities.Positions;
using MiX.Integrate.Shared.Entities.Scoring;
using MiX.Integrate.Shared.Entities.Trips;
using Newtonsoft.Json;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.PORTAL;
using Syscaf.Common.Services;
using Syscaf.Common.Utilities;
using Syscaf.Common.Utils;
using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.DataTableSql;
using Syscaf.Service.dbo.Models;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal.Models;
using SyscafWebApi.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Syscaf.Common.Helpers.Enums;

namespace Syscaf.Service.Portal
{
    public class PortalMService : IPortalMService
    {
        private readonly IAssetsService _asset;
        private readonly IDriverService _driverService;
        private readonly IClientService _clientService;
        private readonly IMixIntegrateService _Mix;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
       
        private readonly ISyscafConn _connDWH;
        private readonly IProcesoGeneracionService _procesoGeneracionService;
        private readonly INotificacionService _notificacionService;
        private readonly SyscafCoreConn _connCore;
        private readonly IMixIntegrateService _MixService;
      

        public PortalMService(ISyscafConn _connDWH, IAssetsService _asset,
            IClientService _clientService, IMixIntegrateService _Mix,
            IMapper _mapper, IProcesoGeneracionService _procesoGeneracionService, INotificacionService _notificacionService,
            ILogService _logService,
            SyscafCoreConn _connCore,
             IMixIntegrateService _MixService,
             IDriverService _driverService
            )
        {
            this._logService = _logService;
            this._connDWH = _connDWH;
            this._asset = _asset;
            this._clientService = _clientService;
            this._Mix = _Mix;
            this._mapper = _mapper;
          
            this._procesoGeneracionService = _procesoGeneracionService;
            this._notificacionService = _notificacionService;
            this._connCore = _connCore;
            this._MixService = _MixService;
            this._driverService = _driverService;
            
        }
        public async Task<ResultObject> Get_ViajesMetricasPorClientes(int? Clienteids, DateTime? FechaInicial, DateTime? FechaFinal)
        {
            ResultObject result = new();
            // traemos el listado de clientes
            var ListadoClientes = await _clientService.GetAsync(1);
            try
            {


                foreach (var item in ListadoClientes)
                {
                    //// guardamos los viajes por periodo, se debe determinar a traves de los viajes cual es el perido al que pertenece la informacion y guardarlo al que corresponda
                    IList<Trip> trips =
                        (FechaFinal == null && FechaFinal == null) ?
                        await _Mix.GetUltimosViajesCreadosByOrganization(item.clienteId, item.clienteIdS, "GetViajesPortal")
                        :
                        await _Mix.getViajes(new List<long>() { item.clienteId }, FechaInicial.Value, FechaFinal.Value, item.clienteIdS);
                    ;

                    if (trips != null && trips.Count > 0)
                    {
                        trips.GroupBy(g => new { Constants.GetFechaServidor(g.TripStart).Month, Constants.GetFechaServidor(g.TripStart).Year })
                                                  .Select(s => new { Period = s.Key.Month.ToString() + s.Key.Year.ToString(), Viajes = s }).ToList().ForEach(async f =>
                                                  {
                                                  //// verifica que existan y se manda la fecha desde el cual elsistema empieza a validar si existen
                                                  var ResultTrips = await GetIdsNoIngresadosByClienteAsync(f.Viajes.Select(s => s.TripId).ToList(), f.Period, (int)Enums.PortalTipoValidacion.viajes, item.clienteIdS);
                                                  //// cruzamos el resultado con el listado general de eventos
                                                  var tripsFilters = f.Viajes.Where(w => ResultTrips.Any(a => a == w.TripId)).ToList();

                                                      if (tripsFilters.Count > 0)
                                                      {

                                                          var tripsInserts = _mapper.Map<List<TripsNew>>(tripsFilters);
                                                          result = await SetDatosPortalByClienteAsync(HelperDatatable.ToDataTable(tripsInserts), f.Period, "Trips", item.clienteIdS);
                                                          if (!result.Exitoso)
                                                              _logService.SetLogError(0, "Inserta Trips ", $"Cliente: {item.clienteNombre} , {result.Mensaje}");
                                                      }
                                                  });

                        if (item.Metrics)
                        {
                            DateTime minDateMetricas = trips.Min(min => min.TripStart);
                            minDateMetricas = minDateMetricas.Date;
                            DateTime maxDateMetricas = trips.Max(max => max.TripEnd);
                            maxDateMetricas = maxDateMetricas.Date;

                            while (minDateMetricas <= maxDateMetricas)
                            {

                                DateTime fechaconsulta = minDateMetricas;
                                minDateMetricas = minDateMetricas.AddDays(1);
                                DateTime fechaconsultaf = minDateMetricas;
                                var totalTrips = trips.Where(w => w.TripStart >= fechaconsulta && w.TripStart <= fechaconsultaf);

                                if (totalTrips.Count() > 0)
                                {
                                    //  treemos las metricas por rangos no superiores a  7 días dependiendo del los rangos de fechas
                                    List<TripRibasMetrics> metricasMix = await _Mix.GetMetricasPorDriver(trips.Select(s => s.DriverId).Distinct().ToList(), fechaconsulta.AddHours(-5), fechaconsultaf.AddHours(-5), item.clienteIdS);


                                    metricasMix
                                        .GroupBy(g => new { Constants.GetFechaServidor(g.TripStart).Month, Constants.GetFechaServidor(g.TripStart).Year })
                                        .Select(s => new { Period = s.Key.Month.ToString() + s.Key.Year.ToString(), Metricas = s }).ToList().ForEach(async f =>
                                        {

                                        // verifica que existan 
                                        var ResultMetricas = await GetIdsNoIngresadosByClienteAsync(f.Metricas.Select(s => s.TripId).ToList(), f.Period, (int)Enums.PortalTipoValidacion.metricas, item.clienteIdS);

                                        // cruzamos el resultado con el listado general de eventos
                                        var metricasFilter = f.Metricas.Where(w => ResultMetricas.Any(a => a == w.TripId)).ToList();

                                            if (metricasFilter.Count > 0)
                                            {
                                                var tripsMetris = _mapper.Map<List<MetricsNew>>(metricasFilter);


                                                result = await SetDatosPortalByClienteAsync(HelperDatatable.ToDataTable(tripsMetris), f.Period, "TripsMetrics", item.clienteIdS);

                                                if (!result.Exitoso)
                                                    _logService.SetLogError(0, "Inserta Trips Metrics ", $"Cliente: {item.clienteNombre} , {result.Mensaje}");
                                            }

                                        });

                                }

                            }


                        }
                    }

                    // ejecuta los eventos
                  
                }
            }
            catch (Exception ex)
            {

                result.error(ex.ToString());
            }

            return result;
        }

        public async Task<ResultObject> Get_EventosPorClientes(int? ClienteIds, DateTime? FechaInicial, DateTime? FechaFinal)
        {
            ResultObject result = new();
            string _Clientenombre = "";
            try
            {
                // traemos el listado de clientes
                var ListadoClientes = await _clientService.GetAsync(1, ClienteIds);
               
                foreach (var item in ListadoClientes)
                {
                                       // si tienen configurado al menos un evento que extraer
                    var getEventos = GetPreferenciasDescargarEventos(item.clienteIdS);
                    _Clientenombre = item.clienteNombre;
                    if (getEventos != null && getEventos.Count > 0)
                    {
                        //////////////////////////////////////////////////////////////////////////////////////
                        ///  GUARDAMOS LOS ULTIMOS EVENTOS CREADOS POR ORGANIZACION

                        // nos traemos los últimos eventos creados por cada vehículo


                        var eventos =
                             (FechaFinal == null && FechaFinal == null) ?
                            await _Mix.GetUltimosEventosCreadosPorOrganizacion(item.clienteId, getEventos.Select(s => s.EventTypeId.Value).Distinct().ToList(), item.clienteIdS)
                            :
                            await _Mix.GetEventosClientePorAssets(new List<long> { item.clienteId }, getEventos.Select(s => s.EventTypeId.Value).Distinct().ToList(), FechaInicial.Value, FechaFinal.Value, item.clienteIdS)
                            ;

                        // filtramos por los eventos que necesitamos consultar
                        if (eventos != null && eventos.Count > 0)
                        {
                            eventos = eventos.Where(w => w.StartDateTime != null).ToList();
                            eventos.
                                GroupBy(g => new { Constants.GetFechaServidor(g.StartDateTime, false)?.Month, Constants.GetFechaServidor(g.StartDateTime, false)?.Year })
                                .Select(s => new { Period = s.Key.Month.ToString() + s.Key.Year.ToString(), Eventos = s }).ToList().ForEach(async f =>
                                {

                                    var ResultEvents = await GetIdsNoIngresadosByClienteAsync(f.Eventos.Select(s => s.EventId).ToList(), f.Period, (int)Enums.PortalTipoValidacion.eventos, item.clienteIdS);

                                    var eventosFilter = f.Eventos.Where(w => ResultEvents.Any(a => a == w.EventId)).ToList();

                                    if (eventosFilter.Count > 0)
                                    {
                                        var listEventosInsertar = _mapper.Map<List<EventsNew>>(eventosFilter);
                                        listEventosInsertar = listEventosInsertar.Select(s =>
                                        {
                                            s.isebus = getEventos.
                                               Where(w => (w.Parametrizacion ?? "").Contains("75") && w.EventTypeId == s.EventTypeId).Count() > 0;
                                         
                                            return s;
                                        }).ToList();

                                        var resultevento = await SetDatosPortalByClienteAsync(HelperDatatable.ToDataTable(listEventosInsertar), f.Period, "Event", item.clienteIdS);

                                        if (!resultevento.Exitoso)
                                            _logService.SetLogError(0, "Portal.GetEventos", resultevento.Mensaje);

                                    }
                                });
                        }
                                            }

                    result.success($"Cliente { item.clienteNombre } cargado satisfactoriametne  { Constants.GetFechaServidor()}");
                }

              
            }
            catch (Exception ex)
            {
                string mensaje = $" {Constants.GetFechaServidor()} = { ex.Message}";              
                result.error(ex.Message.ToString());
                await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, $"Cliente  =  {_Clientenombre} Error  = { mensaje}  ;", Enums.ListaDistribucion.LSSISTEMA);
                _logService.SetLogError(0, "Portal - Eventos", mensaje);             
             
            }


            return result;
        }

        public async Task<ResultObject> Get_EventosActivosPorClientes(int? ClienteIds)
        {
            ResultObject result = new();
            string _Clientenombre = "";
            try
            {
                // traemos el listado de clientes
                var ListadoClientes = await _clientService.GetAsync(1, clienteIds: ClienteIds);

                foreach (var item in ListadoClientes.Where(w => w.ActiveEvent == true)) 
                {
                    // si tienen configurado al menos un evento que extraer
                    var getEventos = GetPreferenciasDescargarEventos(item.clienteIdS);
                    _Clientenombre = item.clienteNombre;

                    if (getEventos != null && getEventos.Count > 0)
                    {
                        //////////////////////////////////////////////////////////////////////////////////////
                        ///  GUARDAMOS LOS ULTIMOS EVENTOS CREADOS POR ORGANIZACION

                        // nos traemos los últimos eventos creados por cada vehículo

                        var Data = new ResultObject();

                        var eventsactive =  await _Mix.GetEventosActivosCreadosPorOrganizacion( item.clienteId, getEventos.Select(s => s.EventTypeId.Value).Distinct().ToList(), item.clienteIdS);

                        

                        // filtramos por los eventos que necesitamos consultar
                        if (eventsactive != null && eventsactive.Count > 0)
                        {
                            eventsactive = eventsactive.Select(s => { 
                                s.EventDateTime = Constants.GetFechaServidor(s.EventDateTime); 
                                s.ReceivedDateTime = Constants.GetFechaServidor(s.ReceivedDateTime ?? DateTime.Now); return s; }).ToList();
                            eventsactive.
                                GroupBy(g => new { Constants.GetFechaServidor(g.EventDateTime, false)?.Month, Constants.GetFechaServidor(g.EventDateTime, false)?.Year })
                                .Select(s => new { Period = s.Key.Month.ToString() + s.Key.Year.ToString(), Eventos = s }).ToList()
                                .ForEach(async f =>
                                {

                                    //Serializamod la data

                                    // modificamos la fecha para que sea la colombiana que necesitamos 
                                   
                                    Data.Data = JsonConvert.SerializeObject(f.Eventos);



                                    var parametros = new Dapper.DynamicParameters();
                                    parametros.Add("JSON_STR", Data.Data);
                                    parametros.Add("periodo", f.Period);
                                    parametros.Add("clienteIdS", item.clienteIdS);

                                    try
                                    {
                                        //Se ejecuta el procedimiento almacenado.
                                        var resultado = await Task.FromResult(_connDWH.Get<string>(PortalQueryHelper.ActiveEvent, parametros, commandType: CommandType.StoredProcedure));

                                        result.Mensaje = resultado?.ToString();
                                        result.Exitoso = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.Message.ToString();
                                    }

                                })
                                ;

                            
                        }
                    }

                    result.success($"Cliente { item.clienteNombre } cargado satisfactoriametne  { Constants.GetFechaServidor()}");
                }


            }
            catch (Exception ex)
            {
                string mensaje = $" {Constants.GetFechaServidor()} = { ex.Message}";
                result.error(ex.Message.ToString());
                await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, $"Cliente  =  {_Clientenombre} Error  = { mensaje}  ;", Enums.ListaDistribucion.LSSISTEMA);
                _logService.SetLogError(0, "Portal - Eventos", mensaje);

            }


            return result;
        }
        // ingreso de informacion del reporte sotramac
        #region REPORTE SOTRAMAC
        #endregion


        #region GUARDA DATOS TABLAS PORTAL

        public async Task<ResultObject> SetDatosPortalByClienteAsync(DataTable data, string Periodo, string tabla, int Clienteids)
        {
            ResultObject resultado = new ResultObject();
            try
            {
               
                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("Clienteids", Clienteids, DbType.Int32);
                    parametros.Add("Period", Periodo, DbType.String);
                    parametros.Add($"Data{tabla}", data.AsTableValuedParameter($"PORTAL.UDT_{tabla}"));

                    //  traemos la información de los identificadores que no existen en la base de datos                   
                    await Task.FromResult(_connDWH.Execute(PortalQueryHelper._guardaTablasPortal(tabla), parametros, commandType: CommandType.StoredProcedure));

                    resultado.success(null);
                
            }
            catch (Exception ex)
            {
                resultado.error(ex.ToString());
            }
            return resultado;
        }

        #endregion
        #region VERIFICA IDS
        // trae de la bd los identificadoes no ingresados en el sietma
        public async Task<List<long>> GetIdsNoIngresadosByClienteAsync(List<long> Ids, string Periodo, int tipo, int Clienteids)
        {
            try
            {
                using (SyscafBD ctx = new SyscafBD())
                {

                    var parametros = new Dapper.DynamicParameters();
                    var IdsList = Ids.Select(s => new { Id = s }).ToList();
                    parametros.Add("Clienteids", Clienteids, DbType.Int32);
                    parametros.Add("Period", Periodo, DbType.String);
                    parametros.Add("Table", tipo, DbType.Int32);
                    parametros.Add("Data", HelperDatatable.ToDataTable(IdsList).AsTableValuedParameter("dbo.UDT_TableIdentity"));

                    //  traemos la información de los identificadores que no existen en la base de datos                   
                    return await Task.FromResult(_connDWH.GetAll<long>(PortalQueryHelper._verificaIdExistentes, parametros, commandType: CommandType.StoredProcedure));
                    // trae la información de eventos 
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        public List<PreferenciasDescargarWS> GetPreferenciasDescargarEventos(int clienteIdS)
        {
            List<PreferenciasDescargarWS> preferencias = new List<PreferenciasDescargarWS>();
            Task task = Task.Run(() =>
            {
                try
                {
                    string sqlCommand = $" Where (TPDW.TipoPreferencia > 2) AND TPDW.ClientesId LIKE '%{clienteIdS}%'";
                    preferencias = _connDWH.GetAll<PreferenciasDescargarWS>(PortalQueryHelper._SelectPreferenciasDescargas + sqlCommand, null, CommandType.Text).ToList();

                }
                catch (Exception ex)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());
                }
            });

            task.Wait();

            return preferencias;
        }
        public List<PreferenciasDescargarWS> GetPreferenciasDescargarEventos(int clienteIdS, int TipoPreferencia)
        {
            List<PreferenciasDescargarWS> preferencias = new List<PreferenciasDescargarWS>();
            Task task = Task.Run(() =>
            {
                try
                {
                    string sqlCommand = $" Where (TPDW.TipoPreferencia = {TipoPreferencia}) AND TPDW.ClientesId LIKE '%{clienteIdS}%'";
                    preferencias = _connDWH.GetAll<PreferenciasDescargarWS>(PortalQueryHelper._SelectPreferenciasDescargas + sqlCommand, null, CommandType.Text).ToList();

                }
                catch (Exception ex)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());
                }
            });

            task.Wait();

            return preferencias;
        }

        public async Task<ResultObject> Get_PositionsByClient(int? Clienteids, int ProcesoGeneracionDatosId)
        {
            ResultObject result = new();
            // traemos el listado de clientes
            var ListadoClientes = await _clientService.GetAsync(1, Clienteids);

            _logService.SetLogError(0, "Posiciones - obtenerPosiciones", "Operacion inicial en posiciones");
            foreach (var item in ListadoClientes.Where(w => w.notificacion))
            {
                //// guardamos los viajes por periodo, se debe determinar a traves de los viajes cual es el perido al que pertenece la informacion y guardarlo al que corresponda

                try
                {
                    List<Position> positions = await _Mix.getPositionsByGroups(new List<long>() { item.clienteId }, item.clienteIdS);

                    if (positions != null)
                    {
                        var posiciones = positions.Select(s =>
                            new
                            {
                                s.PositionId,
                                s.AssetId,
                                s.DriverId,
                                s.FormattedAddress,
                                s.Hdop,
                                s.Heading,
                                s.IsAvl,
                                s.Latitude,
                                s.Longitude,
                                s.SpeedKilometresPerHour,
                                s.SpeedLimit,
                                Timestamp = s.Timestamp.ToColombiaTime(),
                                FechaSistema = Constants.GetFechaServidor(),
                                item.clienteId

                            });

                        string Lista = JsonConvert.SerializeObject(posiciones);
                        var restult = await _connCore.GetAsync<int>(PortalQueryHelper._insertaPosiciones, new { Lista }, CommandType.StoredProcedure);

                        result.success();

                    }

                }
                catch (Exception ex)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());
                    _procesoGeneracionService.SetLogDetalleProcesoGeneracionDatos(ProcesoGeneracionDatosId, $"ClienteId = {  item.clienteNombre } " + ex.Message, null, (int)Enums.EstadoProcesoGeneracionDatos.SW_NOEXEC);
                    await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, $"Posiciones al cargar posiciones, Cliente = { item.clienteNombre }", Enums.ListaDistribucion.LSSISTEMA);
                    result.error(ex.Message);
                }
            }

            return result;
        }

        public async Task<ResultObject> Get_PositionsByClientPositionsActive()
        {
            ResultObject result = new();
            // traemos el listado de clientes
            var ListadoClientes = await _clientService.GetAsync(1);
            ListadoClientes = ListadoClientes.Where(w => w.Position).ToList();
            _logService.SetLogError(0, "Posiciones - obtenerPosiciones con Positions Active", "Operacion inicial en posiciones");
            foreach (var item in ListadoClientes)
            {
                //// guardamos los viajes por periodo, se debe determinar a traves de los viajes cual es el perido al que pertenece la informacion y guardarlo al que corresponda

                try
                {
                    List<Position> positions = await _Mix.getPositionsByGroups(new List<long>() { item.clienteId }, item.clienteIdS);

                    if (positions != null)
                    {
                        var posiciones = positions.Select(s =>
                            new
                            {
                                s.PositionId,
                                s.AssetId,
                                s.DriverId,
                                s.FormattedAddress,
                                s.Latitude,
                                s.Longitude,
                                s.NumberOfSatellites,
                                s.SpeedLimit,
                                s.AltitudeMetres,
                                Timestamp = s.Timestamp.ToColombiaTime(),
                                FechaSistema = Constants.GetFechaServidor()
                            });

                        string Lista = JsonConvert.SerializeObject(posiciones);
                        var restult = await _connCore.GetAsync<int>(PortalQueryHelper._insertaPosicionesCliente, new { Lista }, CommandType.StoredProcedure);

                        result.success();

                    }

                }
                catch (Exception ex)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());
                    await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem, $"Posiciones al cargar posiciones, Cliente = { item.clienteNombre }", Enums.ListaDistribucion.LSSISTEMA);
                    result.error(ex.Message);
                }
                finally {
                    _logService.SetLogError(0, "Posiciones - obtenerPosiciones con Positions Active", "Operacion final en posiciones");
                }
            }

            return result;
        }

        public async Task<ResultObject> GetDetallesListas(int? ListaId, string Sigla)
        {
            var r = new ResultObject();
            try
            {

                var parametros = new Dapper.DynamicParameters();
                parametros.Add("ListaId", ListaId);
                parametros.Add("Sigla", Sigla);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<DetalleListaVM>(PortalQueryHelper._listaDetalle, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result;
                    r.Exitoso = true;
                    r.Mensaje = "Operación éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Lista " + ListaId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }

        public async Task<List<long>> GetDriverxCliente(int ClienteId)
        {
            var r = new List<long>();
            try
            {

                var parametros = new Dapper.DynamicParameters();
                parametros.Add("ClienteId", ClienteId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<long>(PortalQueryHelper.DriverxCliente, parametros, commandType: CommandType.StoredProcedure));
                    r = result;

                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
            return r;
        }
        
        public async Task<ResultObject> GuardarEncScoringDetalleScoringFlexDriver(int? ClienteIds, DateTime? FechaInicial, DateTime? FechaFinal, string aggregationPeriod = "Daily")
        {
            ResultObject result = new();
            string from = (FechaInicial ?? Constants.GetFechaServidor()).Date.AddDays(-1).FormatoyyyyMMddHHmmss();
            string to = (FechaFinal ?? Constants.GetFechaServidor()).Date.AddSeconds(-1).FormatoyyyyMMddHHmmss();

            // traemos el listado de clientes
            var ListadoClientes = await _clientService.GetAsync(1, clienteIds: ClienteIds);
            var nameOfProperty = "DriverId";
            foreach (var item in ListadoClientes)
            {
                try
                {
                    var datapura = (await _driverService.GetByClienteIds(item.clienteIdS, null));
                    var conductores = datapura.Select(s =>
                    {
                        var propiedades = (IDictionary<string, object>)s;
                        propiedades.TryGetValue(nameOfProperty, out object valor);
                        return (long)valor;
                    }
                     ).ToList();

                    var Datos = new Report_FlexibleRAG();
                    var Data = new ResultObject();
                    Datos = await _MixService.GetFlexibleRAGScoreReportAsync(conductores, from, to, aggregationPeriod, item.clienteIdS, item.clienteId);
                    Data.Data = JsonConvert.SerializeObject(Datos);

                    // var stringsql = JsonConvert.SerializeObject(String.Join(",", nue.Score));
                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("JSON_STR", Data.Data);
                    try
                    {
                        //Se ejecuta el procedimiento almacenado.
                        var resultado = await Task.FromResult(_connDWH.Get<string>(PortalQueryHelper.EncScoringDetalleScoringFlexDriver, parametros, commandType: CommandType.StoredProcedure));
                        result.Mensaje = resultado?.ToString();
                        result.Exitoso = true;
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                    }
                }
                catch (Exception ex)
                {
                    // result.error(ex.Message.ToString());
                    throw;
                }
            }

            return result;





        }
        #region PRUEBAS SIMCARD


        public async Task<ResultObject> PruebaSimCard()
        {
            ResultObject r = new();
            try
            {
                //Se ejecuta el procedimiento almacenado.
                var result = await Task.FromResult(_connDWH.GetAll<AssetsSimCardVM>(PortalQueryHelper.getAssetsProgramacion, null, commandType: CommandType.Text));

                if (result.Count > 0)
                {
                    foreach (var vehiculo in result.GroupBy(g => g.clienteid))
                    {
                        var lstVehiculos = vehiculo.Select(s => s.AssetId).ToList();
                        var posiciones = await _Mix.getPositions(lstVehiculos, 0);

                        var lstPosiciones = posiciones.Select(
                             s => new
                             {
                                 Placa = result.Find(f => f.AssetId == s.AssetId).Description,
                                 UltimoAvl = Constants.GetFechaServidor(s.Timestamp),
                                 Latitud = s.Latitude,
                                 Longitud = s.Longitude,
                                 FechaSistema = Constants.GetFechaServidor(),
                                 ProcesoGeneracionDatosId = result.Find(f => f.AssetId == s.AssetId).ProcesoGeneracionDatosId,
                                 Velocidad = s.SpeedKilometresPerHour
                             }
                             ).ToList();

                        var resultado = await _connDWH.Insert(PortalQueryHelper.insertPruebasSimCard, lstPosiciones, commandType: CommandType.Text);

                        r.Exitoso = (resultado > 0);
                    }
                }

            }
            catch (Exception ex)
            {
                r.error(ex.Message);

            }
            return r;
        }
        #endregion
        public async Task<List<dynamic>> getDynamicValueDWH(string Clase, string NombreConsulta, DynamicParameters lstparams)
        {
            try
            {
                string consulta = await _connCore.GetAsync<string>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase, NombreConsulta }, commandType: CommandType.Text);

                if (consulta != null && consulta.Length > 0)
                    //Se ejecuta el procedimiento almacenado.
                    return await Task.FromResult(_connDWH.GetAll<dynamic>(consulta, lstparams, commandType: CommandType.Text));

                else
                    throw new Exception("La consulta no se ha encontrado");

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<List<dynamic>> getDynamicValueProcedureDWH(string Clase, string NombreConsulta, DynamicParameters lstparams)
        {
            try
            {
                string consulta = await _connCore.GetAsync<string>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase, NombreConsulta }, commandType: CommandType.Text);

                if (consulta != null && consulta.Length > 0)
                    //Se ejecuta el procedimiento almacenado.
                    return await Task.FromResult(_connDWH.GetAll<dynamic>(consulta, lstparams));

                else
                    throw new Exception("La consulta no se ha encontrado");

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<int> Portal_RellenoInfomesViajesEventos(int clienteIdS, DateTime FechaInicial, DateTime FechaFinal)
        {
            return await _connDWH.ExecuteAsync("Portal.RellenoInfomesViajesEventos", new { PeriodoFecha = FechaInicial, clienteIdS, FechaInicial, FechaFinal });
        }

        public async Task<int> Portal_GetTokenPowerBI()
        {
            return await _connCore.GetAsync<dynamic>(PortalQueryHelper.getTokenPorTipo, new  { Tipo = "PBI" } );
        }
        public async Task<int> Portal_SetTokenPowerBI(string Token, DateTime ExpirationDate, bool isExists)
        {
            string _query;

            _query = (isExists) ? PortalQueryHelper.setTokenPorTipo : PortalQueryHelper.newTokenPorTipo;
            return await _connCore.ExecuteAsync(_query, new { Tipo = "PBI", Token , ExpirationDate });
        }

    }
    public interface IPortalMService
    {
        Task<ResultObject> Get_ViajesMetricasPorClientes(int? Clienteids, DateTime? FechaInicial, DateTime? FechaFinal);
        Task<ResultObject> Get_EventosPorClientes(int? ClienteIds, DateTime? FechaInicial, DateTime? FechaFinal);

        Task<ResultObject> Get_EventosActivosPorClientes(int? ClienteIds);

        Task<ResultObject> Get_PositionsByClient(int? Clienteids, int ProcesoGeneracionDatosId);
        Task<ResultObject> GetDetallesListas(int? ListaId, string Sigla);
        Task<List<long>> GetDriverxCliente(int ClienteId);
      
        Task<ResultObject> GuardarEncScoringDetalleScoringFlexDriver(int? ClienteIds, DateTime? FechaInicial, DateTime? FechaFinal, string aggregationPeriod = "Daily");
        Task<ResultObject> PruebaSimCard();

        Task<List<dynamic>> getDynamicValueDWH(string Clase, string NombreConsulta, DynamicParameters lstparams);
        Task<ResultObject> Get_PositionsByClientPositionsActive();
        Task<List<dynamic>> getDynamicValueProcedureDWH(string Clase, string NombreConsulta, DynamicParameters lstparams);
        Task<int> Portal_RellenoInfomesViajesEventos(int clienteIdS, DateTime FechaInicial, DateTime FechaFinal);
        Task<int> Portal_GetTokenPowerBI();
        Task<int> Portal_SetTokenPowerBI(string Token, DateTime ExpirationDate, bool isExists);
    }
}
