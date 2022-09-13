using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.PBIConn.Services
{
    public class AadService
    {
       

        /// <summary>
        /// Get Access token
        /// </summary>
        /// <returns>Access token</returns>
        public static async Task<string> GetAccessToken()
        {
            AuthenticationResult authenticationResult = null;
            if (ConfigValidatorService.AuthenticationType.Equals("masteruser", StringComparison.InvariantCultureIgnoreCase))
            {
                IPublicClientApplication clientApp = PublicClientApplicationBuilder
                                                                    .Create(ConfigValidatorService.ApplicationId)
                                                                    .WithAuthority(ConfigValidatorService.m_authorityUrl)
                                                                    .Build();
                var userAccounts = await clientApp.GetAccountsAsync();

                try
                {
                    authenticationResult = await clientApp.AcquireTokenSilent(ConfigValidatorService.m_scope, userAccounts.FirstOrDefault()).ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    SecureString secureStringPassword = new SecureString();
                    foreach (var key in ConfigValidatorService.Password)
                    {
                        secureStringPassword.AppendChar(key);
                    }
                    authenticationResult = await clientApp.AcquireTokenByUsernamePassword(ConfigValidatorService.m_scope, ConfigValidatorService.Username, secureStringPassword).ExecuteAsync();
                }
            }



            // Service Principal auth is recommended by Microsoft to achieve App Owns Data Power BI embedding
            else if (ConfigValidatorService.AuthenticationType.Equals("serviceprincipal", StringComparison.InvariantCultureIgnoreCase))
            {
                // For app only authentication, we need the specific tenant id in the authority url
                var tenantSpecificURL = ConfigValidatorService.m_authorityUrl.Replace("organizations", ConfigValidatorService.Tenant);

                IConfidentialClientApplication clientApp = ConfidentialClientApplicationBuilder
                                                                                .Create(ConfigValidatorService.ApplicationId)
                                                                                .WithClientSecret(ConfigValidatorService.ApplicationSecret)
                                                                                .WithAuthority(tenantSpecificURL)
                                                                                .Build();

                authenticationResult = await clientApp.AcquireTokenForClient(ConfigValidatorService.m_scope).ExecuteAsync();
            }
           
            return authenticationResult.AccessToken;
        }
    }
}
