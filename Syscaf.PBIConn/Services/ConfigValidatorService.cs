using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.PBIConn.Services
{
    public static class ConfigValidatorService
    {
        private static IConfiguration ConfigurationManager;

        public static string m_authorityUrl;
        public static string[] m_scope;
        public static string urlPowerBiServiceApiRoot;
        public static string ApplicationId;
        public static Guid WorkspaceId;
        public static Guid ReportId;
        public static string DatasetId_858;
        public static string DatasetId_914;
        public static string DatasetId_915;
        public static string AuthenticationType;
        public static string ApplicationSecret;
        public static string Tenant;
        public static string Username;
        public static string Password;

        public static void Inicializar(IConfiguration configuration)
        {
            ConfigurationManager = configuration;

            m_authorityUrl = ConfigurationManager.GetSection("PwParams")["authorityUrl"];
            m_scope = ConfigurationManager.GetSection("PwParams")["scope"].Split(';');
            urlPowerBiServiceApiRoot = ConfigurationManager.GetSection("PwParams")["urlPowerBiServiceApiRoot"];
            ApplicationId = ConfigurationManager.GetSection("PwParams")["applicationId"];
            WorkspaceId = GetParamGuid(ConfigurationManager.GetSection("PwParams")["workspaceId"]);
            ReportId = GetParamGuid(ConfigurationManager.GetSection("PwParams")["reportId"]);
            DatasetId_858 = ConfigurationManager.GetSection("PwParams")["DatasetId"];
            DatasetId_914 = ConfigurationManager.GetSection("PwParams")["DatasetIdSomosF"];
            DatasetId_915 = ConfigurationManager.GetSection("PwParams")["DatasetIdSomosU"];
            AuthenticationType = ConfigurationManager.GetSection("PwParams")["authenticationType"];
            ApplicationSecret = ConfigurationManager.GetSection("PwParams")["applicationSecret"];
            Tenant = ConfigurationManager.GetSection("PwParams")["tenant"];
            Username = ConfigurationManager.GetSection("PwParams")["pbiUsername"];
            Password = ConfigurationManager.GetSection("PwParams")["pbiPassword"];
        }

        /// <summary>
        /// Check if web.config embed parameters have valid values.
        /// </summary>
        /// <returns>Null if web.config parameters are valid, otherwise returns specific error string.</returns>
        public static string GetWebConfigErrors()
        {
            string message = null;
            Guid result;

            // Application Id must have a value.
            if (string.IsNullOrWhiteSpace(ApplicationId))
            {
                message = "ApplicationId is empty. please register your application as Native app in https://dev.powerbi.com/apps and fill client Id in web.config.";
            }
            // Application Id must be a Guid object.
            else if (!Guid.TryParse(ApplicationId, out result))
            {
                message = "ApplicationId must be a Guid object. please register your application as Native app in https://dev.powerbi.com/apps and fill application Id in web.config.";
            }
            // Workspace Id must have a value.
            else if (WorkspaceId == Guid.Empty)
            {
                message = "WorkspaceId is empty or not a valid Guid. Please fill its Id correctly in web.config";
            }
            // Report Id must have a value.
            else if (ReportId == Guid.Empty)
            {
                message = "ReportId is empty or not a valid Guid. Please fill its Id correctly in web.config";
            }
            else if (AuthenticationType.Equals("masteruser", StringComparison.InvariantCultureIgnoreCase))
            {
                // Username must have a value.
                if (string.IsNullOrWhiteSpace(Username))
                {
                    message = "Username is empty. Please fill Power BI username in web.config";
                }

                // Password must have a value.
                if (string.IsNullOrWhiteSpace(Password))
                {
                    message = "Password is empty. Please fill password of Power BI username in web.config";
                }
            }
            else if (AuthenticationType.Equals("serviceprincipal", StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(ApplicationSecret))
                {
                    message = "ApplicationSecret is empty. please register your application as Web app and fill appSecret in web.config.";
                }
                // Must fill tenant Id
                else if (string.IsNullOrWhiteSpace(Tenant))
                {
                    message = "Invalid Tenant. Please fill Tenant ID in Tenant under web.config";
                }
            }
            else
            {
                message = "Invalid authentication type";
            }

            return message;
        }

        private static Guid GetParamGuid(string param)
        {
            Guid paramGuid = Guid.Empty;
            Guid.TryParse(param, out paramGuid);
            return paramGuid;
        }
    }
}
