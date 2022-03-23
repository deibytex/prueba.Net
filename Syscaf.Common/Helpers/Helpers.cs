using System;
using System.Linq;
using System.Text;
//using System.Web.Routing;
//using TimeZoneConverter;

namespace Syscaf.Common.Helpers
{
    public static class Helpers
    {
        public static DateTime? timeNull = null;

        //TimeZoneInfo multiplataforma
        public static TimeZoneInfo timezone = null;//  TZConvert.GetTimeZoneInfo(Constants.ZonaHoraria);

        public static string FormatoFechaHora = "yyyy/MM/dd HH:mm";


        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars);
        }
        public static DateTime GetFechaServidor()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, timezone);
        }

        public static DateTime GetFechaServidor(DateTime Fecha)
        {
            return TimeZoneInfo.ConvertTime(Fecha, timezone);
        }

        public static string GetFechaServidor(DateTime? Fecha)
        {
            return Fecha.HasValue ? TimeZoneInfo.ConvertTime(Fecha.Value, timezone).ToString("dd/MM/yyyy HH:mm") : "";
        }
        public static DateTime? GetFechaServidor(DateTime? Fecha, bool isString = true)
        {
            return (Fecha.HasValue) ? TimeZoneInfo.ConvertTime(Fecha.Value, timezone) : timeNull;
        }


        //public static string GetControllerString(RouteData route)
        //{
        //    return route.GetRequiredString("controller");
        //}

        //public static string GetAccionString(RouteData route)
        //{
        //    return route.GetRequiredString("action");
        //}

        //public static string GetValueRouteString(RouteData route, string nameRoute)
        //{
        //    return route.GetRequiredString(nameRoute);
        //}

        public static string RandomString(int length) {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }


}
