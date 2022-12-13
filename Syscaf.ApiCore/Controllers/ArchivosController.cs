using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Common.Helpers;
using Syscaf.Common.Models.ARCHIVOS;
using Syscaf.Service.Drive;
using Syscaf.Service.Drive.Models;
using Syscaf.Service.Helpers;

namespace Syscaf.ApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {
        private readonly IArchivosService _Drive;
        private readonly IConfiguration _configuration;

        public string BlobConnexion { get; set; }
        public ArchivosController(IArchivosService _Drive, IConfiguration _configuration)
        {
            this._Drive = _Drive;
            this._configuration = _configuration;

            BlobConnexion = _configuration.GetSection("Neptuno")["conexion"];
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
        public async Task<ResultObject> SetArchivo([FromForm] NuevoArchivoPeticionDTO datosArchivos, [FromQuery] string contenedor)
        {

            if (contenedor != "-1")
            {
                var serviceClient = new BlobServiceClient(BlobConnexion);
                var containerClient = serviceClient.GetBlobContainerClient(contenedor);
                var fileName = $"{Constants.GetFechaServidor().ToString("yyyyMM")}/{Guid.NewGuid()}{datosArchivos.NombreArchivo}";


                var blobClient = containerClient.GetBlobClient(fileName);

                // comvertimos los datos en imemory stream
                if (datosArchivos.archivo.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        datosArchivos.archivo.CopyTo(ms);
                        //   var fileBytes = ms.ToArray();
                        // string s = Convert.ToBase64String(fileBytes);
                        //  await blobClient.DeleteAsync();
                        ms.Position = 0;
                        if (!blobClient.Exists())
                            await blobClient.UploadAsync(ms);
                    }


                }
                //   return blobClient.Uri.AbsolutePath;

                datosArchivos.Src = fileName;
            }
            datosArchivos.FechaSistema = Constants.GetFechaServidor();
            return await _Drive.SetInsertarArchivo(datosArchivos);
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

        [HttpPost("BlobService")]
        public async Task<string> BlobService([FromForm] FileDataDTO datos)
        {

           var serviceClient = new BlobServiceClient(BlobConnexion);
            var containerClient = serviceClient.GetBlobContainerClient(datos.contenedor);

            var fileName = $"{datos.src}/{datos.archivo.FileName}";

            var blobClient = containerClient.GetBlobClient(fileName);

            // comvertimos los datos en imemory stream
            if (datos.archivo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    datos.archivo.CopyTo(ms);
                    //   var fileBytes = ms.ToArray();
                    // string s = Convert.ToBase64String(fileBytes);
                    //  await blobClient.DeleteAsync();
                    ms.Position = 0;
                    if (!blobClient.Exists())
                        await blobClient.UploadAsync(ms);
                }


            }
            return blobClient.Uri.AbsolutePath;
        }
        [HttpGet("DownloadFileFromBlob")]
        public async Task<MemoryStream> DownloadFileFromBlob(string nombrearchivo, string container)
        {

       
            var serviceClient = new BlobServiceClient(BlobConnexion);
            var containerClient = serviceClient.GetBlobContainerClient(container);
            var blobClient = containerClient.GetBlobClient(nombrearchivo);
            MemoryStream stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;
            return stream;
        }

        [HttpGet("getDirectorio")]
        public  List<dynamic> getDirectorio(string container, string? filter)
        {

       
            var serviceClient = new BlobServiceClient(BlobConnexion);
            var containerClient = serviceClient.GetBlobContainerClient(container);
            var blobs =  containerClient.GetBlobs().Where(w => (filter == null ||  w.Name.Contains(filter ?? "",StringComparison.CurrentCultureIgnoreCase)) ).ToList();
            int i = 0;
           
            return _Drive.ListadoCarpeta(blobs.Select(s => new ArchivosSeparados() 
            { 
                ArchivoId = 0,
                Orden = blobs.IndexOf(s), 
                Src = s.Name,
                Nombre = s.Name.Split("/").Last(),
                Peso = (int?)s.Properties.ContentLength 
            }).ToList(), null, 0).ToList<dynamic>();
         
        }
    }

    }
