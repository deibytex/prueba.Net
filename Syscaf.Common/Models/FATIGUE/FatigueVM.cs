using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.FATIGUE
{
	public class FatigueVM
	{
		public string Nombre { get; set; }
		public int Tiempo { get; set; }
		public string Condicion { get; set; }
		public string Columna { get; set; }
		public string ClienteId { get; set; }
		public bool EsActivo { get; set; }
		public DateTime FechaSistema { get; set; }
		public string clienteNombre { get; set; }
		public int ClienteIdS { get; set; }
		public int ConfiguracionAlertaId { get; set; }
        public int MinAmber { get; set; }
        public int MaxAmber { get; set; }

    }
	public class SetFatigueVM
	{
		public int? Clave { get; set; }
		public string? Nombre { get; set; }
		public int? Tiempo { get; set; }
		public string? Condicion { get; set; }
		public string? Columna { get; set; }
		public string? ClienteId { get; set; }
		public bool? EsActivo { get; set; }
		public int? ConfiguracionAlertaId { get; set; }

	}

	public class GetFatigueVM
	{
		public string? Nombre { get; set; }
		public string? ClienteId { get; set; }
		public bool? EsActivo { get; set; }


	}
}