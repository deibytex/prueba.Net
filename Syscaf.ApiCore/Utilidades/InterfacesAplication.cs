using Syscaf.ApiCore.Auth;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Service.eBus.Gcp;
using Syscaf.Service.Portal;

namespace Syscaf.ApiCore.Utilidades
{
     public static class InterfacesAplication
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IeBusGcpService, eBusGcpService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IGruposSeguridadService, GruposSeguridadService>();
           
        }
    }
}
