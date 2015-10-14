using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace AzureADSample
{
    public partial class Startup
    {
        //
        // The Client ID is used by the application to uniquely identify itself to Azure AD.
        // The Metadata Address is used by the application to retrieve the signing keys used by Azure AD.
        // The AAD Instance is the instance of Azure, for example public Azure or Azure China.
        // The Authority is the sign-in URL of the tenant.
        // The Post Logout Redirect Uri is the URL where the user will be redirected after they sign out.
        //
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        // NOTE: [ap] different types of notifications

                        AuthenticationFailed = context =>
                        {
                            try
                            {
                                context.HandleResponse();
                                context.Response.Redirect("/Error?message=" + context.Exception.Message);
                                return Task.FromResult(0);
                            }
                            catch (Exception ex)
                            {
                                
                                throw;
                            }
                        },

                        // we use this notification for injecting our custom logic
                        SecurityTokenValidated = (context) =>
                        {
                            try
                            {
                                // retriever caller data from the incoming principal
                                string issuer = context.AuthenticationTicket.Identity.FindFirst("iss").Value;
                                string UPN = context.AuthenticationTicket.Identity.FindFirst(System.IdentityModel.Claims.ClaimTypes.Name).Value;
                                string tenantID = context.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;


                                ClaimsIdentity claimsId = context.AuthenticationTicket.Identity;
                                claimsId.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Admin", ClaimValueTypes.String));

                                return Task.FromResult(0);
                            }
                            catch (Exception ex)
                            {
                                
                                throw;
                            }

                        },

                    }
                });
        }
    }
}