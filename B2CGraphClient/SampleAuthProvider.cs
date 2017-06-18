using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Resources;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace B2CGraphShell
{

        public interface IAuthProvider
        {
            Task<string> GetUserAccessTokenAsync();
        Task<Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult> GetOldUserAccessTokenAsync();
        }
    public sealed class SampleAuthProvider : IAuthProvider
    {
        private string clientId = ConfigurationManager.AppSettings["b2c:ClientId"];
        private string clientSecret = ConfigurationManager.AppSettings["b2c:ClientSecret"];
        private string tenant = ConfigurationManager.AppSettings["b2c:Tenant"];
        
        // Properties used to get and manage an access token.
        private string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private string appId = ConfigurationManager.AppSettings["ida:AppId"];
        private string appSecret = ConfigurationManager.AppSettings["ida:AppSecret"];
        private string nonAdminScopes = ConfigurationManager.AppSettings["ida:NonAdminScopes"];
        private string adminScopes = ConfigurationManager.AppSettings["ida:AdminScopes"];
        private AuthenticationContext authContext;
        private Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential credential;
        //private SessionTokenCache tokenCache { get; set; }
        private string url { get; set; }

        private static readonly SampleAuthProvider instance = new SampleAuthProvider();
        private SampleAuthProvider() { }

        public static SampleAuthProvider Instance
        {
            get
            {
                return instance;
            }
        }
        public async Task<Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult> GetOldUserAccessTokenAsync()
        {
            // The AuthenticationContext is ADAL's primary class, in which you indicate the direcotry to use.
            this.authContext = new AuthenticationContext("https://login.microsoftonline.com/" + tenant);

            // The ClientCredential is where you pass in your client_id and client_secret, which are 
            // provided to Azure AD in order to receive an access_token using the app's identity.
            this.credential = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(clientId, clientSecret);
            // First, use ADAL to acquire a token using the app's identity (the credential)
            // The first parameter is the resource we want an access_token for; in this case, the Graph API.
            //Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult result = authContext.AcquireTokenAsync("https://graph.windows.net", credential).Result;
            return await authContext.AcquireTokenAsync("https://graph.windows.net", credential);
        }
        // Gets an access token and its expiration date. First tries to get the token from the token cache.
        public async Task<string> GetUserAccessTokenAsync()
        {
            throw new NotImplementedException();
            // Initialize the cache.
            //HttpContextBase context = HttpContext.Current.GetOwinContext().Environment["System.Web.HttpContextBase"] as HttpContextBase;
            ////tokenCache = new SessionTokenCache(
            ////    ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value,
            ////    context);
            ////var cachedItems = tokenCache.ReadItems(appId); // see what's in the cache

            //if (!redirectUri.EndsWith("/")) redirectUri = redirectUri + "/";
            //string[] segments = context.Request.Path.Split(new char[] { '/' });
            //ConfidentialClientApplication cca = new ConfidentialClientApplication(
            //    appId,
            //    redirectUri + segments[1],
            //    new Microsoft.Identity.Client.ClientCredential(appSecret),
            //    tokenCache);
            //bool? isAdmin = HttpContext.Current.Session["IsAdmin"] as bool?;

            //string allScopes = nonAdminScopes;
            //if (isAdmin.GetValueOrDefault())
            //{
            //    allScopes += " " + adminScopes;
            //}
            //string[] scopes = allScopes.Split(new char[] { ' ' });
            //try
            //{
            //    Microsoft.Identity.Client.AuthenticationResult result = await cca.AcquireTokenSilentAsync(scopes);
            //    return result.Token;
            //}

            //// Unable to retrieve the access token silently.
            //catch (MsalSilentTokenAcquisitionException)
            //{
            //    HttpContext.Current.Request.GetOwinContext().Authentication.Challenge(
            //      new AuthenticationProperties() { RedirectUri = redirectUri + segments[1] },
            //      OpenIdConnectAuthenticationDefaults.AuthenticationType);

            //    throw new ServiceException(
            //        new Error
            //        {
            //            Code = GraphErrorCode.AuthenticationFailure.ToString(),
            //            Message = Resource.Error_AuthChallengeNeeded,
            //        });
            //}
        }
    }
}
