using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.ARCHIVOS
{
    public class ArchivosVM
    {
        public ArchivosVM()
        {
            Hijos = new List<ArchivosVM>();
        }
        public int ArchivoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public int? Orden { get; set; }
        public string Src { get; set; }
        public int? ArchivoPadreId { get; set; }
        public int? Peso { get; set; }
        public DateTime FechaSistema { get; set; }
   
        public ICollection<ArchivosVM> Hijos { get; set; }
    }
    public class ArchivosSeparados
    {
        public int ArchivoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public int? Orden { get; set; }
        public string Src { get; set; }
        public int? ArchivoPadreId { get; set; }
        public int? Peso { get; set; }
        public string[] ArraySrc { get { return Src.Split("/"); } }
        public DateTime FechaSistema { get; set; }
    }
}
