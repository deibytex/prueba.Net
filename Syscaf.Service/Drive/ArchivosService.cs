﻿using System;
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
using Syscaf.Service.Drive.Models;

namespace Syscaf.Service.Drive
{
    public class ArchivosService : IArchivosService
    {
        private readonly SyscafCoreConn _conn;
        public ArchivosService(SyscafCoreConn conn)
        {
            _conn = conn;
        }
        public async Task<ResultObject> SetInsertarArchivo(NuevoArchivoDTO datosArchivos)
        {
            var r = new ResultObject();

            var propiedades = datosArchivos.GetType().GetProperties().Where(w => w.Name != "archivo" );

            Type t = typeof(NuevoArchivoDTO);
           
           IDictionary<string, object > d = new Dictionary<string, object>();

            foreach (var p in propiedades) {

                d.Add(p.Name, p.GetValue(datosArchivos));
            }

            try
            {
              
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await _conn.Insert<string>(ArchivosQueryHelper._Insert, d, commandType: CommandType.StoredProcedure);
                    r.Mensaje = result;
                    if (result == "Operación Éxitosa") r.success(); else r.error("Error al guardar informacion");
                    
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
        public async Task<List<ArchivosSeparados>> GetMenuArchivo(string Nombre, string contenedor)
        {

            return await _conn.GetAllAsync<ArchivosSeparados>("NEP.GetMenuArchivos", new { Nombre, contenedor });
        }

        public async Task<ResultObject> GetArchivosDatabase(string Nombre, string contenedor)
        {
            var r = new ResultObject();
            try
            {
                //Consulto la informacion de la base de datos.
               var MenuArchivosSeparados = await GetMenuArchivo(Nombre, contenedor);
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
        public  List<ArchivosVM> ListadoCarpeta(List<ArchivosSeparados> Datos, List<ArchivosVM> Carpetas, int index)
        {
            var groupData = Datos.Where(x => x.ArraySrc.Length > index).GroupBy(x => x.ArraySrc[index]).ToList();
            if (Carpetas == null)
                Carpetas = new List<ArchivosVM>();
            Random random = new Random();
            foreach (var group in groupData)
            {
                ArchivosVM archivo = new ArchivosVM();
                archivo.Nombre = group.Key;
                archivo.Src = String.Join('/', group.First().ArraySrc.Take(index + 1));
                archivo.Orden = group.FirstOrDefault().Orden;
                archivo.FechaSistema = group.Select(s => s.FechaSistema).FirstOrDefault();
                archivo.Descripcion = group.Select(s => s.Descripcion).FirstOrDefault();
                archivo.Tipo = (group.Key.Contains(".") ? "archivo" : "carpeta");
                archivo.Peso = group.Select(s => s.Peso).FirstOrDefault();
                archivo.ArchivoId = group.Key.Contains(".") ? group.Select(s=> s.ArchivoId).FirstOrDefault(): random.Next(45545) * 58621;
                archivo.Hijos = ListadoCarpeta(group.ToList(), null, index + 1);
                archivo.ContentType = group.Select(s => s.Tipo).FirstOrDefault();
                Carpetas.Add(archivo);
                
                //CreateTreeRecursive(group.ToList(), newNode, index + 1);
            }
            return Carpetas;
        }
        public async Task<ResultObject> SetLog(string Descripcion, int MovimientoId, int ArchivoId, string UsuarioId, int AreaId)
        {
            var r = new ResultObject();
            var parametros = new Dapper.DynamicParameters();
            parametros.Add("Descripcion", Descripcion);
            parametros.Add("MovimientoId", MovimientoId);
            parametros.Add("ArchivoId", ArchivoId);
            parametros.Add("UsuarioId", UsuarioId);
            parametros.Add("AreaId", AreaId);
            try
            {
                //Se ejecuta el procedimiento almacenado.
                var result = await Task.FromResult(_conn.Insert<String>(ArchivosQueryHelper._InsertLog, parametros, commandType: CommandType.StoredProcedure));
                r.Mensaje = result;
                r.Exitoso = (result == "Operación Éxitosa") ? true : false;
                r.success();
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
            }
            return r;
        }
    }

    public interface IArchivosService
    {
        Task<ResultObject> SetInsertarArchivo(NuevoArchivoDTO datosArchivos);
        Task<ResultObject> GetArchivosDatabase(string Nombre, string contenedor);
        Task<ResultObject> SetLog(string Descripcion, int MovimientoId, int ArchivoId, string UsuarioId, int AreaId);
        List<ArchivosVM> ListadoCarpeta(List<ArchivosSeparados> Datos, List<ArchivosVM> Carpetas, int index);
    }
}
