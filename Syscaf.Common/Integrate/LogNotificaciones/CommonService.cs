using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
using Syscaf.Data.Models;
using Syscaf.Service.DataTableSql;
using Syscaf.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Helper = Syscaf.Common.Helpers.Helpers;
namespace Syscaf.Common.Integrate.LogNotificaciones
{
    public  class CommonService: ICommonService
    {
        readonly ILogService _LogService;
        public CommonService(ILogService LogService) => this._LogService = LogService;
        //public  Listas GetListaBySigla(string sigla)
        //{
        //    Listas lista = null;
        //    try
        //    {
        //        using (SyscafBD ctx = new SyscafBD())
        //        {
        //            lista = ctx.Listas.Include("Detalle").Where(w => w.Sigla.Equals(sigla, Constants.comparer)).FirstOrDefault();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _LogService.SetLog(ex.ToString(), "", "CommonService - GetListaBySigla ");
        //    }

        //    return lista;

        //}
        //public  DetalleListas GetDetalleListaBySigla(string sigla)
        //{
        //    DetalleListas detalle = null;
        //    try
        //    {
        //        using (SyscafBD ctx = new SyscafBD())
        //        {
        //            detalle = ctx.DetalleListas.Where(w => w.Sigla.Equals(sigla, Constants.comparer)).FirstOrDefault();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _LogService.SetLog(ex.ToString(), "", "CommonService - GetDetalleListaBySigla ");
        //    }

        //    return detalle ?? new DetalleListas();


        //}

        //public  DetalleListas GetDetalleListaByListaId(int ListaId)
        //{
        //    DetalleListas detalle = null;
        //    try
        //    {
        //        using (SyscafBD ctx = new SyscafBD())
        //        {
        //            detalle = ctx.DetalleListas.Where(w => w.ListaId == ListaId).FirstOrDefault();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _LogService.SetLog(ex.ToString(), "", "CommonService - GetDetalleListaByListaId ");
        //    }

        //    return detalle ?? new DetalleListas();

        //}


        //public  Listas GetListaById(string siglaLista)
        //{
        //    Listas lista = null;
        //    try
        //    {
        //        using (SyscafBD ctx = new SyscafBD())
        //        {
        //            lista = ctx.Listas.Where(w => w.Sigla.Equals(siglaLista, Constants.comparer)).FirstOrDefault();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _LogService.SetLog(ex.ToString(), "", "CommonService - GetListaById ");
        //    }

        //    return lista;

        //}
        //public  Listas GetDetallesListaById(int Id)
        //{
        //    Listas lista = null;
        //    try
        //    {
        //        using (SyscafBD ctx = new SyscafBD())
        //        {
        //            lista = ctx.Listas.Include("Detalle").Where(w => w.ListaId == Id).FirstOrDefault();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _LogService.SetLog(ex.ToString(), "", "CommonService - GetListaByID ");
        //    }

        //    return lista;
        //}



        //public  List<DetalleListas> GetDetalleListasByListaId(int ListaId)
        //{
        //    List<DetalleListas> ctxs = new List<DetalleListas>();
        //    using (SyscafBD ctx = new SyscafBD())
        //    {
        //        ctxs = ctx.DetalleListas.Where(w => w.ListaId == ListaId).ToList();
        //    }

        //    return ctxs;

        //}
        //public  Listas GetListaBySiglaReporte(string sigla)
        //{
        //    Listas lista = null;
        //    try
        //    {
        //        using (SyscafBD ctx = new SyscafBD())
        //        {
        //            lista = ctx.Listas.Where(w => w.Sigla.Equals(sigla, Constants.comparer)).FirstOrDefault();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        _LogService.SetLog(ex.ToString(), "", "CommonService - GetListaBySigla ");
        //    }

        //    return lista;

        //}

        //// trae de la bd los identificadoes no ingresados en el sietma
        //// de viajes, eventos y metricas
        //public  List<long> GetIdsNoIngresados(List<long> Ids, int ClienteIds, int tipo, DateTime dias)
        //{


        //    try
        //    {
        //        using (SyscafBD ctx = new SyscafBD())
        //        {
        //            var dtIds = HelperDatatable.GetDataTableIdentity();
        //            // los guarda en el DataTable
        //            Ids.ForEach(e =>
        //             dtIds.Rows.Add(e));

        //            //  traemos la información de los identificadores que no existen en la base de datos

        //            var parmCliente = new SqlParameter("ClienteIds", SqlDbType.Int)
        //            {
        //                Value = ClienteIds
        //            };

        //            var parmTipo = new SqlParameter("Tipo", SqlDbType.Int)
        //            {
        //                Value = tipo
        //            };
        //            var parmDias = new SqlParameter("dias", SqlDbType.DateTime)
        //            {
        //                Value = dias
        //            };

        //            var parmEventosIds = new SqlParameter("Ids", SqlDbType.Structured)
        //            {
        //                Value = dtIds,
        //                TypeName = "dbo.UDT_TableIdentity"
        //            };

        //            // trae la información de eventos 
        //            return ctx.Database.SqlQuery<long>("SP_Event_VerificaExistentes @ClienteIds, @Tipo, @dias, @Ids ", parmCliente, parmTipo, parmDias, parmEventosIds).ToList();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

    }
}