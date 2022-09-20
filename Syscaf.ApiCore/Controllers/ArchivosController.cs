using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Service.Drive;
using Syscaf.Service.Helpers;

namespace Syscaf.ApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {
        private readonly IArchivosService _Drive;
        public ArchivosController(IArchivosService _Drive)
        {
            this._Drive = _Drive;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NombreArchivo"></param>
        /// <param name="Descripcion"></param>
        /// <param name="DescripcionLog"></param>
        /// <param name="Peso"></param>
        /// <param name="MovimientoId"></param>
        /// <param name="UsuarioId"></param>
        /// <returns></returns>
        [HttpPost("SetArchivo")]
        public async Task<ResultObject> SetArchivo(string NombreArchivo, string Descripcion, string DescripcionLog, int Peso, string Tipo, int? Orden, string Src, int MovimientoId, int? AreaId, string UsuarioId)
        {
            return await _Drive.SetInsertarArchivo(NombreArchivo, Descripcion, DescripcionLog, Peso, Tipo, Orden, Src, MovimientoId, AreaId, UsuarioId);
        }
        /// <summary>
        /// Archivos listado
        /// </summary>
        /// <param name="UsuarioNombre"></param>
        /// <returns></returns>
        [HttpPost("GetArchivosDatabase")]
        public async Task<ResultObject> GetArchivosDatabase(string? UsuarioNombre)
        {
            return await _Drive.GetArchivosDatabase(UsuarioNombre);
        }
        /// <summary>
        /// Se realiza el guardado de logs general para neptuno
        /// </summary>
        /// <param name="Descripcion"></param>
        /// <param name="MovimientoId"></param>
        /// <param name="ArchivoId"></param>
        /// <param name="UsuarioId"></param>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        [HttpPost("SetLog")]
        public async Task<ResultObject> SetLog(string Descripcion, int MovimientoId, int ArchivoId, string UsuarioId, int AreaId)
        {
            return await _Drive.SetLog(Descripcion, MovimientoId, ArchivoId, UsuarioId, AreaId);
        }
    }
}
