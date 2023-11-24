using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AAS2237A5.Startup))]

namespace AAS2237A5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
