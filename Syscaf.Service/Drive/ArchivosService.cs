using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using Syscaf.Service.Helpers;
using Syscaf.Data.Helpers.Archivos;
using Syscaf.Data;
using System.Data;
using System.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Syscaf.Common.Models.ARCHIVOS;

namespace Syscaf.Service.Drive
{
    public class ArchivosService : IArchivosService
    {
        private readonly SyscafCoreConn _conn;
        public ArchivosService(SyscafCoreConn conn)
        {
            _conn = conn;
        }
        /* Global instance of the scopes required by this quickstart.
        If modifying these scopes, delete your previously saved token.json/ folder. */
        static string[] Scopes = { global::Google.Apis.Drive.v3.DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Syscaf";
        public Task<string> AccesoDrive()
        {
            var r = "";
            try
            {
                UserCredential credential;
                // Load client secrets.
                using (var stream =
                       new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    /* The file token.json stores the user's access and refresh tokens, and is created
                     automatically when the authorization flow completes for the first time. */
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Drive API service.
                var service = new global::Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Define parameters of request.
                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = 10;
                listRequest.Fields = "nextPageToken, files(id, name)";

                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
                Console.WriteLine("Files:");
                if (files == null || files.Count == 0)
                {
                    Console.WriteLine("No files found.");
                    return Task.FromResult("");
                }
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return Task.FromResult(r);
        }
        public async Task<ResultObject> SetInsertarArchivo(string NombreArchivo,string Descripcion,string DescripcionLog,int Peso, string Tipo,int? Orden,string Src, int MovimientoId, int? AreaId, string UsuarioId)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("NombreArchivo", NombreArchivo);
                parametros.Add("Descripcion", Descripcion);
                parametros.Add("DescripcionLog", DescripcionLog);
                parametros.Add("Peso", Peso);
                parametros.Add("Tipo", Tipo);
                parametros.Add("Orden", Orden);
                parametros.Add("Src", Src);
                parametros.Add("MovimientoId", MovimientoId);
                parametros.Add("AreaId", AreaId);
                parametros.Add("UsuarioId", UsuarioId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.Insert<String>(ArchivosQueryHelper._Insert, parametros, commandType: CommandType.StoredProcedure));
                    r.Mensaje = result;
                    r.Exitoso = (result == "Operación Éxitosa") ? true : false;
                    r.success();
                }
                catch (Exception ex)
                {
                    r.error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<List<ArchivosSeparados>> GetMenuArchivo(string Nombre)
        {

            return await _conn.GetAllAsync<ArchivosSeparados>("NEP.GetMenuArchivos", new { Nombre });
        }
        public async Task<ResultObject> GetArchivosDatabase(string UsuarioId)
        {
            var r = new ResultObject();
            try
            {
                //Consulto la informacion de la base de datos.
               var MenuArchivosSeparados = await GetMenuArchivo(UsuarioId);
                r.Data = ListadoCarpeta(MenuArchivosSeparados, null,0);
                r.Exitoso = true;
                r.Mensaje = "Operación Éxitosa";
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        private static List<ArchivosVM> ListadoCarpeta(List<ArchivosSeparados> Datos, List<ArchivosVM> Carpetas, int index)
        {
            var groupData = Datos.Where(x => x.ArraySrc.Length > index).GroupBy(x => x.ArraySrc[index]).ToList();
            if (Carpetas == null)
                Carpetas = new List<ArchivosVM>();

            foreach (var group in groupData)
            {
                ArchivosVM archivo = new ArchivosVM();
                archivo.Nombre = group.Key;
                archivo.Src = group.Where(w => w.Nombre == group.Key).Select(s => s.Src).FirstOrDefault();
                archivo.Orden = group.Where(w => w.Nombre == group.Key).Select(s => s.Orden).FirstOrDefault();
                archivo.FechaSistema = group.Where(w => w.Nombre == group.Key).Select(s => s.FechaSistema).FirstOrDefault();
                archivo.Descripcion = group.Where(w => w.Nombre == group.Key).Select(s => s.Descripcion).FirstOrDefault();
                archivo.Tipo = group.Where(w => w.Nombre == group.Key).Select(s => s.Tipo).FirstOrDefault();
                archivo.Peso = group.Where(w => w.Nombre == group.Key).Select(s => s.Peso).FirstOrDefault();
                archivo.ArchivoId = group.Where(w => w.Nombre == group.Key).Select(s => s.ArchivoId).FirstOrDefault();
                archivo.Hijos = ListadoCarpeta(group.ToList(), null, index + 1);
                Carpetas.Add(archivo);
                
                //CreateTreeRecursive(group.ToList(), newNode, index + 1);
            }
            return Carpetas;
        }        
    }

    public interface IArchivosService
    {
        Task<string> AccesoDrive();
        Task<ResultObject> SetInsertarArchivo(string NombreArchivo, string Descripcion, string DescripcionLog, int Peso, string Tipo, int? Orden, string Src, int MovimientoId, int? AreaId, string UsuarioId);
        Task<ResultObject> GetArchivosDatabase(string UsuarioId);
    }
}
