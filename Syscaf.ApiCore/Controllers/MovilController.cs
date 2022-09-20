using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Service.Helpers;
using System.ComponentModel.DataAnnotations;
using Syscaf.Service.Portal;
using Syscaf.Common.Models.MOVIL;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;
using Syscaf.Service.Drive;

namespace Syscaf.ApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovilController : ControllerBase
    {

        private readonly IMovilService _Movil;
      
        public MovilController(IMovilService _Movil)
        {
            this._Movil = _Movil;
           
        }
        /// <summary>
        /// Setea las respuestas del usuario desde el movil
        /// </summary>
        /// <param name="RespuestasPreoperacional"></param>
        /// <returns></returns>
        [HttpPost("SetRespuestasPreoperacional")]
        public async Task<ResultObject> SetRespuestasPreoperacional([FromBody] String RespuestasPreoperacional)
        {
            return await _Movil.SetRespuestasPreoperacional(RespuestasPreoperacional);
        }
        /// <summary>
        /// Se consultan las respuestas ya sea por cliente, por pregunta, o por usuarios pero opcional
        /// </summary>
        /// <param name="Fecha"></param>
        /// <param name="UsuarioId"></param>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("GetRespuestasPreoperacional")]
        public async Task<ResultObject> GetRespuestasPreoperacional(string? Fecha, string? UsuarioId, Int64? ClienteId)
        {
            return await _Movil.GetRespuestasPreoperacional(Fecha, UsuarioId, ClienteId);
        }
        /// <summary>
        /// Se consultan las preguntas de preoperacional.
        /// </summary>
        /// <param name="UsuarioId"></param>
        /// <param name="NombrePlantilla"></param>
        /// <param name="TipoPregunta"></param>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("GetPreguntasPreoperacional")]
        public async Task<ResultObject> GetPreguntasPreoperacional(string? UsuarioId, string? NombrePlantilla, string? TipoPregunta, long? ClienteId)
        {
            return await _Movil.GetPreguntasPreoperacional(UsuarioId, NombrePlantilla, TipoPregunta, ClienteId);
        }

        [HttpPost("BlobService")]
        public async Task<string> BlobService([FromForm] FileDataDTO datos)
        {

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=neptunodataaccount;AccountKey=ONCwJMNPn4N9a960rBu8ontFlcQTbiGtK2inKFQo80BUAgO7n75n97B07rNhCeU6Wc8Coi5kozj4+AStIFkGkw==;EndpointSuffix=core.windows.net";

            var serviceClient = new BlobServiceClient(connectionString);
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
        public async Task<MemoryStream> DownloadFileFromBlob(string nombrearchivo)
        {

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=neptunodataaccount;AccountKey=ONCwJMNPn4N9a960rBu8ontFlcQTbiGtK2inKFQo80BUAgO7n75n97B07rNhCeU6Wc8Coi5kozj4+AStIFkGkw==;EndpointSuffix=core.windows.net";

            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient("serviciotecnico");
            var blobClient = containerClient.GetBlobClient(nombrearchivo);
            MemoryStream stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;
            return stream;
        }

        [HttpGet("getDirectorio")]
        public async Task<object> getDirectorio(string container)
        {

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=neptunodataaccount;AccountKey=ONCwJMNPn4N9a960rBu8ontFlcQTbiGtK2inKFQo80BUAgO7n75n97B07rNhCeU6Wc8Coi5kozj4+AStIFkGkw==;EndpointSuffix=core.windows.net";

            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient("serviciotecnico");
            
           

            return containerClient.GetBlobs().ToList().Select(s => s.Name);
        }



    }

    public class FileDataDTO
    {
        public FileDataDTO()
        {
            contenedor = "serviciotecnico";
        }
        public string? nombre { get; set; }
        public string? contenedor { get; set; }
        public string? src { get; set; }
        public IFormFile? archivo { get; set; }

       
    }
}
