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
			i. <add key="ida:ClientId" value="" />
<add key="ida:Key" value="" />
<add key="ida:Tenant" value="" />
<add key="ida:AADInstance" value="" />
<add key="ida:PostLogoutRedirectUri" value="" />
			
	7. Enable https on a project and setup virtual directory
	8. Set [Authorize] attribute
	9. Copy/paste code for rest of the stuff regarding Graph API


You'll maybe need this:
 <compilation debug="true" targetFramework="4.5" >
      <assemblies>
        <add assembly="System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
      </assemblies>
    </compilation>

