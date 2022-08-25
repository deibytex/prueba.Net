using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.MOVIL
{
    public class RespuestasVM
    {
        public string? Respuesta { get; set; }
        public string? ValorRedLine { get; set; }
        public int? PreguntaId { get; set; }
        public string? UsuarioId { get; set; }
        public long? ClienteId { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Pregunta { get; set; }
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
    public class getPreguntasVM
    {
        public int PreguntaId { get; set; }
        public int PlantillaId { get; set; }
        public string NombrePlantilla { get; set; }
        public int TipoPlantilla { get; set; }
        public int TipoPreguntaId { get; set; }
        public string TipoPregunta { get; set; }
        public string Valores { get; set; }
        public int Secuencia { get; set; }
        public string Descripcion { get; set; }
        public string UsuarioId { get; set; }
        public DateTime FechaSistema { get; set; }
        public bool EsActivo { get; set; }
        public bool EsRedLine { get; set; }
        public string ValorRedLine { get; set; }
        public string Agrupador { get; set; }
    }
    public class EncabezadoPreoperacionalVM
    {
        public int? PreoperacionalId { get; set; }
        public DateTime? FechaHora { get; set; }
        public DateTime? Fecha { get; set; }
        public string? UserId { get; set; }         
        public long? AssetId { get; set; }      
        public string ClienteId { get; set; }
        public int? EstadoPreoperacional { get; set; } // 1 - Valido - 2. Invalido
        public string? Vehiculo { get; set; }
        public string? email { get; set; }
    }
    public  class Preoperacional
    {
        public EncabezadoPreoperacionalVM Encabezado { get;set; }
        public List<RespuestasVM> Respuestas { get; set; }
    }
}
