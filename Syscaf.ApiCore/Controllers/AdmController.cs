using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syscaf.ApiCore.Controllers;
using Syscaf.ApiCore.DTOs;
using Syscaf.ApiCore.Utilidades;
using Syscaf.Common.Models.PORTAL;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;
using System.ComponentModel.DataAnnotations;

namespace Syscaf.ApiTx.Controllers
{
    /// <summary>
    /// Controlador de transmisión
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdmController : BaseController
    {
        private readonly IGruposSeguridadService _GruposSeguridad;
        private readonly IAdmService _admService;
   
        public AdmController(IGruposSeguridadService _GruposSeguridad, IAdmService _admService)
        {
            this._GruposSeguridad = _GruposSeguridad;
            this._admService = _admService;
        }
        /// <summary>
        /// Obtiene la lista de organizaciones.
        /// </summary>
        /// <param name="OrganzacionId"></param>
        /// <returns></returns>
        [HttpGet("GetOrganizaciones")]
        public async Task<DataTableVM> GetListaOrganizaciones(int? OrganzacionId)
        {
            return await _GruposSeguridad.GetListaOrganizaciones(OrganzacionId);
        }
        /// <summary>
        /// Se obtiene el listado de usuarios si es por organizacion, true o false si esta seleccionado.
        /// </summary>
        /// <param name="OrganzacionId"></param>
        /// <returns></returns>
        [HttpGet("GetUsuarioOrganizacion")]
        public async Task<ResultObject> GetUsuarioOrganizacion([Required] int OrganzacionId)
        {
            return await _GruposSeguridad.GetListadoUsuarioOrganizacion(OrganzacionId);
        }
        /// <summary>
        /// Guarda o modifica una organizacion
        /// </summary>
        /// <param name="OrganizacionId"></param>
        /// <param name="Nombre"></param>
        /// <param name="Descripcion"></param>
        /// <param name="EsActivo"></param>
        /// <param name="FechaSistema"></param>
        /// <returns></returns>
        [HttpPost("GuardarOrganizacion")]
        public async Task<ResultObject> GuardarOrganizacion(int OrganizacionId, [Required] string Nombre, [Required] string Descripcion, [Required]  bool EsActivo, [Required] DateTime FechaSistema)
        {
            OrganizacionPostVM Organzacion = new OrganizacionPostVM
            {
                Nombre = Nombre,
                Descripcion = Descripcion,
                EsActivo = EsActivo,
                FechaSistema = FechaSistema,
                OrganizacionId = OrganizacionId
            };
            return await _GruposSeguridad.GuardarOrganizacion(Organzacion);
        }
        /// <summary>
        /// Cambia el estado de las organizaciones
        /// </summary>
        /// <param name="OrganizacionId"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        [HttpGet("setEstadosOrganizacion")]
        public async Task<ResultObject> setEstadosOrganizacion([Required] int OrganizacionId,  [Required] bool EsActivo)
        {
            return await _GruposSeguridad.setEstadosOrganizacion(OrganizacionId, EsActivo);
        }
        /// <summary>
        /// Se asigna los usuarios a la organizacion
        /// </summary>
        /// <param name="OrganizacionId"></param>
        /// <param name="Usuarios"></param>
        /// <returns></returns>
        [HttpGet("setUsuariosOrganizaciones")]
        public async Task<ResultObject> SetListadoUsuariosOrganizaciones([Required] int OrganizacionId,[FromBody] [Required] List<ListadoUsuariosVM> Usuarios)
        {
            return await _GruposSeguridad.SetListadoUsuariosOrganizaciones(OrganizacionId, Usuarios);
        }
        /// <summary>
        /// Se lista el grupo de seguridad por usuario
        /// </summary>
        /// <param name="UsuarioId"></param>
        /// <returns></returns>
        [HttpGet("ListarGruposSeguridadUsuarios")]
        public async Task<ResultObject> ListarGruposSeguridadUsuarios(int? UsuarioId)
        {
            return await _GruposSeguridad.ListarGruposSeguridadUsuarios(UsuarioId);
        }
        /// <summary>
        /// Obtener sitios del cliente o de los clientes.
        /// </summary>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpPost("ObtenerDataSiteCliente")]
        public async Task<ResultObject> ObtenerDataSiteCliente([FromBody] List<string> ClienteId)
        {
            return await _GruposSeguridad.ObtenerDataSiteCliente(ClienteId);
        }
        /// <summary>
        /// Se obtienen los grupos de seguridad segun el cliente
        /// </summary>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("GetGrupoSeguridadSitios")]
        public async Task<ResultObject> GetGrupoSeguridadSitios(string ClienteId)
        {
            return await _GruposSeguridad.GetGrupoSeguridadSitios(ClienteId);
        }
        /// <summary>
        /// Trae los sitios seleccionados del cliente
        /// </summary>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("GetSitiosGrupoSeguridad")]
        public async Task<ResultObject> GetSitiosGrupoSeguridad(string ClienteId)
        {
            return await _GruposSeguridad.GetSitiosGrupoSeguridad(ClienteId);
        }
        /// <summary>
        /// Se asignan los sitios o se eliminan
        /// </summary>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        [HttpPost("AsignarSitioAgrupoSeguridad")]
        public async Task<ResultObject> AsignarSitioAgrupoSeguridad([FromBody] SitiosGrupoSeguridadAsignacionVM Modelo)
        {
            return await _GruposSeguridad.AsignarSitioAgrupoSeguridad(Modelo);
        }
        /// <summary>
        /// Se eliminan los grupos de seguridad
        /// </summary>
        /// <param name="GruposeguridadId"></param>
        /// <returns></returns>
        [HttpGet("EliminarGrupoSeguridad")]
        public async Task<ResultObject> EliminarGrupoSeguridad([Required] int GruposeguridadId)
        {
            return await _GruposSeguridad.EliminarGrupoSeguridad(GruposeguridadId);
        }
        /// <summary>
        /// Se consultan los grupos de seguridad.
        /// </summary>
        /// <param name="GruposeguridadId"></param>
        /// /// <param name="UsuarioIds"></param>
        /// <returns></returns>
        [HttpGet("ConsultarGrupoSeguridad")]
        public async Task<ResultObject> ConsultarGrupoSeguridad(int? GruposeguridadId, string UsuarioIds)
        {
            return await _GruposSeguridad.ConsultarGrupoSeguridad(GruposeguridadId, UsuarioIds);
        }
        /// <summary>
        /// Se asigna el usuario a uno o varios grupos de seguridad
        /// </summary>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        [HttpPost("AsignarUsuarioAgrupoSeguridad")]
        public async Task<ResultObject> AsignarUsuarioAgrupoSeguridad([FromBody] UsuarioGrupoSeguridadVM Modelo)
        {
            return await _GruposSeguridad.AsignarUsuarioAgrupoSeguridad(Modelo);
        }
        /// <summary>
        /// Se consulta el grupo de seguridad por cliente
        /// </summary>
        /// <param name="clienteIdS"></param>
        /// <returns></returns>
        [HttpGet("ConsultarGrupoSeguridadClientes")]
        public async Task<ResultObject> ConsultarGrupoSeguridadClientes(int? clienteIdS)
        {
            return await _GruposSeguridad.ConsultarGrupoSeguridadClientes(clienteIdS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clienteIds"></param>
        /// <param name="GrupoSeguridadID"></param>
        /// <returns></returns>
        [HttpGet("ConsultarSitiosGrupoSeguridadCliente")]
        public async Task<ResultObject> ConsultarSitiosGrupoSeguridadCliente(int? clienteIds,int GrupoSeguridadID)
        {
            return await _GruposSeguridad.ConsultarSitiosGrupoSeguridadCliente(clienteIds, GrupoSeguridadID);
        }
        /// <summary>
        /// Guarda o Edita un grupo de seguridad
        /// </summary>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        [HttpPost("GuardarEditarGrupoSeguridad")]
        public async Task<ResultObject> GuardarEditarGrupoSeguridad([FromBody] GrupoSeguridadPostVM Modelo)
        {
            return await _GruposSeguridad.GurdarEditarGrupoSeguridad(Modelo);
        }

        [HttpPost("GetConsultasDinamicas")]
        public async Task<List<dynamic>> GetConsultasDinamicas([FromBody] Dictionary<string, string> parametros, [FromQuery] string Clase, [FromQuery] string NombreConsulta, [FromQuery] PaginacionDTO? paginacionDTO)
        {
           
            return await _getConsultasDinamicas(parametros, Clase, NombreConsulta, paginacionDTO);
        }
        [HttpPost("auth/GetConsultasDinamicas")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<dynamic>> GetConsultasDinamicasConAutorizacion([FromBody] Dictionary<string, string> parametros, [FromQuery] string Clase, [FromQuery] string NombreConsulta, [FromQuery] PaginacionDTO? paginacionDTO)
        {

            return await _getConsultasDinamicas(parametros, Clase, NombreConsulta, paginacionDTO);
        }
        [HttpPost("GetConsultasDinamicasString")]
        public async Task<List<dynamic>> GetConsultasDinamicas([FromBody] string parametros, [FromQuery] string Clase, [FromQuery] string NombreConsulta)
        {
            var pdes = JsonConvert.DeserializeObject<Dictionary<string, string>>(parametros);
          

            var dynamic = new Dapper.DynamicParameters();
            foreach (var kvp in pdes)
            {
                dynamic.Add(kvp.Key, kvp.Value);
            }
            return await _admService.getDynamicValueCore(Clase, NombreConsulta, dynamic);
        }

        [HttpPost("ExecProcedureByTipoConsulta")]
        public async Task<ResultObject> ExecProcedureByTipoConsulta([FromBody] Dictionary<string, string> parametros, [FromQuery] string Clase, [FromQuery] string NombreConsulta)
        {
            return await _execProcedureByTipoConsulta(parametros, Clase, NombreConsulta);
        }

        [HttpPost("auth/ExecProcedureByTipoConsulta")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ResultObject> ExecProcedureByTipoConsultaConAutorizacion([FromBody] Dictionary<string, string> parametros, [FromQuery] string Clase, [FromQuery] string NombreConsulta)
        {
            return await _execProcedureByTipoConsulta(parametros, Clase, NombreConsulta);
        }
        [HttpPost("auth/GetConsultasDinamicasConAutorizacionUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<dynamic>> GetConsultasDinamicasConAutorizacionUser([FromBody] Dictionary<string, string> parametros, [FromQuery] string Clase, [FromQuery] string NombreConsulta, [FromQuery] PaginacionDTO? paginacionDTO)
        {
            parametros.Add("UsuarioId", this.UserId);
            return await _getConsultasDinamicas(parametros, Clase, NombreConsulta, paginacionDTO);
        }
        [HttpPost("auth/GetConsultasDinamicasConAutorizacionUserDWH")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<dynamic>> GetConsultasDinamicasConAutorizacionUserDWH([FromBody] Dictionary<string, string> parametros, [FromQuery] string Clase, [FromQuery] string NombreConsulta, [FromQuery] PaginacionDTO? paginacionDTO)
        {
            parametros.Add("usuarioids", null);
            parametros.Add("userid", this.UserId);
            return await _getConsultasDinamicasDWH(parametros, Clase, NombreConsulta, paginacionDTO);
        }
        private async Task<List<dynamic>> _getConsultasDinamicas(Dictionary<string, string> parametros,  string Clase,  string NombreConsulta,  PaginacionDTO? paginacionDTO) {
            var dynamic = new Dapper.DynamicParameters();
            foreach (var kvp in parametros)
            {
                dynamic.Add(kvp.Key, kvp.Value);
            }
            var resultado = await _admService.getDynamicValueCore(Clase, NombreConsulta, dynamic);
            if (paginacionDTO != null && paginacionDTO.RecordsPorPagina != -1)
            {
                await HttpContext.InsertarParametrosPaginacionEnCabecera(resultado.AsQueryable());
                resultado = resultado.AsQueryable().Paginar(paginacionDTO).ToList();
            }
            return resultado;
        }
        private async Task<List<dynamic>> _getConsultasDinamicasDWH(Dictionary<string, string> parametros, string Clase, string NombreConsulta, PaginacionDTO? paginacionDTO)
        {
            var dynamic = new Dapper.DynamicParameters();
            foreach (var kvp in parametros)
            {
                dynamic.Add(kvp.Key, kvp.Value);
            }
            var resultado = await _admService.getDynamicValueDWH(Clase, NombreConsulta, dynamic);
            if (paginacionDTO != null && paginacionDTO.RecordsPorPagina != -1)
            {
                await HttpContext.InsertarParametrosPaginacionEnCabecera(resultado.AsQueryable());
                resultado = resultado.AsQueryable().Paginar(paginacionDTO).ToList();
            }
            return resultado;
        }

        private async Task<ResultObject> _execProcedureByTipoConsulta(Dictionary<string, string> parametros, string Clase, string NombreConsulta)
        {
            var dynamic = new Dapper.DynamicParameters();
            foreach (var kvp in parametros)
            {
                dynamic.Add(kvp.Key, kvp.Value);
            }
            int r = await _admService.setDynamicValueCore(Clase, NombreConsulta, dynamic);
            return new ResultObject() { Exitoso = (r > 0), Mensaje = (r < 0) ? "No se actaulizaron registros." : "Actualizado exitosamente." };
        }


        }
}
