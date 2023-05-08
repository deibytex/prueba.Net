using MiX.Integrate.Shared.Entities.Assets;
using MiX.Integrate.Shared.Entities.DeviceConfiguration;
using MiX.Integrate.Shared.Entities.Drivers;
using MiX.Integrate.Shared.Entities.Events;
using MiX.Integrate.Shared.Entities.Groups;
using MiX.Integrate.Shared.Entities.LibraryEvents;
using MiX.Integrate.Shared.Entities.Locations;
using MiX.Integrate.Shared.Entities.Positions;
using MiX.Integrate.Shared.Entities.Scoring;
using MiX.Integrate.Shared.Entities.Trips;
using Syscaf.Common.Models;
using Syscaf.Common.PORTAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyscafWebApi.Service
{
    public interface IMixIntegrateService
    {
        Task<List<Group>> getClientes();
        Task<List<Asset>> getVehiculosAsync(long clienteId, int ClienteIds);
        Task<List<AdditionalDetails>> getAditionalDetailAssetsAsync(long clienteId, int ClienteIds);

        // trae la información de los conductores desde Mix
        Task<List<Driver>> getDrivers(long clienteId, int ClienteIds);
        Task<IList<Event>> GetEventosClientePorAssets(List<long> ll, List<long> eventosImportantes, DateTime FechaInicio, DateTime FechaFinal, int ClienteIds);
        Task<IList<Event>> GetEventosClientePorDriver(List<long> ll, List<long> eventosImportantes, DateTime FechaInicio, DateTime FechaFinal, int ClienteIds);
        Task<IList<Trip>> getViajes(List<long> ll, DateTime FechaInicio, DateTime FechaFinal, int ClienteIds, bool includesubtrips = true);
        Task<List<Event>> GetUltimosEventosCreadosPorOrganizacion(long organizacion, List<long> eventosImportantes, int ClienteIds);
        Task<List<ActiveEvent>> GetEventosActivosCreadosPorOrganizacion(long organizacion, List<long> eventosImportantes, int ClienteIds);
        Task<List<ActiveEvent>> GetEventosActivosHistoricalCreadosPorAssets(long groupId, List<long> assets, List<long> entityTypes, DateTime From, DateTime To);
        Task<IList<Event>> GetEventosPorAssets(List<long> ll, List<long> eventosImportantes, DateTime? cachedSince, int ClienteIds);
        Task<GroupSummary> getSitios(long clienteId, int ClienteIds);
        Task<List<Position>> getPositionsByGroups(List<long> groupsIds, int ClienteIds);
        Task<List<LibraryEvent>> getTipoEventos(long clienteId, int ClienteIds);
        Task<List<TripRibasMetrics>> GetMetricasPorDriver(List<long> drivers, DateTime FechaInicio, DateTime FechaFinal, int ClienteIds);
        Task<List<Trip>> GetUltimosViajesCreadosByOrganization(long organizacion, int ClienteIds, string methodP);
        Task<List<Event>> GetEventosCliente(List<long> ll, DateTime fechaInicial, DateTime fechaFinal, List<long> eventosImportantes, int ClienteIds);
        Task<List<ActiveEvent>> GetEventosActivosCreadosPorVehiculos(List<long> organizacion, int ClienteIds);
        Task<List<Position>> getLastPositionsByGroups(List<long> organizacion, int ClienteIds);
        Task<ProximityQueryResult> GetLocationsNearestByGroup(long organizacion, double Latitude, double Longitude, int ClienteIds);
        Task<List<Location>> GetLocationsInRangeByGroup(long organizacion, double Latitude, double Longitude, long meters, int ClienteIds);
        Task<List<Location>> GetLocationsByGroup(long organizacion, int ClienteIds);
        Task<List<Position>> getPositions(List<long> assetsId, int ClienteIds);
        Task<List<ActiveEvent>> GetEventosActivosHistoricalCreadosPorOrganizacion(long organizacion, List<long> eventosImportantes, int ClienteIds);
        Task<List<Event>> GetEventosClientePorOrganizacion(long OrganizacionId, DateTime fechaInicial, DateTime fechaFinal, List<long> eventosImportantes, int ClienteIds);

        Task<List<MobileUnitDeviceConfiguration>> GetMobileUnitDeviceConfigurationsByGroupId(long groupId, int ClienteIds);
        Task<List<MobileUnitCommunicationSettings>> GetCommunicationSettings(long groupId, List<long> AssetIds, int ClienteIds);
        Task<List<MobileUnitConfigurationState>> GetConfigurationState(long groupId, List<long> AssetIds, int ClienteIds);
        Task<List<AssetDiagnostics>> GetAssetDiagnostics(long groupId, List<long> AssetIds, int ClienteIds);
        Task<List<ReporteConfiguracion>> GetConfiguracionAsync(long groupId);
        Task<Report_FlexibleRAG> GetFlexibleRAGScoreReportAsync(List<long> drivers, string from, string to, string aggregationPeriod, int ClienteId, long organizacion);
    }
}