using System;
using System.Collections.Generic;
using System.Text;

namespace Syscaf.Data.Helpers
{
   public static class slnHelper
    {
        public static DateTime? timeNull = null;        
        public static string FormatoFechaHora = "yyyy/MM/dd HH:mm";

        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars);
        }
        public static DateTime GetFechaServidor(TimeZoneInfo timezone)
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, timezone);
        }

        public static DateTime GetFechaServidor(DateTime Fecha, TimeZoneInfo timezone)
        {
            return TimeZoneInfo.ConvertTime(Fecha, timezone);
        }

        public static string GetFechaServidor(DateTime? Fecha, TimeZoneInfo timezone)
        {
            return Fecha.HasValue ? TimeZoneInfo.ConvertTime(Fecha.Value, timezone).ToString("dd/MM/yyyy HH:mm") : "";
        }
        public static DateTime? GetFechaServidor(DateTime? Fecha, TimeZoneInfo timezone, bool isString = true)
        {
            return (Fecha.HasValue) ? TimeZoneInfo.ConvertTime(Fecha.Value, timezone) : timeNull;
        }
          

     

    }
}
