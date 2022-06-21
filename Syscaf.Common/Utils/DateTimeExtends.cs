using Syscaf.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Utilities
{
    public static partial class DateTimeExtensions
    {
       

        public static string FormatoFechaHora(this DateTime dt)
        {
            return dt.ToString(Constants.FormatoFechaHora);
        }
        public static string FormatoFechaHoraSeg(this DateTime dt)
        {
            return dt.ToString(Constants.FormatoFechaHoraSeg);
        }

        public static string FormatoPegaso(this DateTime dt)
        {
            return dt.ToString(Constants.FormatoPegaso);
        }
        public static string FormatoSinceToken(this DateTime dt)
        {
            return dt.ToString(Constants.FormatoSinceToken);
        }

        public static string FormatoHoraPacifico(this DateTime dt)
        {
            return dt.ToString(Constants.FormatoHoraPacifico);
        }
        public static string FormatoHoraPacificoHMS(this DateTime dt)
        {
            return dt.ToString(Constants.FormatoHoraPacificoHMS);
        }

        public static DateTime ToColombiaTime(this DateTime Fecha)
        {
            return TimeZoneInfo.ConvertTime(Fecha, Constants.timezone);
        }
        public static string FormatoyyyyMMddhhmmss(this DateTime dt)
        {
            return dt.ToString(Constants.FormatoyyyyMMddhhmmss);
        }
    }
}
