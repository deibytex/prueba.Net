
using Microsoft.Extensions.Options;
using MiX.Integrate.Shared.Entities.Assets;
using MiX.Integrate.Shared.Entities.Carriers;
using MiX.Integrate.Shared.Entities.DeviceConfiguration;
using MiX.Integrate.Shared.Entities.Drivers;
using MiX.Integrate.Shared.Entities.Events;
using MiX.Integrate.Shared.Entities.Groups;
using MiX.Integrate.Shared.Entities.LibraryEvents;
using MiX.Integrate.Shared.Entities.Locations;
using MiX.Integrate.Shared.Entities.Positions;
using MiX.Integrate.Shared.Entities.Scoring;
using MiX.Integrate.Shared.Entities.Trips;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.PORTAL;
using Syscaf.Common.Models;
using Syscaf.Common.PORTAL;
using Syscaf.Common.Services;

using Syscaf.Service.ViewModels.PORTAL;
using SyscafWebApi.App_Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Help = Syscaf.Common.Helpers.Helpers;
namespace SyscafWebApi.Service
{


    public class MixIntegrateService : IMixIntegrateService
    {
        private readonly IPortalService _portalService;
        private readonly MixCredenciales _options;


        private readonly string AssemblyName = "Syscaf.Common.Services.MixServiceConn, Syscaf.Common";
        private DateTime FechaActual
        {
            get
            {
                return Constants.GetFechaServidor();
            }
        }

        // private IMemoryCache _cache;
        private List<CredencialesMixVM> _crendeciales = null;
        public MixIntegrateService(IPortalService _portalService
            , IOptions<MixCredenciales> _options)
        {
            this._portalService = _portalService;
            this._options = _options.Value;

            //  _cache = memoryCache;
        }

        private CredencialesMixVM getCredenciales(int ClienteIds)
        {
            if (_crendeciales == null)
            {
                _crendeciales = _portalService.GetCredencialesClientes();
            }
            var credencial = _crendeciales.Where(s => s.clientesId.Split(',').ToList().Exists(e => e == ClienteIds.ToString())).FirstOrDefault();
            if (credencial == null)
                credencial = _crendeciales.Find(f => f.ClienteId == 0);
            return credencial;
        }

        private async void validateCalls(int credencialid, DateTime date)
        {
            var llamadas = _portalService.GetCallsMethod(credencialid);

            if (llamadas.Count > 0)
            {
                var call = llamadas.First();

                DateTime fechaCall = call.DateCall ?? date;
                DateTime fechaCallH = call.dateHour ?? date;

                call.TotalCalls++;
                call.TotalCallsHour++;
                int diffMin = (int)(FechaActual - fechaCall).TotalSeconds;
                // valida las llamadas por minuto
                if ((call.TotalCalls + 1 >= (Constants.CallsMin)))
                {
                    if (diffMin < 60)
                        Thread.Sleep((Constants.SecondMinute + 1 - diffMin) * 1000); // dureme por los segundos que haga falta 
                    call.DateCall = FechaActual;
                    call.TotalCalls = 0;

                }

                // valida las llamadas por hora
                if (call.TotalCallsHour >= (Constants.CallsHour))
                {
                    int diifH = (int)(FechaActual - fechaCallH).TotalSeconds;
                    if (diifH < 3600)
                        Thread.Sleep(((3600 - diifH)) * 1000); // si las llamadas son mas de 500 por hora duerme hasta la siguiente ejecucion

                    call.dateHour = FechaActual;
                    call.TotalCallsHour = 0;
                }


                await _portalService.SetCallsMethod(credencialid, call.DateCall ?? FechaActual, call.dateHour ?? FechaActual, call.TotalCallsHour, call.TotalCalls);
            }
        }
        public async Task<IList<Event>> GetEventosClientePorDriver(List<long> ll, List<long> eventosImportantes, DateTime FechaInicio, DateTime FechaFinal, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetEventosClientePorDriver", new object[] { ll, eventosImportantes, FechaInicio, FechaFinal });
            return (result.Exitoso) ? (IList<Event>)result.Data : null;
        }

        public async Task<List<MiX.Integrate.Shared.Entities.Groups.Group>> getClientes()
        {
            MixServiceVM result = await invokeMethodAsync(-1, AssemblyName, "getClientes", null);
            return (result.Exitoso) ? (List<MiX.Integrate.Shared.Entities.Groups.Group>)result.Data : null;
        }
        public async Task<IList<Event>> GetEventosClientePorAssets(List<long> ll, List<long> eventosImportantes, DateTime FechaInicio, DateTime FechaFinal, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetEventosClientePorAssets", new object[] { ll, eventosImportantes, FechaInicio, FechaFinal });
            return (result.Exitoso) ? (IList<Event>)result.Data : null;
        }

        public async Task<IList<Event>> GetEventosPorAssets(List<long> ll, List<long> eventosImportantes, DateTime? cachedSince, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetEventosPorAssets", new object[] { ll, eventosImportantes, cachedSince });
            return (result.Exitoso) ? (IList<Event>)result.Data : null;
        }

        public async Task<GroupSummary> getSitios(long clienteId, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "getSitios", new object[] { clienteId });
            return (result.Exitoso) ? (GroupSummary)result.Data : null;
        }

        // trae la información de los vehiculos desde Mix
        public async Task<List<Asset>> getVehiculosAsync(long clienteId, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "getVehiculos", new object[] { clienteId });
            return (result.Exitoso) ? (List<Asset>)result.Data : null;

        }

        public async Task<List<Position>> getPositionsByGroups(List<long> groupsIds, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "getPositionsByGroups", new object[] { groupsIds });
            return (result.Exitoso) ? (List<Position>)result.Data : null;

        }
        // trae la información de los conductores desde Mix
        public async Task<List<Driver>> getDrivers(long clienteId, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "getDrivers", new object[] { clienteId });
            return (result.Exitoso) ? (List<Driver>)result.Data : null;
        }
        public async Task<List<LibraryEvent>> getTipoEventos(long clienteId, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "getTipoEventos", new object[] { clienteId });
            return (result.Exitoso) ? (List<LibraryEvent>)result.Data : null;
        }

        public async Task<IList<Trip>> getViajes(List<long> ll, DateTime FechaInicio, DateTime FechaFinal, int ClienteIds, bool includesubtrips = true)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "getViajes", new object[] { ll, FechaInicio, FechaFinal, includesubtrips });
            return (result.Exitoso) ? (IList<Trip>)result.Data : null;
        }
        public async Task<List<TripRibasMetrics>> GetMetricasPorDriver(List<long> drivers, DateTime FechaInicio, DateTime FechaFinal, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetMetricasPorDriver", new object[] { drivers, FechaInicio, FechaFinal });
            return (result.Exitoso) ? (List<TripRibasMetrics>)result.Data : null;
        }

        public async Task<List<Trip>> GetUltimosViajesCreadosByOrganization(long organizacion, int ClienteIds, string methodP)
        {
            string method = methodP == null ? "Trips.GetCreatedSinceForOrganisation" : methodP;
            string sinceToken = _portalService.GetTokenClientes(ClienteIds, method);
           
            byte cantidad = 100;
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetUltimosViajesCreadosByOrganization", new object[] { organizacion, sinceToken, cantidad });
            CreatedSinceResult<Trip> datos = (CreatedSinceResult<Trip>)result.Data;
            _portalService.GetTokenClientes(ClienteIds, method, datos.GetSinceToken);
            return datos.Items;
        }
        public async Task<List<Event>> GetUltimosEventosCreadosPorOrganizacion(long organizacion, List<long> eventosImportantes, int ClienteIds)
        {
            string method = "Event.GetCreatedSinceForOrganisation";
            string sinceToken = _portalService.GetTokenClientes(ClienteIds, method);

           
            byte cantidad = 100;
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetUltimosEventosCreadosPorOrganizacion", new object[] { organizacion, eventosImportantes, sinceToken, cantidad });
            CreatedSinceResult<Event> datos = (CreatedSinceResult<Event>)result.Data;
            _portalService.GetTokenClientes(ClienteIds, method, datos.GetSinceToken);
            return datos.Items;
        }

        public async Task<List<ActiveEvent>> GetEventosActivosCreadosPorOrganizacion(long organizacion, List<long> eventosImportantes, int ClienteIds)
        {
            string method = "Active.GetCreatedSinceForOrganisation";
            string sinceToken = _portalService.GetTokenClientes(ClienteIds, method);

            byte cantidad = 100;


            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetEventosActivosCreadosPorOrganizacion", new object[] { organizacion, eventosImportantes, sinceToken, cantidad });
            CreatedSinceResult<ActiveEvent> datos = (CreatedSinceResult<ActiveEvent>)result.Data;
            _portalService.GetTokenClientes(ClienteIds, method, datos.GetSinceToken);
            return datos.Items;
        }
        public async Task<List<ActiveEvent>> GetEventosActivosCreadosPorVehiculos(List<long> organizacion, int ClienteIds)
        {
            string method = "Active.GetCreatedSinceForOrganisation";
            string sinceToken = _portalService.GetTokenClientes(ClienteIds, method);

           
            byte cantidad = 100;


            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetEventosActivosCreadosPorVehiculo", new object[] { organizacion, sinceToken, cantidad });
            CreatedSinceResult<ActiveEvent> datos = (CreatedSinceResult<ActiveEvent>)result.Data;
            _portalService.GetTokenClientes(ClienteIds, method, datos.GetSinceToken);
            return datos.Items;
        }
        public async Task<List<ActiveEvent>> GetEventosActivosHistoricalCreadosPorAssets(long groupId, List<long> assets, List<long> entityTypes, DateTime From, DateTime To)
        {
            MixServiceVM result = await invokeMethodAsync(-1, AssemblyName, "GetEventosActivosHistoricalCreadosPorAssets", new object[] { assets, entityTypes, From, To });
            return (List<ActiveEvent>)result.Data;
        }

        public async Task<List<Position>> getLastPositionsByGroups(List<long> organizacion, int ClienteIds)
        {
            string method = "getLastPositionsByGroups";
            string sinceToken = _portalService.GetTokenClientes(ClienteIds, method);           

            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "getLastPositionsByGroups", new object[] { organizacion, sinceToken });
            CreatedSinceResult<Position> datos = (CreatedSinceResult<Position>)result.Data;
            _portalService.GetTokenClientes(ClienteIds, method, datos.GetSinceToken);
            return datos.Items;
        }



        public async Task<List<Event>> GetEventosCliente(List<long> ll, DateTime fechaInicial, DateTime fechaFinal, List<long> eventosImportantes, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetEventosCliente", new object[] { ll, fechaInicial, fechaFinal, eventosImportantes });
            return (result.Exitoso) ? (List<Event>)result.Data : null;
        }

        public async Task<List<Event>> GetEventosClientePorOrganizacion(long OrganizacionId, DateTime fechaInicial, DateTime fechaFinal, List<long> eventosImportantes, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetEventosClientePorOrganizacion", new object[] { OrganizacionId, fechaInicial, fechaFinal, eventosImportantes });
            return (result.Exitoso) ? (List<Event>)result.Data : null;
        }

        public async Task<List<Location>> GetLocationsByGroup(long organizacion, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetLocationsByGroup", new object[] { organizacion });
            return (List<Location>)result.Data;
        }

        public async Task<List<Location>> GetLocationsInRangeByGroup(long organizacion, double Latitude, double Longitude, long meters, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetLocationsInRangeByGroup", new object[] { organizacion, Latitude, Longitude, meters });
            return (List<Location>)result.Data;
        }


        public async Task<ProximityQueryResult> GetLocationsNearestByGroup(long organizacion, double Latitude, double Longitude, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetLocationsNearestByGroup", new object[] { organizacion, Latitude, Longitude });
            return (ProximityQueryResult)result.Data;
        }

        public async Task<List<Position>> getPositions(List<long> assetsId, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "getPositions", new object[] { assetsId });
            return (List<Position>)result.Data;
        }

        public async Task<List<ActiveEvent>> GetEventosActivosHistoricalCreadosPorOrganizacion(long organizacion, List<long> eventosImportantes, int ClienteIds)
        {
            byte cantidad = 100;
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetEventosActivosHistoricalCreadosPorOrganizacion", new object[] { organizacion, eventosImportantes, cantidad });
            return (List<ActiveEvent>)result.Data;
        }

        public async Task<List<MobileUnitDeviceConfiguration>> GetMobileUnitDeviceConfigurationsByGroupId(long groupId, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetMobileUnitDeviceConfigurationsByGroupId", new object[] { groupId });
            return (List<MobileUnitDeviceConfiguration>)result.Data;
        }

        public async Task<List<MobileUnitCommunicationSettings>> GetCommunicationSettings(long groupId, List<long> AssetIds, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetCommunicationSettings", new object[] { groupId, AssetIds });
            return (List<MobileUnitCommunicationSettings>)result.Data;
        }
        public async Task<List<MobileUnitConfigurationState>> GetConfigurationState(long groupId, List<long> AssetIds, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetConfigurationState", new object[] { groupId, AssetIds });
            return (List<MobileUnitConfigurationState>)result.Data;
        }

        public async Task<List<AssetDiagnostics>> GetAssetDiagnostics(long groupId, List<long> AssetIds, int ClienteIds)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteIds, AssemblyName, "GetAssetDiagnostics", new object[] { groupId, AssetIds });
            return (List<AssetDiagnostics>)result.Data;
        }
        public async Task<List<ReporteConfiguracion>> GetConfiguracionAsync(long groupId)
        {
            MixServiceVM result = await invokeMethodAsync(-1, AssemblyName, "GetConfiguracionAsync", new object[] { groupId });
            return (List<ReporteConfiguracion>)result.Data;
        }

        private async Task<MixServiceVM> invokeMethodAsync(int ClienteIds, string clase, string methodo, object[] parametros)
        {
            try
            {


                //obtiene las credenciales por clienteids
                var credencial = getCredenciales(ClienteIds);
                int credencialid = credencial.Id;


                Type dynamicType = Type.GetType(clase);

                //Stopwatch lo usamos para saber cuanto duro eb darme respuesta el metodo
                Stopwatch s = new Stopwatch();
                var obj = Activator.CreateInstance(dynamicType, new object[] { _options, credencial.UserId, credencial.Password });

                //validar las llamadas por credencial
                //  validateCalls(credencialid, FechaActual);
                s.Start();
                // obtenemos el nombre del metodo de la instancia 
                MethodInfo method = dynamicType.GetMethod(methodo);
                // guardamos el momento cuando invocamoes el metodo
                s.Stop();



                Task<MixServiceVM> PreResult = (Task<MixServiceVM>)method.Invoke(obj, parametros);
                var result = await PreResult;
                _portalService.SetLog(credencialid, methodo, result.StatusCode, result.Response, FechaActual, ClienteIds);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task<Report_FlexibleRAG> GetFlexibleRAGScoreReportAsync(List<long> drivers, string from, string to, string aggregationPeriod, int ClienteId, long Organizacion)
        {
            MixServiceVM result = await invokeMethodAsync(ClienteId, AssemblyName, "GetFlexibleRAGScoreReportAsync", new object[] { drivers, from, to,  aggregationPeriod, Organizacion });
            return (result.Exitoso) ? (Report_FlexibleRAG)result.Data : null;
        }

    }
}