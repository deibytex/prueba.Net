using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.MOVIL
{
    public class RespuestasVM
    {
        public string Descripcion { get; set; }
        public int PreguntaId { get; set; }
        public string UsuarioId { get; set; }
    }

    public class getRespuestasVM
    {
        public string Pregunta { get; set; }
        public string Respuesta { get; set; }
        public int RespuestaId { get; set; }
        public string UsuarioID { get; set; }
        public string ClienteId { get; set; }
        public string Usuario { get; set; }
        public string clienteNombre { get; set; }
        public DateTime FechaSistema { get; set; }
        public DateTime Fecha { get; set; }
        public int Secuencia { get; set; }
    }
}
