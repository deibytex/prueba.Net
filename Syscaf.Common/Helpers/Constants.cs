
using Microsoft.Extensions.Configuration;
using Syscaf.Common.Utils;
using Syscaf.Data.Helpers.Settings;
using System;

using System.Data;
using System.Globalization;

namespace Syscaf.Common.Helpers
{
    public static class Constants 
    {
        private static  IConfiguration ConfigurationManager;
     
        private static DateTime? timeNull = null;

        //TimeZoneInfo multiplataforma
        public static TimeZoneInfo timezone = null;


        public static void Inicializar(IConfiguration configuration)
        {
             ConfigurationManager = configuration;         
         
            timezone = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.GetSection("Constants")["NameZone"]);
            timeCache = ConfigurationManager.GetSection("Constants")["timeCache"];
            Dominio = ConfigurationManager.GetSection("Constants")["Dominio"];
        }
 
        public static DateTimeFormatInfo CultureDate
        {
            get
            { return CultureInfo.CreateSpecificCulture("es-EN").DateTimeFormat;
            }
        }
        public static string usuario = "usuario_{0}";

        public static string menu = "menu";
        public static string opciones = "opciones";
        public static string FiltroClienteUsuario = "FiltroClienteUsuario";
        public static string timeCache = null ;
        public static StringComparison comparer = StringComparison.CurrentCultureIgnoreCase;

        public static string FormatoFechaHora = "yyyy/MM/dd HH:mm";
        public static string FormatoFechaHoraSeg = "yyyy/MM/dd HH:mm:ss";
        public static string FormatoPegaso = "yyyy-MM-dd HH:mm:ss";
        public static string FormatoSinceToken = "yyyyMMddHHmmssfff";
        public static string Dominio = "";
        public static string FormatoHoraPacifico = "dd/MM/yyyy";
        public static string FormatoHoraPacificoHMS = "dd/MM/yyyy HH:mm:ss";
        public static string FormatoyyyyMMddHHmmss = "yyyyMMddHHmmss";
        

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

        // parametro de llamadas por minuto
        public static int CallsMin
        {
            get
            {
                int.TryParse(ConfigurationManager.GetSection("Constants")["CallsMix"], out int calls);
                calls = (calls == 0) ? 20 : calls;
                return calls;
            }
        }

        // parametro de llamadas por minuto
        public static int CallsHour
        {
            get
            {
                int.TryParse(ConfigurationManager.GetSection("Constants")["CallsMixHour"], out int calls);
                calls = (calls == 0) ? 500 : calls;
                return calls;
            }
        }

        public static int SecondMinute = 60;


        public static string ReporteErroresViajesUsos = "ErroresViajesUsos";


        // trae la infomracion del datatable para el img
        public static DataTable GetDataTableIMG()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("TripNo", typeof(long)));
            dt.Columns.Add(new DataColumn("Cliente", typeof(string)));
            dt.Columns.Add(new DataColumn("US", typeof(string)));
            dt.Columns.Add(new DataColumn("VehicleID", typeof(int)));
            dt.Columns.Add(new DataColumn("Placa", typeof(string)));
            dt.Columns.Add(new DataColumn("VehicleSiteID", typeof(long)));
            dt.Columns.Add(new DataColumn("VehicleSiteName", typeof(string)));
            dt.Columns.Add(new DataColumn("DriverID", typeof(long)));
            dt.Columns.Add(new DataColumn("DriverName", typeof(string)));
            dt.Columns.Add(new DataColumn("DriverSiteID", typeof(long)));
            dt.Columns.Add(new DataColumn("DriverSiteName", typeof(string)));
            dt.Columns.Add(new DataColumn("OriginalDriverID", typeof(int)));
            dt.Columns.Add(new DataColumn("OriginalDriverName", typeof(string)));
            dt.Columns.Add(new DataColumn("TripStart", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("TripEnd", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("CategoryID", typeof(int)));
            dt.Columns.Add(new DataColumn("Notes", typeof(string)));
            dt.Columns.Add(new DataColumn("StartSubTripSeq", typeof(int)));
            dt.Columns.Add(new DataColumn("EndSubTripSeq", typeof(int)));
            dt.Columns.Add(new DataColumn("TripDistance", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Odometer", typeof(decimal)));
            dt.Columns.Add(new DataColumn("MaxSpeed", typeof(int)));
            dt.Columns.Add(new DataColumn("SpeedTime", typeof(int)));
            dt.Columns.Add(new DataColumn("SpeedOccurs", typeof(int)));
            dt.Columns.Add(new DataColumn("MaxBrake", typeof(int)));
            dt.Columns.Add(new DataColumn("BrakeTime", typeof(int)));
            dt.Columns.Add(new DataColumn("BrakeOccurs", typeof(int)));
            dt.Columns.Add(new DataColumn("MaxAccel", typeof(int)));
            dt.Columns.Add(new DataColumn("AccelTime", typeof(int)));
            dt.Columns.Add(new DataColumn("AccelOccurs", typeof(int)));
            dt.Columns.Add(new DataColumn("MaxRPM", typeof(int)));
            dt.Columns.Add(new DataColumn("RPMTime", typeof(int)));
            dt.Columns.Add(new DataColumn("RPMOccurs", typeof(int)));
            dt.Columns.Add(new DataColumn("GBTime", typeof(int)));
            dt.Columns.Add(new DataColumn("ExIdleTime", typeof(int)));
            dt.Columns.Add(new DataColumn("ExIdleOccurs", typeof(int)));
            dt.Columns.Add(new DataColumn("NIdleTime", typeof(int)));
            dt.Columns.Add(new DataColumn("NIdleOccurs", typeof(int)));
            dt.Columns.Add(new DataColumn("StandingTime", typeof(int)));
            dt.Columns.Add(new DataColumn("Litres", typeof(decimal)));
            dt.Columns.Add(new DataColumn("StartGPSID", typeof(long)));
            dt.Columns.Add(new DataColumn("EndGPSID", typeof(long)));
            dt.Columns.Add(new DataColumn("StartEngineSeconds", typeof(int)));
            dt.Columns.Add(new DataColumn("EndEngineSeconds", typeof(int)));
            dt.Columns.Add(new DataColumn("ClienteIdS", typeof(int)));

            return dt;


        }


        public static DataTable GetDataTableIdentity()
        {

            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Id", typeof(long)));

            return dt;

        }
    

        public static string TraceLabelMes = "TraceLabelMes";

        #region "eBus"
        public static string ebus_seleccion_cliente = "ebus_seleccion_cliente_{0}";
        #endregion

       

    }
}