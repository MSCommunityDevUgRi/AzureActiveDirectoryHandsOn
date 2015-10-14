using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Enigma.Startup))]
namespace Enigma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
