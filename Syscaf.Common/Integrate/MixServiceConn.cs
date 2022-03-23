using Microsoft.Extensions.Options;
using MiX.Integrate.API.Client;
using MiX.Integrate.Shared.Entities.Events;
using Syscaf.Common.Helpers;
using Syscaf.Common.Models;
using Syscaf.Common.PORTAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Syscaf.Common.Services
{
    public class MixServiceConn : IMixServiceConn
    {

        private string _user = "";
        private string _pws = "";
        private readonly MixCredenciales _options;
      
        public MixServiceConn(IOptions<MixCredenciales> options ,string _user, string _pws)
        {
            this._options = options.Value;
            this._user = _user;
            this._pws = _pws;
        }
        // variables de solo lectura 
        private string ApiBaseUrl
        {
            get
            {
                return _options.ApiUrl;
            }
        }

        private MixServiceVM ReturnSuccess(object data) {
            return new MixServiceVM() { Exitoso = true, StatusCode = 200, Data = data };
        }
        private MixServiceVM ReturnError(int code, string response)
        {
            return new MixServiceVM() { Exitoso = true, StatusCode = code, Response = response };
        }

        private IdServerResourceOwnerClientSettings ClientSettings
        {
            get
            {
                return new IdServerResourceOwnerClientSettings()
                {
                    BaseAddress = _options.IdentityServerBaseAddress,
                    ClientId = _options.IdentityServerClientId,
                    ClientSecret = _options.IdentityServerClientSecret,
                    UserName = _user.Trim(),
                    Password = _pws.Trim(),
                    Scopes = _options.IdentityServerScopes
                };
            }
        }

     
      
        // trae la información de los eventos de los clientes
        // Params:  ll 
        // eventos importante: 
        public async Task<MixServiceVM> GetEventosClientePorDriver(List<long> ll, List<long> eventosImportantes, DateTime FechaInicio, DateTime FechaFinal)
        {
            try
            {
                var eventsClient = new EventsClient(ApiBaseUrl, ClientSettings);
                var result = await eventsClient.GetRangeForDriversAsync(ll, FechaInicio, FechaFinal, eventosImportantes);

                return ReturnSuccess(result);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }

        public async Task<MixServiceVM> GetEventosClientePorAssets(List<long> ll, List<long> eventosImportantes, DateTime FechaInicio, DateTime FechaFinal)
        {
            try
            {
                var eventsClient = new EventsClient(ApiBaseUrl, ClientSettings);
                var result = await eventsClient.GetRangeForGroupsAsync(ll, "Asset", FechaInicio, FechaFinal, eventosImportantes);

                return ReturnSuccess(result);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }

        public async Task<MixServiceVM> GetEventosPorAssets(List<long> ll, List<long> eventosImportantes, DateTime? cachedSince)
        {
            try
            {
                var eventsClient = new EventsClient(ApiBaseUrl, ClientSettings);
                var result = await eventsClient.GetLatestForAssetsAsync(ll,  eventTypeIds: eventosImportantes, cachedSince: cachedSince);

                return ReturnSuccess(result);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }


        // trae la información de los clientes desde Mix
        public async Task<MixServiceVM> getClientes()
        {
            try
            {
                var groupsClient = new GroupsClient(ApiBaseUrl, ClientSettings);
                return ReturnSuccess(await groupsClient.GetAvailableOrganisationsAsync());
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }
        // trae la información de los sitios desde Mix
        public async Task<MixServiceVM> getSitios(long groupId)
        {
            try
            {
                var groupsClient = new GroupsClient(ApiBaseUrl, ClientSettings);
                return ReturnSuccess(await groupsClient.GetSubGroupsAsync(groupId));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }
        // trae la información de los vehiculos desde Mix
        public async Task<MixServiceVM> getVehiculos(long clienteId)
        {
            try
            {
                var assetsClient = new AssetsClient(ApiBaseUrl, ClientSettings);
                var result = await assetsClient.GetAllAsync(clienteId);
                return ReturnSuccess(result);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }
        // trae la información de las posiciones desde Mix
        public async Task<MixServiceVM> getPositions(List<long> assetsId)
        {
            try
            {
                var positionsClient = new PositionsClient(ApiBaseUrl, ClientSettings);
              
                return ReturnSuccess(await positionsClient.GetLatestByAssetIdsAsync(assetsId, 1, Helpers.Helpers.GetFechaServidor().Date));
            }
            catch (Exception exp)
            {
                throw;
            }



        }

        public async Task<MixServiceVM> getPositionsByGroups(List<long> groupsIds)
        {
            try
            {
                var positionsClient = new PositionsClient(ApiBaseUrl, ClientSettings);          
                return ReturnSuccess(await positionsClient.GetLatestByGroupIdsAsync(groupsIds, 1));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }



        }

        public async Task<MixServiceVM> getLastPositionsByGroups(List<long> groupsIds, string sinceToken)
        {
            try
            {
                var positionsClient = new PositionsClient(ApiBaseUrl, ClientSettings);
                return ReturnSuccess(await positionsClient.GetCreatedSinceForGroupsAsync(groupsIds, "Asset", sinceToken,100));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }



        }
        // trae la información de los conductores desde Mix
        public async Task<MixServiceVM> getDrivers(long clienteId)
        {
            try
            {
                var driverClient = new DriversClient(ApiBaseUrl, ClientSettings);              
                return ReturnSuccess(await driverClient.GetAllDriversAsync(clienteId));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }
        }
        // trae la información de los tipos de eventos desde Mix
        public async Task<MixServiceVM> getTipoEventos(long clienteId)
        {
            try
            {
                var eventsClient = new LibraryEventsClient(ApiBaseUrl, ClientSettings);                
                return ReturnSuccess(await eventsClient.GetAllLibraryEventsAsync(clienteId));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }
        }
        // trae la información de los viajes desde Mix
      

        // trae la información de los viajes desde Mix
        public async Task<MixServiceVM> getViajes(List<long> clientes, DateTime FechaInicio, DateTime FechaFinal, bool incluedesubtrips)
        {
            try
            {
                var tripsClient = new TripsClient(ApiBaseUrl, ClientSettings);                 
                return ReturnSuccess( await tripsClient.GetRangeForGroupsAsync(clientes, "Asset", FechaInicio, FechaFinal, incluedesubtrips));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }

        public async Task<MixServiceVM> GetMetricasPorDriver(List<long> drivers, DateTime FechaInicio, DateTime FechaFinal)
        {
            try
            {
                var tripsClient = new TripsClient(ApiBaseUrl, ClientSettings);        
                return ReturnSuccess(await tripsClient.GetRibasMetricsForDriversAsync(drivers, FechaInicio, FechaFinal));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }


        }
        public async Task<MixServiceVM> GetUltimosViajesCreadosByOrganization(long organizationId, string token, int cantidad = 1000)
        {
            try
            {
                var tripsClient = new TripsClient(ApiBaseUrl, ClientSettings);
                // trae la informacion de viajes  desde el momento que fue creado
                var createsinceresult = await tripsClient.GetCreatedSinceForOrganisationAsync(organizationId, token, cantidad, true);
                return ReturnSuccess(createsinceresult);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }


        public async Task<MixServiceVM> GetUltimosEventosCreadosPorOrganizacion(long organizacionId, List<long> entityTypes, string token, byte cantidad = 100)
        {
            try
            {               
                var tripsClient = new EventsClient(ApiBaseUrl, ClientSettings);
                // trae la informacion de viajes  desde el momento que fue creado
                var createsinceresult = await tripsClient.GetCreatedSinceForOrganisationFilteredAsync(organizacionId, eventTypeIds: entityTypes, sinceToken: token, quantity: 1000);

                return ReturnSuccess(createsinceresult);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }
        public async Task<MixServiceVM> GetEventosActivosCreadosPorVehiculo(List<long> vehiculos,  string token, byte cantidad = 100)
        {
            try
            {
                var tripsClient = new ActiveEventsClient(ApiBaseUrl, ClientSettings);
                // trae la informacion de viajes  desde el momento que fue creado
                var createsinceresult = await tripsClient.GetCreatedSinceForAssetsAsync(vehiculos,  sinceToken: token, quantity: 1000);

                return ReturnSuccess(createsinceresult);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }

        public async Task<MixServiceVM> GetEventosActivosCreadosPorOrganizacion(long organizacionId, List<long> entityTypes, string token, byte cantidad = 100)
        {
            try
            {               
                var tripsClient = new ActiveEventsClient(ApiBaseUrl, ClientSettings);
                // trae la informacion de viajes  desde el momento que fue creado
                var createsinceresult = await tripsClient.GetCreatedSinceForOrganisationFilteredAsync(organizacionId, eventTypeIds: entityTypes, sinceToken: token, quantity: 1000);
                return ReturnSuccess(createsinceresult);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }


        public async Task<MixServiceVM> GetEventosActivosHistoricalCreadosPorOrganizacion(long organizacionId, List<long> entityTypes,  byte cantidad = 100)
        {
            try
            {
                var tripsClient = new ActiveEventsClient(ApiBaseUrl, ClientSettings);
                // trae la informacion de viajes  desde el momento que fue creado
                var createsinceresult = await tripsClient.GetLatestForOrganisationAsync(organizacionId, eventTypeIds: entityTypes, quantity: cantidad);
                return ReturnSuccess(createsinceresult);
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }

        }



        public async Task<MixServiceVM> GetEventosCliente(List<long> ll, DateTime fechaInicial, DateTime fechaFinal, List<long> eventosImportantes)
        {
            try
            {
                var eventsClient = new EventsClient(ApiBaseUrl, ClientSettings);         
                return ReturnSuccess(await eventsClient.GetRangeForAssetsAsync(ll, fechaInicial, fechaFinal, eventosImportantes));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }
        }
        public async Task<MixServiceVM> GetEventosClientePorOrganizacion(long OrganizacionId, DateTime fechaInicial, DateTime fechaFinal, List<long> eventosImportantes)
        {
            try
            {
                var eventsClient = new EventsClient(ApiBaseUrl, ClientSettings);
                return ReturnSuccess(await eventsClient.GetRangeForGroupsAsync(new List<long>() { OrganizacionId}, "Asset", fechaInicial, fechaFinal, eventTypeIds: eventosImportantes));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }
        }


        public List<ReporteConfiguracion> GetConfiguracion(long groupid)
        {
            try
            {
                _user = _options.IdentityServerUserName;
                _pws = _options.IdentityServerPassword;
                //   MobileUnitDeviceConfiguration
                // groupid = -5205654157038870688;
                var deviceCofiguration = new DeviceConfigurationClient(ApiBaseUrl, ClientSettings);

                var assets = new AssetsClient(ApiBaseUrl, ClientSettings);

                var lastTrip = new TripsClient(ApiBaseUrl, ClientSettings);
                // configuracion de gprs por clientes 
                var configuracion = deviceCofiguration.GetMobileUnitDeviceConfigurationsByGroupId(groupid);

                var asset = assets.GetAll(groupid);

                DateTime fechabase = new DateTime(1970, 01, 01, 00, 00, 00);

                if (configuracion.Count > 0)
                {
                    // vehiculos con configuracion
                    var assetsClientes = configuracion.Select(s => long.Parse(s.AssetId)).ToList();

                    // traemos a informacion de configuracion de gprs
                    var MobileUnitCommunicationSettings = deviceCofiguration.GetCommunicationSettings(groupid, assetsClientes);

                    // taremes la configuration sate
                    var configuracionstate = deviceCofiguration.GetConfigurationState(groupid, assetsClientes);

                    var diagnostic = assets.GetAssetDiagnostics(groupid, assetsClientes);                  
                    // traemos los ultimos viajes de cliente
                    var ultimosViajes = lastTrip.GetLatestForAssets(assetsClientes).ToList();

                    return configuracion.Select(s => new ReporteConfiguracion
                    {
                        VehicleID = s.AssetId,
                        UnitIMEI = s.Properties?.Find(f => f.Name.Equals("UnitIMEI"))?.Value,
                        UnitSCID = s.Properties?.Find(f => f.Name.Equals("UnitSCID"))?.Value + "_",
                        GPRSContext = MobileUnitCommunicationSettings.Find(f => f.MobileUnitId == long.Parse(s.AssetId))?.GprsContext,
                        LastTrip = Helpers.Helpers.GetFechaServidor(ultimosViajes.Find(f => f.AssetId == long.Parse(s.AssetId))?.TripStart),
                        DeviceType = s.MobileDeviceType,
                        CreatedDate = Helpers.Helpers.GetFechaServidor(Convert.ToDateTime(asset.Find(f => f.AssetId == long.Parse(s.AssetId)).CreatedDate.ToString("d")),true).Value.ToString(Constants.FormatoHoraPacifico, Constants.CultureDate),
                        ConfigurationGroup = s.ConfigurationGroup,
                        DriverOBC = configuracionstate.Find(f => f.MobileUnitId == long.Parse(s.AssetId))?.DriverSetVersion,
                        DriverOBCLoadDate = Helpers.Helpers.GetFechaServidor(fechabase.AddSeconds(Convert.ToInt32(configuracionstate.Find(f => f.MobileUnitId == long.Parse(s.AssetId))?.DriverSetLoadDate)), true).Value.ToString(Constants.FormatoHoraPacifico, Constants.CultureDate),
                        DriverCAN = diagnostic.Find(f => f.AssetId == long.Parse(s.AssetId)).DDMVersion,
                        LastConfiguration = Helpers.Helpers.GetFechaServidor(diagnostic.Find(f => f.AssetId == long.Parse(s.AssetId)).LastConfig, true).Value.ToString(Constants.FormatoHoraPacifico, Constants.CultureDate)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                string exp = ex.ToString();
            }
            return null;
        }

        public async Task<MixServiceVM> GetLocationsByGroup(long groupId)
        {
            try
            {
                var client = new LocationsClient(ApiBaseUrl, ClientSettings);
                return ReturnSuccess(await client.GetAllAsync(groupId,true, true));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }
        }
        public async Task<MixServiceVM> GetLocationsInRangeByGroup(long groupId, double Latitude, double Longitude,long meters)
        {
            try
            {
                var client = new LocationsClient(ApiBaseUrl, ClientSettings);
                return ReturnSuccess(await client.InRangeAsync(groupId, new Coordinate() {
                    Latitude = Latitude,
                    Longitude = Longitude
                }, meters));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }
        }
        public async Task<MixServiceVM> GetLocationsNearestByGroup(long groupId, double Latitude, double Longitude)
        {
            try
            {
                var client = new LocationsClient(ApiBaseUrl, ClientSettings);
                return ReturnSuccess(await client.NearestAsync(groupId, new Coordinate()
                {
                    Latitude = Latitude,
                    Longitude = Longitude
                }));
            }
            catch (Exception ex)
            {
                return ReturnError(ex.HResult, ex.ToString());
            }
        }

    }

    public interface IMixServiceConn
    {

        Task<MixServiceVM> GetEventosClientePorDriver(List<long> ll, List<long> eventosImportantes, DateTime FechaInicio, DateTime FechaFinal);
        Task<MixServiceVM> GetEventosClientePorAssets(List<long> ll, List<long> eventosImportantes, DateTime FechaInicio, DateTime FechaFinal);

        Task<MixServiceVM> getClientes();
        Task<MixServiceVM> getSitios(long groupId);
        Task<MixServiceVM> getVehiculos(long clienteId);
        Task<MixServiceVM> getPositions(List<long> assetsId);
        Task<MixServiceVM> getPositionsByGroups(List<long> groupsIds);
        Task<MixServiceVM> getDrivers(long clienteId);
        Task<MixServiceVM> getTipoEventos(long clienteId);

        Task<MixServiceVM> getViajes(List<long> clientes, DateTime FechaInicio, DateTime FechaFinal, bool incluedesubtrips);
        Task<MixServiceVM> GetMetricasPorDriver(List<long> drivers, DateTime FechaInicio, DateTime FechaFinal);
        Task<MixServiceVM> GetUltimosViajesCreadosByOrganization(long organizationId, string token, int cantidad = 1000);
        Task<MixServiceVM> GetUltimosEventosCreadosPorOrganizacion(long organizacionId, List<long> entityTypes, string token, byte cantidad = 100);
        Task<MixServiceVM> GetEventosActivosCreadosPorOrganizacion(long organizacionId, List<long> entityTypes, string token, byte cantidad = 100);
        Task<MixServiceVM> GetEventosActivosCreadosPorVehiculo(List<long> vehiculos, string token, byte cantidad = 100);
        Task<MixServiceVM> GetEventosCliente(List<long> ll, DateTime fechaInicial, DateTime fechaFinal, List<long> eventosImportantes);
        List<ReporteConfiguracion> GetConfiguracion(long groupid);
        Task<MixServiceVM> GetEventosPorAssets(List<long> ll, List<long> eventosImportantes, DateTime? cachedSince);
        Task<MixServiceVM> getLastPositionsByGroups(List<long> groupsIds, string sinceToken);
        Task<MixServiceVM> GetLocationsByGroup(long groupId);
        Task<MixServiceVM> GetLocationsInRangeByGroup(long groupId, double Latitude, double Longitude, long meters);
        Task<MixServiceVM> GetLocationsNearestByGroup(long groupId, double Latitude, double Longitude);
        Task<MixServiceVM> GetEventosActivosHistoricalCreadosPorOrganizacion(long organizacionId, List<long> entityTypes, byte cantidad = 100);

        Task<MixServiceVM> GetEventosClientePorOrganizacion(long OrganizacionId, DateTime fechaInicial, DateTime fechaFinal, List<long> eventosImportantes);
    }
}