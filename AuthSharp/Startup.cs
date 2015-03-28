using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AuthSharp.Startup))]
namespace AuthSharp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
