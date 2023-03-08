using AutoMapper;
using Dapper;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;
using Syscaf.Service.DataTableSql;
using Syscaf.Service.Helpers;
using SyscafWebApi.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Syscaf.Report.ViewModels.Sotramac;
using Syscaf.Common.Models.MOVIL;
using Syscaf.Data.Helpers.Sotramac;

namespace Syscaf.Service.Sotramac
{

    public class SotramacService : ISotramacService
    {
        private readonly SyscafCoreConn _conn;
        private readonly ISyscafConn _conprod;
        public SotramacService(SyscafCoreConn conn, ISyscafConn _conprod)
        {
            _conn = conn;
            this._conprod = _conprod;
        }

        // adiciona los mensajes a la tabla con el periodo seleccionado

        public async Task<List<ReporteSotramacVM>> GetReporteSotramacConductores(DateTime FechaInicial, DateTime FechaFinal, int clienteIdS, string DriversIdS, int AssetTypeId, Int64? SiteId)
        {
            var parametros = new Dapper.DynamicParameters();
            parametros.Add("FechaInicial", FechaInicial, DbType.DateTime);
            parametros.Add("FechaFinal", FechaInicial, DbType.DateTime);
            parametros.Add("clienteIdS", clienteIdS, DbType.Int32);
            parametros.Add("DriversIdS", DriversIdS, DbType.String);
            parametros.Add("AssetTypeId", AssetTypeId, DbType.Int32);
            parametros.Add("SiteId", SiteId, DbType.Int64);

            return await Task.FromResult(_conn.GetAll<ReporteSotramacVM>(SotramacQueryHelper._getReporteConductores, parametros, commandType: CommandType.StoredProcedure));

        }

        public async Task<List<ReporteSotramacVM>> GetReporteSotramacVehiculos(DateTime FechaInicial, DateTime FechaFinal, int clienteIdS, string Assetsids, int AssetTypeId, Int64? SiteId)
        {
            var parametros = new Dapper.DynamicParameters();
            parametros.Add("FechaInicial", FechaInicial, DbType.DateTime);
            parametros.Add("FechaFinal", FechaInicial, DbType.DateTime);
            parametros.Add("clienteIdS", clienteIdS, DbType.Int32);
            parametros.Add("AssetTypeId", AssetTypeId, DbType.Int32);
            parametros.Add("SiteId", SiteId, DbType.Int64);

            return await Task.FromResult(_conn.GetAll<ReporteSotramacVM>(SotramacQueryHelper._getReporteVehiculos, parametros, commandType: CommandType.StoredProcedure));

        }

        public async Task<List<ReporteSotramacVM>> GetReporteSotramacCOxVH(DateTime FechaInicial, DateTime FechaFinal, int clienteIdS, string DriversIdS, string Assetsids, int AssetTypeId, Int64? SiteId)
        {
            var parametros = new Dapper.DynamicParameters();
            parametros.Add("FechaInicial", FechaInicial, DbType.DateTime);
            parametros.Add("FechaFinal", FechaInicial, DbType.DateTime);
            parametros.Add("clienteIdS", clienteIdS, DbType.Int32);
            parametros.Add("DriversIdS", DriversIdS, DbType.String);
            parametros.Add("Assetsids", Assetsids, DbType.String);
            parametros.Add("AssetTypeId", AssetTypeId, DbType.Int32);
            parametros.Add("SiteId", SiteId, DbType.Int64);

            return await Task.FromResult(_conn.GetAll<ReporteSotramacVM>(SotramacQueryHelper._getReporteCOxVH, parametros, commandType: CommandType.StoredProcedure));

        }
    }

    public interface ISotramacService
    {


        Task<List<ReporteSotramacVM>> GetReporteSotramacConductores(DateTime FechaInicial, DateTime FechaFinal, int clienteIdS, string DriversIdS, int AssetTypeId, Int64? SiteId);
        Task<List<ReporteSotramacVM>> GetReporteSotramacVehiculos(DateTime FechaInicial, DateTime FechaFinal, int clienteIdS, string Assetsids, int AssetTypeId, Int64? SiteId);
        Task<List<ReporteSotramacVM>> GetReporteSotramacCOxVH(DateTime FechaInicial, DateTime FechaFinal, int clienteIdS, string DriversIdS, string Assetsids, int AssetTypeId, Int64? SiteId);

    }
}
