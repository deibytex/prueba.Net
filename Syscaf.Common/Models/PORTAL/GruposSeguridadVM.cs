using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.PORTAL
{
    public class GruposDeSeguridadVM
    {
        public int? clienteIdS { get; set; }
        public string Nombre { get; set; }
        public bool? EsActivo { get; set; }
        public int? TipoSeguridadId { get; set; }
        public List<int> Sitios { get; set; } = new List<int> { };
        public bool? EsAdministrador { get; set; }
        public int? GrupoSeguridadId { get; set; }
    }
    public class GrupoSeguridadPostVM : GruposDeSeguridadVM
    {
        public int Clave { get; set; }
        public string Descripcion { get; set; }
        public List<string> Sites { get; set; } = new List<string> { };
    }
    public class UsuarioOrganizacionVM
    {
        public int UsuarioIds { get; set; }
        public string NombreUsuario { get; set; }

        public bool EsSeleccionado { get; set; }
    }
    public class OrganizacionVM
    {
        public int OrganizacionId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EsActivo { get; set; }

    }
    public class OrganizacionPostVM : OrganizacionVM
    {
       public DateTime FechaSistema { get; set; }
    }
    public class ListadoUsuariosVM
    {
        public int UsuarioId { get; set; }
    }

    public class GrupoSeguridadListaVM
    {
        public int GrupoSeguridadId { get; set; }
        public int? clienteIdS { get; set; }
        public string NombreGrupo { get; set; }
        public bool? EsActivo { get; set; }
    }
    public class SitiosVM
    {
        public string SiteId { get; set; }
        public string siteName { get; set; }
    }
    public class GruposSitiosVM
    {
        public string NombreGrupoSeguridad { get; set; }
        public int? GrupoSeguridadID { get; set; }
        public string SitioNombre { get; set; }
        public string siteIdS { get; set; }
        public bool? ActivoSitio { get; set; }
        public bool? ActivoGrupo { get; set; }
    }
    public class GruposSitiosSelectVM
    {
       
        public string SiteId { get; set; }
        public string SitioNombre { get; set; }
        public bool EsSeleccionado { get; set; }
    }

    public class SitiosGrupoSeguridadAsignacionVM
    {
        public int GrupoSeguridadId { get; set; }
        public string? ClienteId { get; set; }
        public List<string> SiteIds { get; set; } = new List<string> { };
        public int? TipoSeguridadId { get; set; }
        public DateTime? FechaSistema { get; set; }
        public bool? EsActivo { get; set; }
    }

    public class GrupoSeguirdadUsuarioSeleccionadosVM
    {
        public int? GrupoSeguridadId { get; set; }
        public string NombreGrupo { get; set; }
        public bool? EsActivo { get; set; }
        public bool? EsSeleccionado { get; set; }
    }
    public class UsuarioGrupoSeguridadVM
    {
        public string usuarioIdS { get; set; }
        public string? UsuarioId { get; set; }
        public List<int> GruposSeguridadIds { get; set; } = new List<int> { };
        public bool? EsActivo { get; set; }
        public DateTime? FechaSistema { get; set; }
    }

    public class GruposSeguridadClientesVM
    {
        public int? clienteIds { get; set; }
        public string ClienteId { get; set; }
        public string clienteNombre { get; set; }
        public string nit { get; set; }
        public int GrupoSeguridadId { get; set; }
        public string NombreGrupo { get; set; }
        public string Descripcion { get; set; }
        public bool? EsActivo { get; set; }
    }

    public class SitioClienteVM
    {
        public string SiteId { get; set; }
        public string sitioNombre { get; set; }
        public bool? EsSeleccionado { get; set; }
    }
}
