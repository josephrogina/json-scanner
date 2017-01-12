using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JSONScanner.Startup))]
namespace JSONScanner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
