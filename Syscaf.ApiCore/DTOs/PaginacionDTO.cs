using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syscaf.ApiCore.DTOs
{
    public class PaginacionDTO
    {
        private int _pagina { get; set; } = 1;

        public int Pagina
        {
            get
            {
                return _pagina;
            }
            set
            {
                _pagina = (value <= 0) ? MinimoPagina : value;
            }
        }
        private int recordsPorPagina = 10;
        private readonly int cantidadMaximaRecordsPorPagina = 50;
        private readonly int MinimoPagina = 1;

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value;
            }
        }
    }
}
