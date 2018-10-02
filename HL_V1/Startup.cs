using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HL_V1.Startup))]
namespace HL_V1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
