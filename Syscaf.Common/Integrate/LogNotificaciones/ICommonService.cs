using Syscaf.Common.Helpers;
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
namespace Syscaf.Service
{
    public interface ICommonService
    {
        Listas GetListaBySigla(string sigla);
        DetalleListas GetDetalleListaBySigla(string sigla);
        DetalleListas GetDetalleListaByListaId(int ListaId);
        Listas GetListaById(string siglaLista);
        Listas GetDetallesListaById(int Id);
        List<DetalleListas> GetDetalleListasByListaId(int ListaId);
        Listas GetListaBySiglaReporte(string sigla);

        // trae de la bd los identificadoes no ingresados en el sietma
        // de viajes, eventos y metricas
        List<long> GetIdsNoIngresados(List<long> Ids, int ClienteIds, int tipo, DateTime dias);
    }
}