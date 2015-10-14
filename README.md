# AzureActiveDirectoryHandsOn
Repozitorij u svrhu predavanja MS Community Dev UG Rijeka

	1. Register an application in Azure
	2. Create a web project without authentication
	3. Install packages
		Install-Package Microsoft.Owin.Security.OpenIdConnect
		Install-Package Microsoft.Owin.Security.Cookies
		Install-Package Microsoft.Owin.Host.SystemWeb
		Install-Package Microsoft.IdentityModel.Clients.ActiveDirectory
		Install-Package Microsoft.Azure.ActiveDirectory.GraphClient
	
	4. Add Owin startup class
	5. Make a partial class with Configure method
	6. Write Code for Startup.Auth.cs
		a. Insert values for following things:
				<add key="ida:ClientId" value="" />
				<add key="ida:Key" value="" />
				<add key="ida:Tenant" value="" />
				<add key="ida:AADInstance" value="" />
				<add key="ida:PostLogoutRedirectUri" value="" />
	7. Enable https on a project and setup virtual directory
	8. Set [Authorize] attribute
	9. Copy/paste code regarding Graph API
