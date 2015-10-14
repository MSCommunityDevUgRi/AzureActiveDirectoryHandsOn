using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using Microsoft.Owin.Security.OpenIdConnect;

namespace AzureADSample.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        ///     Gets a list of <see cref="Contact" /> objects from Graph.
        /// </summary>
        /// <returns>A view with the list of <see cref="Contact" /> objects.</returns>
        public async Task<ActionResult> Index()
        {
            var userList = new List<User>();
            try
            {
                ActiveDirectoryClient client = GetActiveDirectoryClient();
                IPagedCollection<IUser> pagedCollection = await client.Users.ExecuteAsync();
                if (pagedCollection != null)
                {
                    do
                    {
                        List<IUser> usersList = pagedCollection.CurrentPage.ToList();
                        foreach (IUser user in usersList)
                        {
                            userList.Add((User)user);
                        }
                        pagedCollection = await pagedCollection.GetNextPageAsync();
                    } while (pagedCollection != null);
                }
            }
            catch (Exception e)
            {
                if (Request.QueryString["reauth"] == "True")
                {
                    //
                    // Send an OpenID Connect sign-in request to get a new set of tokens.
                    // If the user still has a valid session with Azure AD, they will not be prompted for their credentials.
                    // The OpenID Connect middleware will return to this controller after the sign-in response has been handled.
                    //
                    HttpContext.GetOwinContext()
                        .Authentication.Challenge(OpenIdConnectAuthenticationDefaults.AuthenticationType);
                }

                //
                // The user needs to re-authorize.  Show them a message to that effect.
                //
                ViewBag.ErrorMessage = "AuthorizationRequired";
                return View();
            }

            return View(userList);
        }

        // GET
        [HttpGet]
        public async Task<ActionResult> AddNewUser()
        {
            ViewBag.Message = "Add new User";
            return View();
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> AddNewUser(
            [Bind(
                Include =
                    "UserPrincipalName,AccountEnabled,PasswordProfile,MailNickname,DisplayName,GivenName,Surname,JobTitle,Department"
                )] User user)
        {
            ActiveDirectoryClient client = null;
            try
            {
                client = GetActiveDirectoryClient();
            }
            catch (Exception e)
            {
                if (Request.QueryString["reauth"] == "True")
                {
                    //
                    // Send an OpenID Connect sign-in request to get a new set of tokens.
                    // If the user still has a valid session with Azure AD, they will not be prompted for their credentials.
                    // The OpenID Connect middleware will return to this controller after the sign-in response has been handled.
                    //
                    HttpContext.GetOwinContext()
                        .Authentication.Challenge(OpenIdConnectAuthenticationDefaults.AuthenticationType);
                }

                //
                // The user needs to re-authorize.  Show them a message to that effect.
                //
                ViewBag.ErrorMessage = "AuthorizationRequired";
                return View();
            }

            try
            {
                await client.Users.AddUserAsync(user);
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", exception.Message);
                return View();
            }
        }

        private ActiveDirectoryClient GetActiveDirectoryClient()
        {
            Uri baseServiceUri = new Uri("https://graph.windows.net" + '/' + ConfigurationManager.AppSettings["ida:Tenant"]);
            ActiveDirectoryClient activeDirectoryClient =
                new ActiveDirectoryClient(new Uri(baseServiceUri, ConfigurationManager.AppSettings["ida:Tenant"]),
                    async () => await AcquireTokenAsyncForApplication());
            return activeDirectoryClient;
        }

        /// <summary>
        /// Async task to acquire token for Application.
        /// </summary>
        /// <returns>Async Token for application.</returns>
        private static async Task<string> AcquireTokenAsyncForApplication()
        {
            AuthenticationContext authenticationContext = new AuthenticationContext("https://login.windows.net/" + ConfigurationManager.AppSettings["ida:Tenant"], false);
            ClientCredential clientCred = new ClientCredential(ConfigurationManager.AppSettings["ida:ClientId"], ConfigurationManager.AppSettings["ida:Key"]);
            AuthenticationResult authenticationResult = authenticationContext.AcquireToken("https://graph.windows.net",
                clientCred);
            return authenticationResult.AccessToken;
        }
    }
}