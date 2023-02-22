using Syscaf.ApiCore.Auth;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Service.Auth;
using Syscaf.Service.Drive;
using Syscaf.Service.eBus.Gcp;
using Syscaf.Service.Fatigue;
using Syscaf.Service.Peg;
using Syscaf.Service.Portal;
using SyscafWebApi.Service;

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
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<INotificacionService, NotificacionService>();
            services.AddTransient<IListaDetalleService, ListaDetalleService>();
            services.AddTransient<IMovilService, MovilService>();
            services.AddTransient<IArchivosService, ArchivosService>();
            services.AddTransient<IAdmService, AdmService>();
 
            services.AddTransient<IFatigueService, FatigueService>();
        }
    }
}
