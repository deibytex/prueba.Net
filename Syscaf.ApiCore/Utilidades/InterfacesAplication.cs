using Syscaf.Common.Integrate.LogNotificaciones;

using SyscafWebApi.Service;
using Syscaf.Service.Portal;
using Syscaf.Common.Integrate.PORTAL;
using Syscaf.Service.RAG;

namespace Syscaf.Api.ApiCore.Utilidades
{
    public static class InterfacesAplication
    {
        public static void ConfigureServices(IServiceCollection services) {

            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IMixIntegrateService, MixIntegrateService>();
            services.AddScoped<IPortalService, PortalService>();
            services.AddScoped<IPortalMService,  PortalMService> ();
            services.AddScoped<IAssetsService, AssetsService>();
            services.AddScoped<ITransmisionService, TransmisionService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IEventTypeService, EventTypeService>();
            services.AddScoped<IDriverService, DriverService>();          
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<INotificacionService, NotificacionService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IProcesoGeneracionService, ProcesoGeneracionService>();
            services.AddScoped<IListaDetalleService, ListaDetalleService>();
            services.AddScoped<IGruposSeguridadService, GruposSeguridadService>();
            services.AddScoped<IRagService, RagService>();

            
        }
    }
}
