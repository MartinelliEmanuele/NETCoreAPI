using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Utils;

namespace GilbarcoReminders.Utils
{
    public class GraphHelper
    {
        private static GraphServiceClient _graphServiceClient;
        public  static GraphServiceClient GetAuthenticatedGraphClient()
        {
            var authenticationProvider = CreateAuthorizationProvider();
            _graphServiceClient = new GraphServiceClient(authenticationProvider);
            return _graphServiceClient;
        }

        public static IAuthenticationProvider CreateAuthorizationProvider()
        {
#if(DEBUG)
            var clientId = ResourceFactory.GetAppSetting("LocalAzureADAppClientId");
            var clientSecret = ResourceFactory.GetAppSetting("LocalAzureADAppClientSecret");
            var tenantId = ResourceFactory.GetAppSetting("LocalAzureADAppTenantId");
#else
            var clientId = ResourceFactory.GetAppSetting("AzureADAppClientId");
            var clientSecret = ResourceFactory.GetAppSetting("AzureADAppClientSecret");
            var tenantId = ResourceFactory.GetAppSetting("AzureADAppTenantId");
#endif
            var authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";

            List<string> scopes = new List<string>();
            scopes.Add("https://graph.microsoft.com/.default");

            var cca = ConfidentialClientApplicationBuilder.Create(clientId)
                                                          .WithAuthority(authority)
                                                          .WithClientSecret(clientSecret)
                                                          .Build();

            return new MsalAuthenticationProvider(cca, scopes.ToArray());
        }
    }
}
