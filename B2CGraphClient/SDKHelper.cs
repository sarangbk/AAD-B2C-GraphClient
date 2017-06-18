using Microsoft.Graph;
using System.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using System.Configuration;

namespace B2CGraphShell
{
    public class SDKHelper
    {
        private static GraphServiceClient graphClient = null;

        // Get an authenticated Microsoft Graph Service client.
        public static GraphServiceClient GetAuthenticatedClient(string graphEndpoint)
        {
            GraphServiceClient graphClient = new GraphServiceClient(
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        //string accessToken = await SampleAuthProvider.Instance.GetUserAccessTokenAsync();
                        Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult authResult = await (SampleAuthProvider.Instance.GetOldUserAccessTokenAsync());
                        // Append the access token to the request.
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                        requestMessage.RequestUri = new Uri("https://graph.windows.net/" + ConfigurationManager.AppSettings["b2c:Tenant"] + "/" + graphEndpoint + "/ " + "?" + "api-version=1.6");
                        // Get event times in the current time zone.
                        //requestMessage.Headers.Add("Prefer", "outlook.timezone=\"" + TimeZoneInfo.Local.Id + "\"");

                        // This header has been added to identify our sample in the Microsoft Graph service. If extracting this code for your project please remove.
                        //requestMessage.Headers.Add("SampleID", "aspnet-snippets-sample");
                    }));
            return graphClient;
        }

        public static void SignOutClient()
        {
            graphClient = null;
        }
    }
}
