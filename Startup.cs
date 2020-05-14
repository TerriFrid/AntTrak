using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AntTrak.Startup))]
namespace AntTrak
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
