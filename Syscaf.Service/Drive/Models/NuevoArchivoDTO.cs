using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Drive.Models
{
    public class NuevoArchivoDTO
    {
        public int? ArchivoId { get; set; }
        public string NombreArchivo { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionLog { get; set; }
        public string Tipo { get; set; }
        public string Src { get; set; }
        public int MovimientoId { get; set; }
        public int Peso { get; set; }
        public int Orden { get; set; }

        public int? AreaId { get; set; }
        public string UsuarioId { get; set; }
        public string DatosAdicionales { get; set; }
        public DateTime? FechaSistema { get; set; }

    }
    public class NuevoArchivoPeticionDTO : NuevoArchivoDTO    {
      
        public IFormFile? archivo { get; set; }


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
