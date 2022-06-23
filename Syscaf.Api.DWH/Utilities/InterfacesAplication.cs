using Syscaf.Common.Integrate.LogNotificaciones;

using SyscafWebApi.Service;
using Syscaf.Service.Portal;
using Syscaf.Common.Integrate.PORTAL;

namespace Syscaf.Api.DWH.Utilities
{
    public static class InterfacesAplication
    {
        public static void ConfigureServices(IServiceCollection services) {

            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IMixIntegrateService, MixIntegrateService>();
            services.AddTransient<IPortalService, PortalService>();
            services.AddTransient<IPortalMService,  PortalMService> ();
            services.AddTransient<IAssetsService, AssetsService>();
            services.AddTransient<ITransmisionService, TransmisionService>();
            services.AddTransient<ISiteService, SiteService>();
            services.AddTransient<IEventTypeService, EventTypeService>();
            services.AddTransient<IDriverService, DriverService>();          
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<INotificacionService, NotificacionService>();
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IProcesoGeneracionService, ProcesoGeneracionService>();
            services.AddTransient<IListaDetalleService, ListaDetalleService>();
            //services.AddTransient<IGruposSeguridadService, GruposSeguridadService>();
        }
    }
}
