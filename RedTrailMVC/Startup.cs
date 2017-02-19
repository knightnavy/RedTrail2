using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RedTrailMVC.Startup))]
namespace RedTrailMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
