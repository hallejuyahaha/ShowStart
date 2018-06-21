using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShowStartWeb.Startup))]
namespace ShowStartWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
